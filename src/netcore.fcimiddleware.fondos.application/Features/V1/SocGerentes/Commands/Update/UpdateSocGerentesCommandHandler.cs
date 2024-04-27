using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Update
{
    public class UpdateSocGerentesCommandHandler : IRequestHandler<UpdateSocGerentesCommand>
    {
        private readonly ILogger<UpdateSocGerentesCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateSocGerentesCommandHandler(
            ILogger<UpdateSocGerentesCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateSocGerentesCommand request, CancellationToken cancellationToken)
        {
            var socGerenteToUpdate = await _mediator.Send(new GetByIdSocGerentesQuery() { Id = request.Id });

            if (socGerenteToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(SocGerente)} id {request.Id}");
                throw new NotFoundException(nameof(SocGerente), request.Id);
            }

            if (socGerenteToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocGerente)} ya se encuentra anulado");
                throw new DeletedException(nameof(SocGerente), request.Id);
            }

            if (socGerenteToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocGerente)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(SocGerente), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, socGerenteToUpdate, typeof(UpdateSocGerentesCommand), typeof(SocGerente));

            var updatedSocGerente = await _unitOfWork.RepositoryWrite<SocGerente>().UpdateAsync(socGerenteToUpdate);

            _logger.LogInformation($"Update - {nameof(SocGerente)} {updatedSocGerente.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateSocGerentesCommand request)
        {

            var descripcionSpec = new SocGerentesSpecificationDescripcion(request.Descripcion);
            var descripcionExists = await _unitOfWork.RepositoryRead<SocGerente>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null && descripcionExists.Id != request.Id)
            {
                _logger.LogError($"Update - {nameof(SocGerente)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(SocGerente), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var idCAFCISpec = new SocGerentesSpecificationCAFCI(request.IdCAFCI);
                var idCAFCIExists = await _unitOfWork.RepositoryRead<SocGerente>().GetByIdWithSpec(idCAFCISpec);

                if (idCAFCIExists != null && idCAFCIExists.Id != request.Id)
                {
                    _logger.LogError($"Update - {nameof(SocGerente)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(SocGerente), request.Descripcion);
                }
            }
        }
    }
}
