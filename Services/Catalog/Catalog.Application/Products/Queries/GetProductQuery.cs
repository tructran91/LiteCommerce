﻿using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Products.Queries
{
    public class GetProductQuery : IRequest<BaseResponse<ProductResponse>>
    {
        public Guid Id { get; set; }

        public GetProductQuery(Guid id)
        {
            Id = id;
        }
    }
}
