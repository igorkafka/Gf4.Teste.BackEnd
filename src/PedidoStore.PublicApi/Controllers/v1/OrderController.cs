using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PedidoStore.Application.Orders.Commands;
using PedidoStore.Application.Orders.Response;
using PedidoStore.PublicApi.Extensions;
using PedidoStore.PublicApi.Models;
using PedidoStore.Query.Application.Order.Queries;
using PedidoStore.Query.QueriesModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace PedidoStore.PublicApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class OrderController(IMediator mediator) : ControllerBase
    {

    [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<CreatedOrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody][Required] CreateOrderCommand command) =>
        (await mediator.Send(command)).ToActionResult();

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update( [FromBody][Required] UpdateOrderCommand command) =>
            (await mediator.Send(command)).ToActionResult();


        [HttpDelete("{id:guid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([Required] Guid id) =>
            (await mediator.Send(new DeleteOrderCommand(id))).ToActionResult();


        [HttpGet("{id:guid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<CustomerQueryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([Required] Guid id) =>
            (await mediator.Send(new GetOrderByIdQuery(id))).ToActionResult();

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerQueryModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll() =>
            (await mediator.Send(new GetAllOrderQuery())).ToActionResult();
    }
}
