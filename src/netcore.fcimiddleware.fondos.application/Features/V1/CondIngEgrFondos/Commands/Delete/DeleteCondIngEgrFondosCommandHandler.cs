using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Delete
{
    public class DeleteCondIngEgrFondosCommandHandler : IRequestHandler<DeleteCondIngEgrFondosCommand>
    {
        private readonly ILogger<DeleteCondIngEgrFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteCondIngEgrFondosCommandHandler(
            ILogger<DeleteCondIngEgrFondosCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteCondIngEgrFondosCommand request, CancellationToken cancellationToken)
        {
            var condIngEgrFondoToDelete = await _mediator.Send(new GetByIdCondIngEgrFondosQuery() { Id = request.Id });
            if (condIngEgrFondoToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(CondIngEgrFondo)} no existe en el sistema");
                throw new NotFoundException(nameof(CondIngEgrFondo), request.Id);
            }

            if (condIngEgrFondoToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(CondIngEgrFondo)} ya se encuentra anulado");
                throw new DeletedException(nameof(CondIngEgrFondo), request.Id);
            }

            if (condIngEgrFondoToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(CondIngEgrFondo)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(CondIngEgrFondo), request.Id);
            }

            condIngEgrFondoToDelete.IsDeleted = true;
            var deletedCondIngEgrFondo = await _unitOfWork.RepositoryWrite<CondIngEgrFondo>().UpdateAsync(condIngEgrFondoToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(CondIngEgrFondo)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
