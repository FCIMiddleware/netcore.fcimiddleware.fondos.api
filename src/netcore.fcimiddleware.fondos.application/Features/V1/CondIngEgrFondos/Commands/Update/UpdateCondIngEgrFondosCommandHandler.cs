using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Update
{
    public class UpdateCondIngEgrFondosCommandHandler : IRequestHandler<UpdateCondIngEgrFondosCommand>
    {
        private readonly ILogger<UpdateCondIngEgrFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateCondIngEgrFondosCommandHandler(
            ILogger<UpdateCondIngEgrFondosCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateCondIngEgrFondosCommand request, CancellationToken cancellationToken)
        {
            var condIngEgrFondoToUpdate = await _mediator.Send(new GetByIdCondIngEgrFondosQuery() { Id = request.Id });
            if (condIngEgrFondoToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(CondIngEgrFondo)} id {request.Id}");
                throw new NotFoundException(nameof(CondIngEgrFondo), request.Id);
            }

            if (condIngEgrFondoToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(CondIngEgrFondo)} ya se encuentra anulado");
                throw new DeletedException(nameof(CondIngEgrFondo), request.Id);
            }

            if (condIngEgrFondoToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(CondIngEgrFondo)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(CondIngEgrFondo), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, condIngEgrFondoToUpdate, typeof(UpdateCondIngEgrFondosCommand), typeof(CondIngEgrFondo));

            var updatedCondIngEgrFondo = await _unitOfWork.RepositoryWrite<CondIngEgrFondo>().UpdateAsync(condIngEgrFondoToUpdate);

            _logger.LogInformation($"Update - {nameof(CondIngEgrFondo)} {updatedCondIngEgrFondo.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateCondIngEgrFondosCommand request)
        {

            var descripcionSpec = new CondIngEgrFondosSpecificationDescripcion(request.Descripcion!.ToUpper(), request.Id);
            var descripcionExists = await _unitOfWork.RepositoryRead<CondIngEgrFondo>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Update - {nameof(CondIngEgrFondo)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(CondIngEgrFondo), request.Descripcion);
            }

            var fondoExists = await _mediator.Send(new GetByIdFondosQuery() { Id = request.FondoId });
            if (fondoExists == null)
            {
                _logger.LogError($"Update - {nameof(Fondo)} {request.FondoId} no existe");
                throw new NotFoundException(nameof(Fondo), request.FondoId);
            }

            var tpValorCptFondoExists = await _mediator.Send(new GetByIdTpValorCptFondosQuery() { Id = request.TpValorCptFondoId });
            if (tpValorCptFondoExists == null)
            {
                _logger.LogError($"Update - {nameof(TpValorCptFondo)} {request.TpValorCptFondoId} no existe");
                throw new NotFoundException(nameof(TpValorCptFondo), request.TpValorCptFondoId);
            }
        }
    }
}
