using AutoMapper;
using Catalog.Application.Database.Commands;
using Catalog.Application.Responses;
using Catalog.Application.Services;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Database.Handlers
{
    public class SeedDatabaseHandler : IRequestHandler<SeedDatabaseCommand, BaseResponse<SeedDatabaseResponse>>
    {
        private readonly IDatabaseSeeder _seeder;
        private readonly IMapper _mapper;
        private readonly ILogger<SeedDatabaseHandler> _logger;

        public SeedDatabaseHandler(IDatabaseSeeder seeder, IMapper mapper, ILogger<SeedDatabaseHandler> logger)
        {
            _seeder = seeder;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<SeedDatabaseResponse>> Handle(SeedDatabaseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SeedDatabaseHandler: Profile={Profile}", request.Payload.Profile);

            // SeedDatabaseValidator has already rejected unknown profiles.
            var result = await _seeder.SeedAsync(request.Payload.Profile, cancellationToken);
            var response = _mapper.Map<SeedDatabaseResponse>(result);
            return BaseResponse<SeedDatabaseResponse>.Success(response, $"Database seeded with '{result.Profile}' profile.");
        }
    }
}
