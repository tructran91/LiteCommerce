using AutoMapper;
using Catalog.Application.AuditLogs.Queries;
using Catalog.Application.Responses;
using Catalog.Core.DTOs;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.AuditLogs.Handlers
{
    public class GetAllAuditLogsHandler : IRequestHandler<GetAllAuditLogsQuery, BaseResponse<List<AuditLogResponse>>>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMapper _mapper;

        public GetAllAuditLogsHandler(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<AuditLogResponse>>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<AuditLogFilter>(request);
            var (auditLogs, totalRecords) = await _auditLogRepository.GetPagedAsync(filter, request.CurrentPage, request.PageSize);

            var auditLogResponses = _mapper.Map<List<AuditLogResponse>>(auditLogs);
            var response = BaseResponse<List<AuditLogResponse>>.Success(auditLogResponses);
            response.Pagination = new Pagination(totalRecords, request.CurrentPage, request.PageSize);

            return response;
        }
    }
}
