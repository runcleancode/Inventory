using System;
using FluentValidation;
using Inventory.Application.DTOs;

namespace Inventory.Application.Validators;

public sealed class PatchProductDtoValidator : AbstractValidator<PatchProductDto>
{
    public PatchProductDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Product name cannot be empty when provided.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.")
            .When(dto => dto.Name is not null);
    }
}
