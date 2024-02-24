using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetById;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Delete
{
    public class DeleteSocGerentesCommandHandler : IRequestHandler<DeleteSocGerentesCommand>
    {
        private readonly ILogger<DeleteSocGerentesCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteSocGerentesCommandHandler(
            ILogger<DeleteSocGerentesCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteSocGerentesCommand request, CancellationToken cancellationToken)
        {
            var socGerenteToDelete = await _mediator.Send(new GetByIdSocGerentesQuery() { Id = request.Id });
            if (socGerenteToDelete == null)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocGerente)} no existe en el sistema");
                throw new NotFoundException(nameof(SocGerente), request.Id);
            }

            if (socGerenteToDelete.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocGerente)} ya se encuentra anulado");
                throw new DeletedException(nameof(SocGerente), request.Id);
            }

            if (socGerenteToDelete.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(SocGerente)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(SocGerente), request.Id);
            }

            socGerenteToDelete.IsDeleted = true;
            var deletedFondo = await _unitOfWork.RepositoryWrite<SocGerente>().UpdateAsync(socGerenteToDelete);

            _logger.LogInformation($"El {request.Id} {nameof(SocGerente)} fue eliminado con exito");

            return Unit.Value;
        }
    }
}
