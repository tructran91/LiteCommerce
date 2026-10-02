using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Database.Queries
{
    public class GetSeedProfilesQuery : IRequest<BaseResponse<List<string>>>
    {
    }
}
