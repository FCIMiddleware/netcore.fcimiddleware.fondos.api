using MediatR;
using Microsoft.AspNetCore.Mvc;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetAll;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;
using System.Net;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SocDepositariaController : ControllerBase
    {
        private IMediator _mediator;

        public SocDepositariaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateSocDepositaria")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CreateSocDepositaria([FromBody] CreateSocDepositariasCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut(Name = "UpdateSocDepositaria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateSocDepositaria([FromBody] UpdateSocDepositariasCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteSocDepositaria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteSocDepositaria(int id)
        {
            var command = new DeleteSocDepositariasCommand
            {
                Id = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("pagination", Name = "PaginationSocDepositaria")]
        [ProducesResponseType(typeof(PaginationVm<SocDepositariaVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<SocDepositariaVm>>> GetPaginationSocGerentes([FromQuery] GetAllSocDepositariasQuery request)
        {
            var paginationSocDepositarias = await _mediator.Send(request);
            return Ok(paginationSocDepositarias);
        }

        [HttpGet("id", Name = "GetByIdSocDepositarias")]
        [ProducesResponseType(typeof(SocDepositaria), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SocDepositaria>> GetByIdSocDepositarias([FromQuery] GetByIdSocDepositariasQuery request)
        {
            var socDepositaria = await _mediator.Send(request);
            return Ok(socDepositaria);
        }
    }
}
