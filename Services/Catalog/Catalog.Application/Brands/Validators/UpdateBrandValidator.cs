using Catalog.Application.Brands.Commands;
using FluentValidation;

namespace Catalog.Application.Brands.Validators
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandValidator()
        {
            RuleFor(x => x.Payload.Id).NotNull().NotEmpty();
            RuleFor(x => x.Payload.Name).NotNull().NotEmpty();
        }
    }
}
