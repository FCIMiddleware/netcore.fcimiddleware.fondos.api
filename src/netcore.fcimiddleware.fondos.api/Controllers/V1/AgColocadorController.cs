using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.api.Errors;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgColocadorController : ControllerBase
    {
        private IMediator _mediator;

        public AgColocadorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateAgColocador")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateAgColocador([FromBody] CreateAgColocadoresCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateAgColocador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAgColocador([FromBody] UpdateAgColocadoresCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteAgColocador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAgColocador(int id)
        {
            var command = new DeleteAgColocadoresCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationAgColocadores")]
        [ProducesResponseType(typeof(PaginationVm<AgColocadorVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationVm<AgColocadorVm>>> GetPaginationSocGerentes([FromQuery] GetAllAgColocadoresQuery request)
        {
            var paginationAgColocadores = await _mediator.Send(request);
            return Ok(paginationAgColocadores);
        }

        [HttpGet("id", Name = "GetByIdAgColocadores")]
        [ProducesResponseType(typeof(AgColocador), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AgColocador>> GetByIdAgColocadores([FromQuery] GetByIdAgColocadoresQuery request)
        {
            var agColocador = await _mediator.Send(request);
            return Ok(agColocador);
        }
    }
}
