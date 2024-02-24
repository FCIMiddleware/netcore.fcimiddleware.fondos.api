using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FondoController : ControllerBase
    {
        private IMediator _mediator;

        public FondoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateFondo")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateFondo([FromBody] CreateFondosCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateFondo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateFondo([FromBody] UpdateFondosCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteFondo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteFondo(int id)
        {
            var command = new DeleteFondosCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination",Name = "PaginationFondo")]
        [ProducesResponseType(typeof(PaginationVm<FondoVm>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<FondoVm>>> GetPaginationFondos([FromQuery] GetAllFondosQuery request)
        {
            var paginationFondos = await _mediator.Send(request);
            return Ok(paginationFondos);
        }

        [HttpGet("id", Name = "GetByIdFondos")]
        [ProducesResponseType(typeof(Fondo), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Fondo>> GetByIdFondos([FromQuery] GetByIdFondosQuery request)
        {
            var fondo = await _mediator.Send(request);
            return Ok(fondo);
        }
    }
}
