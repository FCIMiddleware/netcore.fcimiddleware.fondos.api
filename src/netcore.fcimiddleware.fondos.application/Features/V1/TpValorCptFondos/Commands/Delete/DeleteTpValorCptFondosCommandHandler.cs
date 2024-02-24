using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Delete
{
    public class DeleteTpValorCptFondosCommandHandler : IRequestHandler<DeleteTpValorCptFondosCommand>
    {
        private readonly ILogger<DeleteTpValorCptFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteTpValorCptFondosCommandHandler(
            ILogger<DeleteTpValorCptFondosCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteTpValorCptFondosCommand request, CancellationToken cancellationToken)
        {
            var tpFondoValorCpToDelete = await _mediator.Send(new GetByIdTpValorCptFondosQuery() { Id = request.Id });
            if (tpFondoValorCpToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(TpValorCptFondo)} no existe en el sistema");
                throw new NotFoundException(nameof(TpValorCptFondo), request.Id);
            }

            if (tpFondoValorCpToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(TpValorCptFondo)} ya se encuentra anulado");
                throw new DeletedException(nameof(TpValorCptFondo), request.Id);
            }

            if (tpFondoValorCpToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(TpValorCptFondo)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(TpValorCptFondo), request.Id);
            }

            tpFondoValorCpToDelete.IsDeleted = true;
            var deletedFondo = await _unitOfWork.RepositoryWrite<TpValorCptFondo>().UpdateAsync(tpFondoValorCpToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(TpValorCptFondo)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
