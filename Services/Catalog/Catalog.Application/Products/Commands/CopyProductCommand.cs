using Catalog.Application.Behaviors;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class CopyProductCommand : IRequest<BaseResponse<ProductResponse>>, IActivityLoggable
    {
        public CopyProductCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }

        public string? EntityDisplayName { get; set; }

        public string? LogDescription { get; set; }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "Product";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => EntityDisplayName;

        string? IActivityLoggable.ActivityDescription => LogDescription;
    }
}
