using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SocGerenteController : ControllerBase
    {
        private IMediator _mediator;

        public SocGerenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateSocGerente")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateSocGerente([FromBody] CreateSocGerentesCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateSocGerente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateSocGerente([FromBody] UpdateSocGerentesCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteSocGerente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteSocGerente(int id)
        {
            var command = new DeleteSocGerentesCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationSocGerentes")]
        [ProducesResponseType(typeof(PaginationVm<SocGerenteVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<SocGerenteVm>>> GetPaginationSocGerentes([FromQuery] GetAllSocGerentesQuery request)
        {
            var paginationSocGerentes = await _mediator.Send(request);
            return Ok(paginationSocGerentes);
        }

        [HttpGet("id", Name = "GetByIdSocGerentes")]
        [ProducesResponseType(typeof(SocGerente), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SocGerente>> GetByIdSocGerentes([FromQuery] GetByIdSocGerentesQuery request)
        {
            var socGerente = await _mediator.Send(request);
            return Ok(socGerente);
        }
    }
}
