using Catalog.Application.Categories.Commands;
using FluentValidation;

namespace Catalog.Application.Categories.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Payload.Id).NotNull().NotEmpty();
            RuleFor(x => x.Payload.Name).NotNull().NotEmpty();
            RuleFor(x => x.Payload.IsPublished).NotNull().NotEmpty();
            RuleFor(x => x.Payload.IncludeInMenu).NotNull().NotEmpty();
            RuleFor(x => x.Payload.DisplayOrder).GreaterThanOrEqualTo(0);
        }
    }
}
