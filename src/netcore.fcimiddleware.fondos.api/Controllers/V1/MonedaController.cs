using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MonedaController : ControllerBase
    {
        private IMediator _mediator;

        public MonedaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateMoneda")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateMoneda([FromBody] CreateMonedasCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateMoneda")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateMoneda([FromBody] UpdateMonedasCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteMoneda")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteMoneda(int id)
        {
            var command = new DeleteMonedasCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationMoneda")]
        [ProducesResponseType(typeof(PaginationVm<MonedaVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<MonedaVm>>> GetPaginationMonedas([FromQuery] GetAllMonedasQuery request)
        {
            var paginationMonedas = await _mediator.Send(request);
            return Ok(paginationMonedas);
        }

        [HttpGet("id", Name = "GetByIdMoneda")]
        [ProducesResponseType(typeof(Moneda), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Moneda>> GetByIdPais([FromQuery] GetByIdMonedasQuery request)
        {
            var moneda = await _mediator.Send(request);
            return Ok(moneda);
        }
    }
}
