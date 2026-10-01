using Catalog.Core.Enums;

namespace Catalog.Application.Behaviors
{
    // Marks a command whose successful execution is recorded by ActivityLogBehavior.
    // Implement the members explicitly so they stay out of model binding and Swagger.
    public interface IActivityLoggable
    {
        ActivityAction ActivityAction { get; }

        string ActivityEntityName { get; }

        // Null when the id is only known after the handler runs; the behavior then reads Id from the response data.
        string? ActivityEntityId { get; }

        // Null when the command carries no name; the behavior then reads Name from the response data.
        // Delete commands expose a settable EntityDisplayName that the handler fills after loading the entity.
        string? ActivityEntityDisplayName { get; }
    }
}
