﻿using AutoMapper;
using Catalog.Application.Brands.Commands;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Application.Categories.Handlers
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, BaseResponse<CategoryResponse>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryHandler> _logger;

        public UpdateCategoryHandler(IRepository<Category> categoryRepository, IMapper mapper, ILogger<UpdateCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateCategoryHandler: {JsonSerializer.Serialize(request.Payload)}");

            var existingCategory = await _categoryRepository.GetByIdAsync(Guid.Parse(request.Payload.Id));
            if (existingCategory == null)
            {
                return BaseResponse<CategoryResponse>.Failure("Category does not exist.", null);
            }

            _mapper.Map(request.Payload, existingCategory);
            existingCategory.Slug = existingCategory.Name.Slugify();

            await _categoryRepository.UpdateAsync(existingCategory);

            var response = _mapper.Map<CategoryResponse>(existingCategory);

            return BaseResponse<CategoryResponse>.Success(response);
        }
    }
}
