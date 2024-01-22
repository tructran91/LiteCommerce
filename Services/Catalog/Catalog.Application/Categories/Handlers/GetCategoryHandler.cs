﻿using AutoMapper;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Categories.Handlers
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, BaseResponse<CategoryResponse>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryHandler(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category is null)
            {
                return BaseResponse<CategoryResponse>.Failure("Category does not exist.", null);
            }

            var categoryMapping = _mapper.Map<CategoryResponse>(category);
            return BaseResponse<CategoryResponse>.Success(categoryMapping);
        }
    }
}
