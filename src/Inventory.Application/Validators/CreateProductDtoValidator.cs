using FluentValidation;
using Inventory.Application.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Validators;

public sealed class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(Product.NameMaxLength)
            .WithMessage($"Product name cannot exceed {Product.NameMaxLength} characters.");
    }
}
