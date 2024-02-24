using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Delete
{
    public class DeleteMonedasCommandHandler : IRequestHandler<DeleteMonedasCommand>
    {
        private readonly ILogger<DeleteMonedasCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteMonedasCommandHandler(
            ILogger<DeleteMonedasCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteMonedasCommand request, CancellationToken cancellationToken)
        {
            var monedaToDelete = await _mediator.Send(new GetByIdMonedasQuery() { Id = request.Id });
            if (monedaToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(Moneda)} no existe en el sistema");
                throw new NotFoundException(nameof(Moneda), request.Id);
            }

            if (monedaToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(Moneda)} ya se encuentra anulado");
                throw new DeletedException(nameof(Moneda), request.Id);
            }

            if (monedaToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(Moneda)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(Moneda), request.Id);
            }

            monedaToDelete.IsDeleted = true;
            var deletedMoneda = await _unitOfWork.RepositoryWrite<Moneda>().UpdateAsync(monedaToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(Moneda)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
