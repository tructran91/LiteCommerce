using FluentValidation;
using Identity.Application.DTOs;

namespace Identity.Application.Validators
{
    public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleRequestValidator()
        {
            RuleFor(x => x.Name).ApplyRoleNameRules();
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.PermissionCodes).ApplyPermissionCodeRules();
        }
    }

    public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
    {
        public UpdateRoleRequestValidator()
        {
            RuleFor(x => x.Name).ApplyRoleNameRules();
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }

    public class UpdateRolePermissionsRequestValidator : AbstractValidator<UpdateRolePermissionsRequest>
    {
        public UpdateRolePermissionsRequestValidator()
        {
            // An empty list is valid: it strips every permission from the role.
            RuleFor(x => x.PermissionCodes).ApplyPermissionCodeRules();
        }
    }

    internal static class RoleValidatorRules
    {
        public static IRuleBuilderOptions<T, string> ApplyRoleNameRules<T>(this IRuleBuilder<T, string> rule)
            => rule.NotEmpty()
                .MaximumLength(50)
                .Matches("^[A-Za-z0-9 _-]+$")
                .WithMessage("Name may only contain letters, digits, spaces, underscores and hyphens.");

        public static IRuleBuilderOptions<T, List<string>> ApplyPermissionCodeRules<T>(this IRuleBuilder<T, List<string>> rule)
            => rule.NotNull()
                .Must(codes => codes.Count == codes.Distinct(StringComparer.Ordinal).Count())
                .WithMessage("Permission codes must not contain duplicates.")
                .Must(codes => codes.All(c => !string.IsNullOrWhiteSpace(c) && c.Length <= 100))
                .WithMessage("Each permission code must be non-empty and at most 100 characters.");
    }
}
