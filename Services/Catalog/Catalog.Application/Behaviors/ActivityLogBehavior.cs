using Catalog.Application.Services;
using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Behaviors
{
    // Registered after ValidationBehavior, so it only sees requests that passed validation.
    public class ActivityLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ActivityLogBehavior<TRequest, TResponse>> _logger;

        public ActivityLogBehavior(
            IActivityLogRepository activityLogRepository,
            ICurrentUserService currentUserService,
            ILogger<ActivityLogBehavior<TRequest, TResponse>> logger)
        {
            _activityLogRepository = activityLogRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            if (request is not IActivityLoggable loggable || response is not IBaseResponse { IsSuccess: true } baseResponse)
            {
                return response;
            }

            var entityId = loggable.ActivityEntityId ?? ReadProperty(baseResponse.Data, "Id");
            var displayName = loggable.ActivityEntityDisplayName ?? ReadProperty(baseResponse.Data, "Name");

            var activityLog = new ActivityLog
            {
                Action = loggable.ActivityAction,
                EntityName = loggable.ActivityEntityName,
                EntityId = entityId,
                EntityDisplayName = Truncate(displayName, FieldLength.LogEntityDisplayName),
                Description = Truncate(BuildDescription(loggable.ActivityAction, loggable.ActivityEntityName, displayName, entityId), FieldLength.LogDescription)!,
                RequestName = typeof(TRequest).Name,
                UserName = _currentUserService.UserName,
                CorrelationId = _currentUserService.CorrelationId,
                Timestamp = DateTime.UtcNow
            };

            // The business change is already saved; a failed log write must not turn it into an error response.
            try
            {
                await _activityLogRepository.AddAsync(activityLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ActivityLogBehavior: failed to write activity log for {RequestName}", typeof(TRequest).Name);
            }

            return response;
        }

        private static string BuildDescription(ActivityAction action, string entityName, string? displayName, string? entityId)
        {
            var verb = action switch
            {
                ActivityAction.Create => "Created",
                ActivityAction.Update => "Updated",
                ActivityAction.Delete => "Deleted",
                ActivityAction.Upload => "Uploaded",
                ActivityAction.BulkUpdate => "Bulk updated",
                _ => action.ToString()
            };

            var target = displayName ?? entityId;
            return string.IsNullOrEmpty(target) ? $"{verb} {entityName}" : $"{verb} {entityName} '{target}'";
        }

        private static string? ReadProperty(object? data, string propertyName)
        {
            return data?.GetType().GetProperty(propertyName)?.GetValue(data)?.ToString();
        }

        private static string? Truncate(string? value, int maxLength)
        {
            return value is { Length: > 0 } && value.Length > maxLength ? value[..maxLength] : value;
        }
    }
}
