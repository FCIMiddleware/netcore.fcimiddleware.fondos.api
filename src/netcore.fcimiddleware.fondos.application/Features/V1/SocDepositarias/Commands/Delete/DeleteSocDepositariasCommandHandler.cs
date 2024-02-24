using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Delete
{
    public class DeleteSocDepositariasCommandHandler : IRequestHandler<DeleteSocDepositariasCommand>
    {
        private readonly ILogger<DeleteSocDepositariasCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteSocDepositariasCommandHandler(
            ILogger<DeleteSocDepositariasCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteSocDepositariasCommand request, CancellationToken cancellationToken)
        {
            var socDepositariaToDelete = await _mediator.Send(new GetByIdSocDepositariasQuery() { Id = request.Id });
            if (socDepositariaToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocDepositaria)} no existe en el sistema");
                throw new NotFoundException(nameof(SocGerente), request.Id);
            }

            if (socDepositariaToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocDepositaria)} ya se encuentra anulado");
                throw new DeletedException(nameof(SocGerente), request.Id);
            }

            if (socDepositariaToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocDepositaria)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(SocGerente), request.Id);
            }

            socDepositariaToDelete.IsDeleted = true;
            var deletedFondo = await _unitOfWork.RepositoryWrite<SocDepositaria>().UpdateAsync(socDepositariaToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(SocDepositaria)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
