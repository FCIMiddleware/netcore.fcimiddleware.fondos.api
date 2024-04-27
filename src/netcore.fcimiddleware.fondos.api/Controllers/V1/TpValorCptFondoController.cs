using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.api.Errors;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TpValorCptFondoController : ControllerBase
    {
        private IMediator _mediator;

        public TpValorCptFondoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateTpValorCptFondo")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> CreateTpValorCptFondo([FromBody] CreateTpValorCptFondosCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateTpValorCptFondo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTpValorCptFondo([FromBody] UpdateTpValorCptFondosCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteTpValorCptFondo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTpValorCptFondo(int id)
        {
            var command = new DeleteTpValorCptFondosCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationTpValorCptFondo")]
        [ProducesResponseType(typeof(PaginationVm<TpValorCptFondoVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationVm<TpValorCptFondoVm>>> GetPaginationTpValorCptFondo([FromQuery] GetAllTpValorCptFondosQuery request)
        {
            var paginationTpValorCptFondos = await _mediator.Send(request);
            return Ok(paginationTpValorCptFondos);
        }

        [HttpGet("id", Name = "GetByIdTpValorCptFondo")]
        [ProducesResponseType(typeof(TpValorCptFondo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TpValorCptFondo>> GetByIdTpValorCptFondo([FromQuery] GetByIdTpValorCptFondosQuery request)
        {
            var tpValorCptFondo = await _mediator.Send(request);
            return Ok(tpValorCptFondo);
        }
    }
}
