using Identity.Core.Enums;

namespace Identity.Application.Interfaces
{
    public record ActivityLogEntry(
        ActivityAction Action,
        bool IsSuccess,
        Guid? UserId,
        string? Email,
        ActivityFailureReason? FailureReason = null);

    // Called by AuthService after its own SaveChanges. Never throws: a failed write is only logged,
    // so the auth response is never turned into an error by the log.
    public interface IActivityLogger
    {
        Task LogAsync(ActivityLogEntry entry, CancellationToken cancellationToken = default);
    }
}
