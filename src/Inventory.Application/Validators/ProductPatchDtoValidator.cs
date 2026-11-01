using FluentValidation;
using Inventory.Application.DTOs;

namespace Inventory.Application.Validators;

public sealed class ProductPatchDtoValidator : AbstractValidator<ProductPatchDto>
{
    public ProductPatchDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .Custom((nameField, context) =>
            {
                if (!nameField.IsSet)
                {
                    return;
                }

                if (nameField.Value is null)
                {
                    context.AddFailure(
                        nameof(ProductPatchDto.Name),
                        "Name cannot be set to null.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(nameField.Value))
                {
                    context.AddFailure(
                        nameof(ProductPatchDto.Name),
                        "Product name is required");
                    return;
                }

                if (nameField.Value.Length > 200)
                {
                    context.AddFailure(
                        nameof(ProductPatchDto.Name),
                        "Product name cannot exceed 200 characters.");
                }
            });
    }
}
