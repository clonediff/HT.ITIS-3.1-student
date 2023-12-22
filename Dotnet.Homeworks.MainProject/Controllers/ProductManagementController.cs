using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct(string name, CancellationToken cancellationToken)
    {
        var command = new InsertProductCommand(name);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        var command = new DeleteProductByGuidCommand(guid);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(Guid guid, string name, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(guid, name);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
