using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Update
{
    public class UpdateTpValorCptFondosCommandHandler : IRequestHandler<UpdateTpValorCptFondosCommand>
    {
        private readonly ILogger<UpdateTpValorCptFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateTpValorCptFondosCommandHandler(
            ILogger<UpdateTpValorCptFondosCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateTpValorCptFondosCommand request, CancellationToken cancellationToken)
        {
            var tpValorCptFondosToUpdate = await _mediator.Send(new GetByIdTpValorCptFondosQuery() { Id = request.Id });
            if (tpValorCptFondosToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(TpValorCptFondo)} id {request.Id}");
                throw new NotFoundException(nameof(TpValorCptFondo), request.Id);
            }

            if (tpValorCptFondosToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(TpValorCptFondo)} ya se encuentra anulado");
                throw new DeletedException(nameof(TpValorCptFondo), request.Id);
            }

            if (tpValorCptFondosToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(TpValorCptFondo)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(TpValorCptFondo), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, tpValorCptFondosToUpdate, typeof(UpdateTpValorCptFondosCommand), typeof(TpValorCptFondo));

            var updatedTpValorCptFondos = await _unitOfWork.RepositoryWrite<TpValorCptFondo>().UpdateAsync(tpValorCptFondosToUpdate);

            _logger.LogInformation($"Update - {nameof(TpValorCptFondo)} {updatedTpValorCptFondos.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateTpValorCptFondosCommand request)
        {

            var descripcionSpec = new TpValorCptFondosSpecificationDescripcion(request.Descripcion!.ToUpper(), request.Id);
            var descripcionExists = await _unitOfWork.RepositoryRead<TpValorCptFondo>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Update - {nameof(TpValorCptFondo)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(TpValorCptFondo), request.Descripcion);
            }

            var idCAFCISpec = new TpValorCptFondosSpecificationCAFCI(request.IdCAFCI!.ToUpper(), request.Id);
            var idCAFCIExists = await _unitOfWork.RepositoryRead<TpValorCptFondo>().GetByIdWithSpec(idCAFCISpec);

            if (idCAFCIExists != null)
            {
                _logger.LogError($"Update - {nameof(TpValorCptFondo)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(TpValorCptFondo), request.Descripcion);
            }

            var fondoExists = await _mediator.Send(new GetByIdFondosQuery() { Id = request.FondoId });
            if (fondoExists == null)
            {
                _logger.LogError($"Create - {nameof(Fondo)} {request.FondoId} no existe");
                throw new NotFoundException(nameof(Fondo), request.FondoId);
            }
        }
    }
}
