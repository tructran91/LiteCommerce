using FluentValidation;
using Identity.Application.DTOs;
using LiteCommerce.Shared.Constants;

namespace Identity.Application.Validators
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.PageSize).GreaterThan(PaginationSetting.MinPageSize).LessThanOrEqualTo(PaginationSetting.MaxPageSize);
            RuleFor(x => x.CurrentPage).GreaterThan(PaginationSetting.MinCurrentPage).LessThanOrEqualTo(PaginationSetting.MaxCurrentPage);
            RuleFor(x => x.Search).MaximumLength(256);
            RuleFor(x => x.Role).MaximumLength(50);
        }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100);
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
    {
        public UpdateUserRoleRequestValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }
}
