using FluentValidation;
using Inventory.Application.Queries;

namespace Inventory.Application.Validators;

public class ProductQueryParamsValidator : AbstractValidator<ProductQueryParams>
{
    public const int MaxPageSize = 100;
    public const int MaxSearchLength = 100;

    public ProductQueryParamsValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .When(x => x.Id.HasValue)
            .WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .WithMessage("Name filter cannot exceed 200 characters.");

        RuleFor(x => x.Q)
            .MaximumLength(MaxSearchLength)
            .WithMessage($"Search term cannot exceed {MaxSearchLength} characters.");

        RuleFor(x => x.SortBy)
            .Must(BeValidSortField)
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ProductSortFields.All)}");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage($"PageSize must be between 1 and {MaxPageSize}.");

        RuleFor(x => x.SortDir)
            .IsInEnum()
            .WithMessage("SortDir must be either Asc (1) or Desc (2).");
    }

    private static bool BeValidSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return true;
        }

        return ProductSortFields.All.Contains(sortBy, StringComparer.OrdinalIgnoreCase);
    }
}
