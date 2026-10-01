using AutoMapper;
using Catalog.Application.ActivityLogs.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.ActivityLogs.Handlers
{
    public class GetActivityLogHandler : IRequestHandler<GetActivityLogQuery, BaseResponse<ActivityLogResponse>>
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public GetActivityLogHandler(IActivityLogRepository activityLogRepository, IMapper mapper)
        {
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ActivityLogResponse>> Handle(GetActivityLogQuery request, CancellationToken cancellationToken)
        {
            var activityLog = await _activityLogRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (activityLog is null)
            {
                return BaseResponse<ActivityLogResponse>.Failure("Activity log does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            return BaseResponse<ActivityLogResponse>.Success(_mapper.Map<ActivityLogResponse>(activityLog));
        }
    }
}
