using Catalog.Application.Categories.Commands;
using FluentValidation;

namespace Catalog.Application.Categories.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Payload.Name).NotNull().NotEmpty();
            RuleFor(x => x.Payload.IsPublished).NotNull().NotEmpty();
            RuleFor(x => x.Payload.IncludeInMenu).NotNull().NotEmpty();
            RuleFor(x => x.Payload.DisplayOrder).GreaterThanOrEqualTo(0);
        }
    }
}
