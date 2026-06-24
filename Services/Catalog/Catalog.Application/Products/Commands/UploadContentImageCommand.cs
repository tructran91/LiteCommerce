using Catalog.Application.Requests;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class UploadContentImageCommand : IRequest<BaseResponse<string>>
    {
        public UploadContentImageRequest Payload { get; set; }

        public UploadContentImageCommand(UploadContentImageRequest payload)
        {
            Payload = payload;
        }
    }
}
