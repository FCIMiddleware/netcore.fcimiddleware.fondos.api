using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.api.Errors;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetByDescripcion;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaisController : ControllerBase
    {
        private IMediator _mediator;

        public PaisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreatePais")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreatePais([FromBody] CreatePaisesCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdatePais")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePais([FromBody] UpdatePaisesCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeletePais")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePais(int id)
        {
            var command = new DeletePaisesCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationPais")]
        [ProducesResponseType(typeof(PaginationVm<PaisVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationVm<PaisVm>>> GetPaginationPaises([FromQuery] GetAllPaisesQuery request)
        {
            var paginationPaises = await _mediator.Send(request);
            return Ok(paginationPaises);
        }

        [HttpGet("id", Name = "GetByIdPais")]
        [ProducesResponseType(typeof(Pais), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pais>> GetByIdPais([FromQuery] GetByIdPaisesQuery request)
        {
            var paises = await _mediator.Send(request);
            return Ok(paises);
        }

        [HttpGet("list", Name = "GetByDescripcionPais")]
        [ProducesResponseType(typeof(PaginationVm<PaisListVm>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(CodeErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginationVm<PaisListVm>>> GetByDescripcionPais([FromQuery] GetByDescripcionPaisesQuery request)
        {
            var paises = await _mediator.Send(request);
            return Ok(paises);
        }
    }
}
