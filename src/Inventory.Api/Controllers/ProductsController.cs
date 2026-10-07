using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            ProductDto? product = await productService.GetByIdAsync(id, cancellationToken);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
