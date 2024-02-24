using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Delete
{
    public class DeleteFondosCommandHandler : IRequestHandler<DeleteFondosCommand>
    {
        private readonly ILogger<DeleteFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteFondosCommandHandler(
            ILogger<DeleteFondosCommandHandler> logger, 
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteFondosCommand request, CancellationToken cancellationToken)
        {
            var fondoToDelete = await _mediator.Send(new GetByIdFondosQuery() { Id = request.Id });
            if (fondoToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(Fondo)} no existe en el sistema");
                throw new NotFoundException(nameof(Fondo), request.Id);
            }

            if (fondoToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(Fondo)} ya se encuentra anulado");
                throw new DeletedException(nameof(Fondo), request.Id);
            }

            if (fondoToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(Fondo)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(Fondo), request.Id);
            }

            fondoToDelete.IsDeleted = true;
            var deletedFondo = await _unitOfWork.RepositoryWrite<Fondo>().UpdateAsync(fondoToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(Fondo)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
