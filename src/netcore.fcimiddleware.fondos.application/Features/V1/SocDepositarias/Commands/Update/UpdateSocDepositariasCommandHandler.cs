using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Update
{
    public class UpdateSocDepositariasCommandHandler : IRequestHandler<UpdateSocDepositariasCommand>
    {
        private readonly ILogger<UpdateSocDepositariasCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateSocDepositariasCommandHandler(
            ILogger<UpdateSocDepositariasCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateSocDepositariasCommand request, CancellationToken cancellationToken)
        {
            var socDepositariaToUpdate = await _mediator.Send(new GetByIdSocDepositariasQuery() { Id = request.Id });

            if (socDepositariaToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(SocDepositaria)} id {request.Id}");
                throw new NotFoundException(nameof(SocDepositaria), request.Id);
            }

            if (socDepositariaToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocDepositaria)} ya se encuentra anulado");
                throw new DeletedException(nameof(SocDepositaria), request.Id);
            }

            if (socDepositariaToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocDepositaria)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(SocDepositaria), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, socDepositariaToUpdate, typeof(UpdateSocDepositariasCommand), typeof(SocDepositaria));

            var updatedSocGerente = await _unitOfWork.RepositoryWrite<SocDepositaria>().UpdateAsync(socDepositariaToUpdate);

            _logger.LogInformation($"Update - {nameof(SocDepositaria)} {updatedSocGerente.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateSocDepositariasCommand request)
        {

            var descripcionSpec = new SocDepositariasSpecificationDescripcion(request.Descripcion);
            var descripcionExists = await _unitOfWork.RepositoryRead<SocDepositaria>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null && descripcionExists.Id != request.Id)
            {
                _logger.LogError($"Update - {nameof(SocDepositaria)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(SocDepositaria), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var idCAFCISpec = new SocDepositariasSpecificationCAFCI(request.IdCAFCI);
                var idCAFCIExists = await _unitOfWork.RepositoryRead<SocDepositaria>().GetByIdWithSpec(idCAFCISpec);

                if (idCAFCIExists != null && idCAFCIExists.Id != request.Id)
                {
                    _logger.LogError($"Update - {nameof(SocDepositaria)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(SocDepositaria), request.Descripcion);
                }
            }
        }
    }
}
