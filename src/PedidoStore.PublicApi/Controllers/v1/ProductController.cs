using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidoStore.PublicApi.Extensions;
using PedidoStore.PublicApi.Models;
using PedidoStore.Query.Application.Order.Queries;
using PedidoStore.Query.QueriesModel;
using System.Net.Mime;

namespace PedidoStore.PublicApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductQueryModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll() =>
    (await mediator.Send(new GetAllProductQuery())).ToActionResult();
}

