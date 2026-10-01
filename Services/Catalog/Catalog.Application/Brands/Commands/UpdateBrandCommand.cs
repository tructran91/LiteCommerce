using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Commands
{
    public class UpdateBrandCommand : IRequest<BaseResponse<BrandResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateBrandRequest Payload { get; set; }

        public UpdateBrandCommand(string id, UpdateBrandRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "Brand";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}
