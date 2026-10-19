using FluentValidation;
using Identity.Application.DTOs;
using LiteCommerce.Shared.Constants;

namespace Identity.Application.Validators
{
    public class GetAuditLogsQueryValidator : AbstractValidator<GetAuditLogsQuery>
    {
        public GetAuditLogsQueryValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("PageSize", PaginationSetting.MinPageSize))
                .LessThanOrEqualTo(PaginationSetting.MaxPageSize).WithMessage(ValidationMessages.MustBeLessThanOrEqual("PageSize", PaginationSetting.MaxPageSize));

            RuleFor(x => x.CurrentPage)
                .GreaterThan(PaginationSetting.MinCurrentPage).WithMessage(ValidationMessages.MustBeGreaterThan("CurrentPage", PaginationSetting.MinCurrentPage))
                .LessThanOrEqualTo(PaginationSetting.MaxCurrentPage).WithMessage(ValidationMessages.MustBeLessThanOrEqual("CurrentPage", PaginationSetting.MaxCurrentPage));

            RuleFor(x => x.Action).IsInEnum();

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate).WithMessage(ValidationMessages.MustNotBeAfter("FromDate", "ToDate"))
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }

    public class GetActivityLogsQueryValidator : AbstractValidator<GetActivityLogsQuery>
    {
        public GetActivityLogsQueryValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("PageSize", PaginationSetting.MinPageSize))
                .LessThanOrEqualTo(PaginationSetting.MaxPageSize).WithMessage(ValidationMessages.MustBeLessThanOrEqual("PageSize", PaginationSetting.MaxPageSize));

            RuleFor(x => x.CurrentPage)
                .GreaterThan(PaginationSetting.MinCurrentPage).WithMessage(ValidationMessages.MustBeGreaterThan("CurrentPage", PaginationSetting.MinCurrentPage))
                .LessThanOrEqualTo(PaginationSetting.MaxCurrentPage).WithMessage(ValidationMessages.MustBeLessThanOrEqual("CurrentPage", PaginationSetting.MaxCurrentPage));

            RuleFor(x => x.Action).IsInEnum();

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate).WithMessage(ValidationMessages.MustNotBeAfter("FromDate", "ToDate"))
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
