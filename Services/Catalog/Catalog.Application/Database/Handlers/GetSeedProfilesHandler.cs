using Catalog.Application.Database.Queries;
using Catalog.Application.Services;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Database.Handlers
{
    public class GetSeedProfilesHandler : IRequestHandler<GetSeedProfilesQuery, BaseResponse<List<string>>>
    {
        private readonly IDatabaseSeeder _seeder;

        public GetSeedProfilesHandler(IDatabaseSeeder seeder)
        {
            _seeder = seeder;
        }

        public Task<BaseResponse<List<string>>> Handle(GetSeedProfilesQuery request, CancellationToken cancellationToken)
        {
            var profiles = _seeder.GetAvailableProfiles().ToList();
            return Task.FromResult(BaseResponse<List<string>>.Success(profiles));
        }
    }
}
