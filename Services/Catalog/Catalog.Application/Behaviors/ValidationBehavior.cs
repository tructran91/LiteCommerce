using FluentValidation;
using MediatR;

namespace Catalog.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var errorsDictionary = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .GroupBy(
                    x => ToClientFieldName(x.PropertyName),
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);

            if (errorsDictionary.Any())
            {
                throw new Catalog.Application.Exceptions.ValidationException(errorsDictionary);
            }

            return await next();
        }

        // "Payload.Product.CategoryIds[0]" -> "product.categoryIds[0]", the field name the client sent.
        private static string ToClientFieldName(string propertyName)
        {
            const string payloadPrefix = "Payload.";
            if (propertyName.StartsWith(payloadPrefix, StringComparison.Ordinal))
            {
                propertyName = propertyName[payloadPrefix.Length..];
            }

            var segments = propertyName
                .Split('.')
                .Select(s => s.Length == 0 ? s : char.ToLowerInvariant(s[0]) + s[1..]);

            return string.Join('.', segments);
        }
    }
}
