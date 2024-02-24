using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Delete
{
    public class DeleteAgColocadoresCommandHandler : IRequestHandler<DeleteAgColocadoresCommand>
    {
        private readonly ILogger<DeleteAgColocadoresCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteAgColocadoresCommandHandler(
            ILogger<DeleteAgColocadoresCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteAgColocadoresCommand request, CancellationToken cancellationToken)
        {
            var agColocadoresToDelete = await _mediator.Send(new GetByIdAgColocadoresQuery() { Id = request.Id });
            if (agColocadoresToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(AgColocador)} no existe en el sistema");
                throw new NotFoundException(nameof(AgColocador), request.Id);
            }

            if (agColocadoresToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(AgColocador)} ya se encuentra anulado");
                throw new DeletedException(nameof(AgColocador), request.Id);
            }

            if (agColocadoresToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(AgColocador)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(AgColocador), request.Id);
            }

            agColocadoresToDelete.IsDeleted = true;
            var deletedFondo = await _unitOfWork.RepositoryWrite<AgColocador>().UpdateAsync(agColocadoresToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(AgColocador)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
