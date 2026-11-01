using Inventory.Application.Common.Models;

namespace Inventory.Application.DTOs;

public sealed class ProductPatchDto
{
    public PatchField<string?> Name { get; set; } = PatchField<string?>.Unset;
}
