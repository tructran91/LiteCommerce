using Catalog.Application.Database.Commands;
using Catalog.Application.Services;
using FluentValidation;
using LiteCommerce.Shared.Constants;

namespace Catalog.Application.Database.Validators
{
    public class SeedDatabaseValidator : AbstractValidator<SeedDatabaseCommand>
    {
        public SeedDatabaseValidator(IDatabaseSeeder seeder)
        {
            RuleFor(x => x.Payload.Profile)
                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Profile"))
                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Profile"))
                .Must(profile => seeder.GetAvailableProfiles()
                    .Any(name => name.Equals(profile, StringComparison.OrdinalIgnoreCase)))
                .WithMessage("Unknown seed profile.");
        }
    }
}
