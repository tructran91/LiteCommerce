namespace LiteCommerce.Shared.Models
{
    // Non-generic view of BaseResponse<T> for pipeline code that does not know T.
    public interface IBaseResponse
    {
        bool IsSuccess { get; }

        object? Data { get; }
    }
}
