using Identity.Application.Interfaces;
using Identity.Core.Constants;
using Identity.Core.Entities;
using Identity.Core.Enums;
using Identity.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Services
{
    public class ActivityLogger : IActivityLogger
    {
        private readonly IdentityDbContext _db;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ActivityLogger> _logger;

        public ActivityLogger(IdentityDbContext db, ICurrentUserService currentUserService, ILogger<ActivityLogger> logger)
        {
            _db = db;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task LogAsync(ActivityLogEntry entry, CancellationToken cancellationToken = default)
        {
            var activityLog = new ActivityLog
            {
                Id = Guid.NewGuid(),
                Action = entry.Action,
                IsSuccess = entry.IsSuccess,
                FailureReason = entry.FailureReason,
                UserId = entry.UserId,
                Email = Truncate(entry.Email, LogFieldLength.Email),
                Description = BuildDescription(entry),
                IpAddress = Truncate(_currentUserService.IpAddress, LogFieldLength.IpAddress),
                UserAgent = Truncate(_currentUserService.UserAgent, LogFieldLength.UserAgent),
                UserName = Truncate(_currentUserService.UserName, LogFieldLength.UserName)!,
                CorrelationId = _currentUserService.CorrelationId,
                Timestamp = DateTime.UtcNow
            };

            // Callers have already saved their own changes, so this SaveChanges only carries the log row.
            // The auth result is already decided: a failed log write must not turn it into a 500.
            try
            {
                _db.ActivityLogs.Add(activityLog);
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _db.Entry(activityLog).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                _logger.LogError(ex, "ActivityLogger: failed to write {Action} for {Email}", entry.Action, entry.Email);
            }
        }

        private static string BuildDescription(ActivityLogEntry entry)
        {
            return (entry.Action, entry.IsSuccess, entry.FailureReason) switch
            {
                (ActivityAction.Register, true, _) => "Registered a new account",
                (ActivityAction.Register, false, ActivityFailureReason.EmailAlreadyRegistered) => "Registration rejected: email already registered",

                (ActivityAction.Login, true, _) => "Logged in",
                (ActivityAction.Login, false, ActivityFailureReason.UnknownEmail) => "Login failed: no account with this email",
                (ActivityAction.Login, false, ActivityFailureReason.InvalidPassword) => "Login failed: wrong password",
                (ActivityAction.Login, false, ActivityFailureReason.LockedOut) => "Login rejected: account is locked",
                (ActivityAction.Login, false, ActivityFailureReason.InactiveUser) => "Login rejected: account is inactive",

                (ActivityAction.AccountLocked, _, _) => "Account locked after too many failed logins",
                (ActivityAction.Logout, _, _) => "Logged out",

                (ActivityAction.ChangePassword, true, _) => "Changed password; all sessions were revoked",
                (ActivityAction.ChangePassword, false, ActivityFailureReason.WrongCurrentPassword) => "Password change rejected: wrong current password",

                (ActivityAction.TokenReuseDetected, _, _) => "Reused refresh token detected; all sessions were revoked",

                (ActivityAction.UpdateProfile, true, _) => "Updated profile",
                (ActivityAction.RevokeSession, true, _) => "Revoked a session",
                (ActivityAction.ForgotPassword, true, _) => "Requested a password reset",
                (ActivityAction.ResetPassword, true, _) => "Reset password; all sessions were revoked",
                (ActivityAction.ResetPassword, false, ActivityFailureReason.InvalidResetToken) => "Password reset rejected: invalid or expired token",
                (ActivityAction.AdminUnlockUser, true, _) => "Account unlocked by an administrator",
                (ActivityAction.AdminRevokeSessions, true, _) => "All sessions revoked by an administrator",

                _ => entry.IsSuccess ? entry.Action.ToString() : $"{entry.Action} failed: {entry.FailureReason}"
            };
        }

        private static string? Truncate(string? value, int maxLength)
            => value is not null && value.Length > maxLength ? value[..maxLength] : value;
    }
}
