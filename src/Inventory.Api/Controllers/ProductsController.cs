using System.Text.Json;
using Inventory.Application.Common.Models;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetProductById")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        ProductDto product = await productService.GetByIdAsync(id, cancellationToken);

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType<ProductDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken)
    {
        ProductDto created = await productService.CreateAsync(dto, cancellationToken);

        return CreatedAtRoute(
            routeName: "GetProductById",
            routeValues: new { id = created.Id },
            value: created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken)
    {
        ProductDto updated = await productService.UpdateAsync(id, dto, cancellationToken);

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await productService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet(Name = "GetProducts")]
    [ProducesResponseType<PagedResult<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetAllAsync(
       [FromQuery] ProductQueryParams queryParams,
        CancellationToken cancellationToken = default)
    {
        PagedResult<ProductDto> result = await productService.GetPagedAsync(queryParams, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:int}")]
    [Consumes("application/merge-patch+json")]
    public async Task<ActionResult<ProductDto>> PatchAsync(
        int id,
        [FromBody] JsonElement patchDocument,
        CancellationToken cancellationToken)
    {
        ProductDto updated = await productService.PatchAsync(id, patchDocument, cancellationToken);

        return Ok(updated);
    }
}

