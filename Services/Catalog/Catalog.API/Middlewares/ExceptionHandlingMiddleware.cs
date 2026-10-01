using Catalog.API.Extensions;
using Catalog.Application.Exceptions;
using Catalog.Core.Exceptions;
using LiteCommerce.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Catalog.API.Middlewares
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private const int SqlUniqueIndexViolation = 2601;
        private const int SqlUniqueConstraintViolation = 2627;

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                var statusCode = GetStatusCode(e);

                if (statusCode == HttpStatusCode.InternalServerError)
                    _logger.LogError(e, "Unhandled exception on {Method} {Path}", context.Request.Method, context.Request.Path);
                else
                    _logger.LogWarning("Request {Method} {Path} failed with {StatusCode}: {Message}",
                        context.Request.Method, context.Request.Path, (int)statusCode, e.Message);

                await HandleExceptionAsync(context, e, statusCode);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, HttpStatusCode statusCode)
        {
            var response = BaseResponse<object>.Failure(
                GetMessage(exception, statusCode),
                errors: GetErrors(exception),
                statusCode: statusCode);

            httpContext.Response.StatusCode = (int)statusCode;

            await httpContext.Response.WriteAsJsonAsync(response, ErrorJson.Options);
        }

        private static HttpStatusCode GetStatusCode(Exception exception) =>
            exception switch
            {
                ValidationException => HttpStatusCode.BadRequest,
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                DbUpdateException { InnerException: SqlException { Number: SqlUniqueIndexViolation or SqlUniqueConstraintViolation } }
                    => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

        private static string GetMessage(Exception exception, HttpStatusCode statusCode) =>
            statusCode switch
            {
                HttpStatusCode.Conflict when exception is DbUpdateException => "A record with the same unique value already exists.",
                // Details stay in the log, never in the response.
                HttpStatusCode.InternalServerError => "An unexpected error occurred.",
                _ => exception.Message
            };

        private static Dictionary<string, List<string>>? GetErrors(Exception exception) =>
            exception is ValidationException validationException
                ? validationException.ErrorsDictionary.ToDictionary(x => x.Key, x => x.Value.ToList())
                : null;
    }
}
