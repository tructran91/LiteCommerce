using Identity.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Services
{
    public class LoggingPasswordResetNotifier : IPasswordResetNotifier
    {
        private readonly ILogger<LoggingPasswordResetNotifier> _logger;

        public LoggingPasswordResetNotifier(ILogger<LoggingPasswordResetNotifier> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            _logger.LogWarning(
                "No email provider is configured. Password reset token for {Email}: {Token}",
                email,
                token);
            return Task.CompletedTask;
        }
    }
}
