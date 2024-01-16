using Catalog.Application.Brands.Commands;
using FluentValidation;

namespace Catalog.Application.Brands.Validators
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandValidator()
        {
            RuleFor(x => x.Payload.Name).NotNull().NotEmpty();
        }
    }
}
