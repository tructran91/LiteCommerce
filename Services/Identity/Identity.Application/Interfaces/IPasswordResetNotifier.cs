namespace Identity.Application.Interfaces
{
    public interface IPasswordResetNotifier
    {
        Task SendAsync(string email, string token, CancellationToken cancellationToken = default);
    }
}
