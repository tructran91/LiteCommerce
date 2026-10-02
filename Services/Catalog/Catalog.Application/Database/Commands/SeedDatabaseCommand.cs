using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Database.Commands
{
    public class SeedDatabaseCommand : IRequest<BaseResponse<SeedDatabaseResponse>>, IActivityLoggable
    {
        public SeedDatabaseRequest Payload { get; set; }

        public SeedDatabaseCommand(SeedDatabaseRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "Database";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => $"seed:{Payload.Profile}";
    }
}
