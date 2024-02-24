using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Delete
{
    public class DeletePaisesCommandHandler : IRequestHandler<DeletePaisesCommand>
    {
        private readonly ILogger<DeletePaisesCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeletePaisesCommandHandler(
            ILogger<DeletePaisesCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeletePaisesCommand request, CancellationToken cancellationToken)
        {
            var paisToDelete = await _mediator.Send(new GetByIdPaisesQuery() { Id = request.Id });
            if (paisToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(Pais)} no existe en el sistema");
                throw new NotFoundException(nameof(Pais), request.Id);
            }

            if (paisToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(Pais)} ya se encuentra anulado");
                throw new DeletedException(nameof(Pais), request.Id);
            }

            if (paisToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(Pais)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(Pais), request.Id);
            }

            paisToDelete.IsDeleted = true;
            var deletedFondo = await _unitOfWork.RepositoryWrite<Pais>().UpdateAsync(paisToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(Pais)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
