using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class UploadContentImageCommand : IRequest<BaseResponse<string>>, IActivityLoggable
    {
        public UploadContentImageRequest Payload { get; set; }

        public UploadContentImageCommand(UploadContentImageRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Upload;

        string IActivityLoggable.ActivityEntityName => "Product";

        string? IActivityLoggable.ActivityEntityId => Payload.IsNewProduct ? null : Payload.ProductId;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.File?.FileName;
    }
}
