using FluentValidation;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Core.Entities;
using Identity.Infrastructure.Data;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using LiteCommerce.Shared.Validators;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Identity.Infrastructure.Services
{
    public class LogQueryService : ILogQueryService
    {
        private readonly IdentityDbContext _db;
        private readonly IValidator<GetAuditLogsQuery> _auditQueryValidator;
        private readonly IValidator<GetActivityLogsQuery> _activityQueryValidator;

        public LogQueryService(
            IdentityDbContext db,
            IValidator<GetAuditLogsQuery> auditQueryValidator,
            IValidator<GetActivityLogsQuery> activityQueryValidator)
        {
            _db = db;
            _auditQueryValidator = auditQueryValidator;
            _activityQueryValidator = activityQueryValidator;
        }

        public async Task<BaseResponse<List<AuditLogResponse>>> GetAuditLogsAsync(GetAuditLogsQuery query, CancellationToken cancellationToken = default)
        {
            var errors = ValidationErrors.From(_auditQueryValidator.Validate(query));
            if (errors is not null)
                return BaseResponse<List<AuditLogResponse>>.Failure("One or more validation errors occurred", errors);

            var logs = _db.AuditLogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.EntityName))
                logs = logs.Where(x => x.EntityName == query.EntityName);

            if (!string.IsNullOrWhiteSpace(query.EntityId))
                logs = logs.Where(x => x.EntityId == query.EntityId);

            if (query.Action.HasValue)
                logs = logs.Where(x => x.Action == query.Action);

            if (!string.IsNullOrWhiteSpace(query.CorrelationId))
                logs = logs.Where(x => x.CorrelationId == query.CorrelationId);

            if (query.FromDate.HasValue)
                logs = logs.Where(x => x.Timestamp >= query.FromDate);

            if (query.ToDate.HasValue)
                logs = logs.Where(x => x.Timestamp <= query.ToDate);

            var totalRecords = await logs.CountAsync(cancellationToken);
            var items = await logs
                .OrderByDescending(x => x.Timestamp)
                .Skip((query.CurrentPage - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            var response = BaseResponse<List<AuditLogResponse>>.Success(items.Select(ToResponse).ToList());
            response.Pagination = new Pagination(totalRecords, query.CurrentPage, query.PageSize);
            return response;
        }

        public async Task<BaseResponse<AuditLogResponse>> GetAuditLogAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!GuidValidator.IsValidGuid(id))
                return BaseResponse<AuditLogResponse>.Failure("One or more validation errors occurred", InvalidId());

            var log = await _db.AuditLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id), cancellationToken);
            return log is null
                ? BaseResponse<AuditLogResponse>.Failure("Audit log does not exist.", statusCode: HttpStatusCode.NotFound)
                : BaseResponse<AuditLogResponse>.Success(ToResponse(log));
        }

        public async Task<BaseResponse<List<ActivityLogResponse>>> GetActivityLogsAsync(GetActivityLogsQuery query, CancellationToken cancellationToken = default)
        {
            var errors = ValidationErrors.From(_activityQueryValidator.Validate(query));
            if (errors is not null)
                return BaseResponse<List<ActivityLogResponse>>.Failure("One or more validation errors occurred", errors);

            var logs = _db.ActivityLogs.AsNoTracking();

            if (query.Action.HasValue)
                logs = logs.Where(x => x.Action == query.Action);

            if (query.IsSuccess.HasValue)
                logs = logs.Where(x => x.IsSuccess == query.IsSuccess);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim();
                logs = logs.Where(x => (x.Email != null && x.Email.Contains(search)) || x.Description.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(query.CorrelationId))
                logs = logs.Where(x => x.CorrelationId == query.CorrelationId);

            if (query.FromDate.HasValue)
                logs = logs.Where(x => x.Timestamp >= query.FromDate);

            if (query.ToDate.HasValue)
                logs = logs.Where(x => x.Timestamp <= query.ToDate);

            var totalRecords = await logs.CountAsync(cancellationToken);
            var items = await logs
                .OrderByDescending(x => x.Timestamp)
                .Skip((query.CurrentPage - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            var response = BaseResponse<List<ActivityLogResponse>>.Success(items.Select(ToResponse).ToList());
            response.Pagination = new Pagination(totalRecords, query.CurrentPage, query.PageSize);
            return response;
        }

        public async Task<BaseResponse<ActivityLogResponse>> GetActivityLogAsync(string id, CancellationToken cancellationToken = default)
        {
            if (!GuidValidator.IsValidGuid(id))
                return BaseResponse<ActivityLogResponse>.Failure("One or more validation errors occurred", InvalidId());

            var log = await _db.ActivityLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(id), cancellationToken);
            return log is null
                ? BaseResponse<ActivityLogResponse>.Failure("Activity log does not exist.", statusCode: HttpStatusCode.NotFound)
                : BaseResponse<ActivityLogResponse>.Success(ToResponse(log));
        }

        // Log timestamps are stored as UTC but read back as Unspecified; mark them so the JSON carries the Z suffix.
        private static AuditLogResponse ToResponse(AuditLog log) => new()
        {
            Id = log.Id,
            EntityName = log.EntityName,
            EntityId = log.EntityId,
            Action = log.Action.ToString(),
            Changes = ParseChanges(log.Changes),
            UserName = log.UserName,
            CorrelationId = log.CorrelationId,
            Timestamp = DateTime.SpecifyKind(log.Timestamp, DateTimeKind.Utc)
        };

        private static ActivityLogResponse ToResponse(ActivityLog log) => new()
        {
            Id = log.Id,
            Action = log.Action.ToString(),
            IsSuccess = log.IsSuccess,
            FailureReason = log.FailureReason?.ToString(),
            UserId = log.UserId,
            Email = log.Email,
            Description = log.Description,
            IpAddress = log.IpAddress,
            UserAgent = log.UserAgent,
            UserName = log.UserName,
            CorrelationId = log.CorrelationId,
            Timestamp = DateTime.SpecifyKind(log.Timestamp, DateTimeKind.Utc)
        };

        // AuditLog.Changes is the JSON written by AuditLogInterceptor; values keep their JSON type there.
        private static List<AuditPropertyChangeResponse> ParseChanges(string changes)
        {
            if (string.IsNullOrWhiteSpace(changes)) return new();

            var items = JsonSerializer.Deserialize<List<ChangeJson>>(changes) ?? new();
            return items
                .Select(x => new AuditPropertyChangeResponse
                {
                    Property = x.Property,
                    OldValue = ToDisplayString(x.OldValue),
                    NewValue = ToDisplayString(x.NewValue)
                })
                .ToList();
        }

        private static string? ToDisplayString(JsonElement? value)
        {
            return value?.ValueKind switch
            {
                null or JsonValueKind.Null or JsonValueKind.Undefined => null,
                JsonValueKind.String => value.Value.GetString(),
                _ => value.Value.GetRawText()
            };
        }

        private static Dictionary<string, List<string>> InvalidId()
            => new() { ["id"] = [ValidationMessages.MustBeAValidGuid("Id")] };

        private sealed record ChangeJson(string Property, JsonElement? OldValue, JsonElement? NewValue);
    }
}
