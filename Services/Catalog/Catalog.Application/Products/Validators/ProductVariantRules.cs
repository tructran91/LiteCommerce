using Catalog.Application.Extensions;
using Catalog.Application.ViewModels;
using Catalog.Core.Constants;
using FluentValidation;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Validators;

namespace Catalog.Application.Products.Validators
{
    // Shared by CreateProductValidator and UpdateProductValidator. The unique indexes
    // catch duplicates, but not a missing axis, an unknown value key, or a cartesian
    // product large enough to be a problem.
    public static class ProductVariantRules
    {
        public static void AddProductOptionAndVariantRules<T>(
            this AbstractValidator<T> validator,
            Func<T, ProductViewModel> productSelector)
        {
            validator.RuleFor(x => productSelector(x).Options)
                .Must(options => (options?.Count ?? 0) <= VariantLimits.MaxOptionsPerProduct)
                .WithName("Options")
                .WithMessage(ValidationMessages.MustBeLessThanOrEqual("Options", VariantLimits.MaxOptionsPerProduct));

            validator.RuleFor(x => productSelector(x).Options)
                .Must(HaveUniqueOptionIds)
                .WithName("Options")
                .WithMessage("The same option cannot be added twice.")
                .When(x => productSelector(x).Options?.Count > 0);

            validator.RuleForEach(x => productSelector(x).Options)
                .ChildRules(option =>
                {
                    option.RuleFor(o => o.Id)
                        .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                        .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Id"))
                        .Must(GuidValidator.IsValidGuid!).WithMessage(ValidationMessages.MustBeAValidGuid("Id"));

                    option.RuleFor(o => o.Values)
                        .NotEmpty().WithMessage("An option must declare at least one value.")
                        .Must(values => (values?.Count ?? 0) <= VariantLimits.MaxValuesPerOption)
                        .WithMessage(ValidationMessages.MustBeLessThanOrEqual("Values", VariantLimits.MaxValuesPerOption))
                        .Must(HaveUniqueValueKeys)
                        .WithMessage("Option values must be unique.");

                    option.RuleForEach(o => o.Values)
                        .ChildRules(value =>
                        {
                            value.RuleFor(v => v.Key)
                                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("Key"))
                                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("Key"))
                                .MaximumLength(FieldLength.ProductOptionValueItemKey)
                                .WithMessage(ValidationMessages.MaximumLength("Key", FieldLength.ProductOptionValueItemKey));

                            value.RuleFor(v => v.Display)
                                .MaximumLength(FieldLength.ProductOptionValueItemDisplay)
                                .WithMessage(ValidationMessages.MaximumLength("Display", FieldLength.ProductOptionValueItemDisplay));
                        });
                })
                .When(x => productSelector(x).Options?.Count > 0);

            validator.RuleFor(x => productSelector(x).Variants)
                .Must(variants => (variants?.Count ?? 0) <= VariantLimits.MaxVariantsPerProduct)
                .WithName("Variants")
                .WithMessage(ValidationMessages.MustBeLessThanOrEqual("Variants", VariantLimits.MaxVariantsPerProduct));

            // Without this, AddOrDeleteVariants would silently drop every row.
            validator.RuleFor(x => productSelector(x).Variants)
                .Must(variants => (variants?.Count ?? 0) == 0)
                .WithName("Variants")
                .WithMessage("Variants require at least one product option.")
                .When(x => (productSelector(x).Options?.Count ?? 0) == 0);

            validator.RuleFor(x => productSelector(x))
                .Must(HaveUniqueCombinations)
                .WithName("Variants")
                .WithMessage("Two variants cannot have the same option combination.")
                .When(x => productSelector(x).Variants?.Count > 0);

            validator.RuleFor(x => productSelector(x))
                .Must(HaveUniqueSkus)
                .WithName("Variants")
                .WithMessage("Variant SKUs must be unique within a product.")
                .When(x => productSelector(x).Variants?.Count > 0);

            validator.RuleForEach(x => productSelector(x).Variants)
                .ChildRules(variant =>
                {
                    variant.RuleFor(v => v.Price)
                        .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("Price", 0));

                    variant.RuleFor(v => v.OldPrice)
                        .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("OldPrice", 0))
                        .When(v => v.OldPrice.HasValue);

