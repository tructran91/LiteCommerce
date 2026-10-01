using AutoMapper;
using Catalog.Application.ActivityLogs.Queries;
using Catalog.Application.Responses;
using Catalog.Core.DTOs;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ActivityLogs.Handlers
{
    public class GetAllActivityLogsHandler : IRequestHandler<GetAllActivityLogsQuery, BaseResponse<List<ActivityLogResponse>>>
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public GetAllActivityLogsHandler(IActivityLogRepository activityLogRepository, IMapper mapper)
        {
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ActivityLogResponse>>> Handle(GetAllActivityLogsQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ActivityLogFilter>(request);
            var (activityLogs, totalRecords) = await _activityLogRepository.GetPagedAsync(filter, request.CurrentPage, request.PageSize);

            var activityLogResponses = _mapper.Map<List<ActivityLogResponse>>(activityLogs);
            var response = BaseResponse<List<ActivityLogResponse>>.Success(activityLogResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}
