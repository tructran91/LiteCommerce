using Catalog.Application.ActivityLogs.Queries;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.ActivityLogs.Validators
{
    public class GetAllActivityLogsValidator : AbstractValidator<GetAllActivityLogsQuery>
    {
        public GetAllActivityLogsValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("PageSize", PaginationSetting.MinPageSize))
                .LessThanOrEqualTo(PaginationSetting.MaxPageSize).WithMessage(ValidationMessages.MustBeLessThanOrEqual("PageSize", PaginationSetting.MaxPageSize));

            RuleFor(x => x.CurrentPage)
                .GreaterThan(PaginationSetting.MinPageSize).WithMessage(ValidationMessages.MustBeGreaterThan("CurrentPage", PaginationSetting.MinCurrentPage))
                .LessThanOrEqualTo(PaginationSetting.MaxCurrentPage).WithMessage(ValidationMessages.MustBeLessThanOrEqual("CurrentPage", PaginationSetting.MaxCurrentPage));

            RuleFor(x => x.Action)
                .IsInEnum();

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate).WithMessage(ValidationMessages.MustNotBeAfter("FromDate", "ToDate"))
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