                    variant.RuleFor(v => v.SpecialPrice)
                        .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("SpecialPrice", 0))
                        .When(v => v.SpecialPrice.HasValue);

                    variant.RuleFor(v => v.SpecialPriceStart)
                        .LessThanOrEqualTo(v => v.SpecialPriceEnd)
                        .WithMessage(ValidationMessages.MustNotBeAfter("SpecialPriceStart", "SpecialPriceEnd"))
                        .When(v => v.SpecialPriceStart.HasValue && v.SpecialPriceEnd.HasValue);

                    variant.RuleFor(v => v.StockQuantity)
                        .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MustBeGreaterThanOrEqual("StockQuantity", 0));

                    variant.RuleFor(v => v.Sku)
                        .MaximumLength(FieldLength.ProductSku)
                        .WithMessage(ValidationMessages.MaximumLength("Sku", FieldLength.ProductSku));

                    variant.RuleFor(v => v.Gtin)
                        .MaximumLength(FieldLength.ProductGtin)
                        .WithMessage(ValidationMessages.MaximumLength("Gtin", FieldLength.ProductGtin));

                    variant.RuleFor(v => v.Name)
                        .MaximumLength(FieldLength.ProductVariantName)
                        .WithMessage(ValidationMessages.MaximumLength("Name", FieldLength.ProductVariantName));

                    variant.RuleFor(v => v.Id)
                        .Must(GuidValidator.IsValidGuid!).WithMessage(ValidationMessages.MustBeAValidGuid("Id"))
                        .When(v => !string.IsNullOrWhiteSpace(v.Id));

                    variant.RuleForEach(v => v.OptionCombinations)
                        .ChildRules(combination =>
                        {
                            combination.RuleFor(c => c.OptionId)
                                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("OptionId"))
                                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("OptionId"))
                                .Must(GuidValidator.IsValidGuid!).WithMessage(ValidationMessages.MustBeAValidGuid("OptionId"));

                            combination.RuleFor(c => c.ValueKey)
                                .NotNull().WithMessage(ValidationMessages.NotNullOrEmpty("ValueKey"))
                                .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty("ValueKey"));
                        });
                })
                .When(x => productSelector(x).Variants?.Count > 0);

            // The cross-check AddOrDeleteVariants would otherwise fail silently on.
            validator.RuleForEach(x => productSelector(x).Variants)
                .Must((command, variant) => CoversAllAxes(productSelector(command), variant))
                .WithMessage("A variant must have exactly one value for each product option, and the value must be one of the declared values.")
                .When(x => productSelector(x).Variants?.Count > 0 && productSelector(x).Options?.Count > 0);
        }

        private static bool HaveUniqueOptionIds(IList<ProductOptionViewModel>? options)
        {
            var ids = (options ?? []).Select(o => o.Id ?? string.Empty).ToList();
            return ids.Distinct(StringComparer.OrdinalIgnoreCase).Count() == ids.Count;
        }

        private static bool HaveUniqueValueKeys(IList<ProductOptionValueViewModel>? values)
        {
            var keys = (values ?? []).Select(v => NormalizeKey(v.Key)).ToList();
            return keys.Distinct(StringComparer.OrdinalIgnoreCase).Count() == keys.Count;
        }

        private static bool HaveUniqueCombinations(ProductViewModel product)
        {
            var signatures = (product.Variants ?? [])
                .Select(BuildCombinationSignature)
                .ToList();

            return signatures.Distinct(StringComparer.OrdinalIgnoreCase).Count() == signatures.Count;
        }

        private static bool HaveUniqueSkus(ProductViewModel product)
        {
            var skus = (product.Variants ?? [])
                .Select(v => v.Sku)
                .Where(sku => !string.IsNullOrWhiteSpace(sku))
                .Select(sku => sku!.Trim())
                .ToList();

            return skus.Distinct(StringComparer.OrdinalIgnoreCase).Count() == skus.Count;
        }

        // Ordered by option id so the signature does not depend on the order the client
        // serialised the combination in.
        private static string BuildCombinationSignature(ProductVariantViewModel variant)
            => string.Join("|", (variant.OptionCombinations ?? [])
                .OrderBy(c => c.OptionId, StringComparer.OrdinalIgnoreCase)
                .Select(c => $"{c.OptionId}:{NormalizeKey(c.ValueKey)}"));

        private static bool CoversAllAxes(ProductViewModel product, ProductVariantViewModel variant)
        {
            var axes = product.Options ?? [];
            var combinations = variant.OptionCombinations ?? [];

            if (combinations.Count != axes.Count)
            {
                return false;
            }

            foreach (var axis in axes)
            {
                var matches = combinations
                    .Where(c => string.Equals(c.OptionId, axis.Id, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matches.Count != 1)
                {
                    return false;
                }

                var allowedKeys = (axis.Values ?? [])
                    .Select(v => NormalizeKey(v.Key))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (!allowedKeys.Contains(NormalizeKey(matches[0].ValueKey)))
                {
                    return false;
                }
            }

            return true;
        }

        // Mirrors ProductService.NormalizeKey; the two must agree on what counts as
        // the same key or validation and persistence disagree.
        private static string NormalizeKey(string? key)
        {
            var slug = (key ?? string.Empty).Slugify();
            return string.IsNullOrWhiteSpace(slug) ? (key ?? string.Empty).Trim().ToLowerInvariant() : slug;
        }
    }
}
