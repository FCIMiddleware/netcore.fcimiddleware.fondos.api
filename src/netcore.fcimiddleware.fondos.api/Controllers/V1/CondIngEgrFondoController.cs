using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.api.Errors;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CondIngEgrFondoController : ControllerBase
    {
        private IMediator _mediator;

        public CondIngEgrFondoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateCondIngEgrFondo")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CreateCondIngEgrFondo([FromBody] CreateCondIngEgrFondosCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateCondIngEgrFondo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCondIngEgrFondo([FromBody] UpdateCondIngEgrFondosCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteCondIngEgrFondo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCondIngEgrFondo(int id)
        {
            var command = new DeleteCondIngEgrFondosCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationCondIngEgrFondo")]
        [ProducesResponseType(typeof(PaginationVm<CondIngEgrFondoVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationVm<CondIngEgrFondoVm>>> GetPaginationCondIngEgrFondo([FromQuery] GetAllCondIngEgrFondosQuery request)
        {
            var paginationCondIngEgrFondos = await _mediator.Send(request);
            return Ok(paginationCondIngEgrFondos);
        }

        [HttpGet("id", Name = "GetByIdCondIngEgrFondo")]
        [ProducesResponseType(typeof(CondIngEgrFondo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CondIngEgrFondo>> GetByIdCondIngEgrFondo([FromQuery] GetByIdCondIngEgrFondosQuery request)
        {
            var condIngEgrFondo = await _mediator.Send(request);
            return Ok(condIngEgrFondo);
        }
    }
}
