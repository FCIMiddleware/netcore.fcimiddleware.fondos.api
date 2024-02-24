using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.AgColocadores;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Update
{
    public class UpdateAgColocadoresCommandHandler : IRequestHandler<UpdateAgColocadoresCommand>
    {
        private readonly ILogger<UpdateAgColocadoresCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateAgColocadoresCommandHandler(
            ILogger<UpdateAgColocadoresCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateAgColocadoresCommand request, CancellationToken cancellationToken)
        {
            var agColocadoresToUpdate = await _mediator.Send(new GetByIdAgColocadoresQuery() { Id = request.Id });

            if (agColocadoresToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(AgColocador)} id {request.Id}");
                throw new NotFoundException(nameof(AgColocador), request.Id);
            }

            if (agColocadoresToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(AgColocador)} ya se encuentra anulado");
                throw new DeletedException(nameof(AgColocador), request.Id);
            }

            if (agColocadoresToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(AgColocador)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(AgColocador), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, agColocadoresToUpdate, typeof(UpdateAgColocadoresCommand), typeof(AgColocador));

            var updatedSocGerente = await _unitOfWork.RepositoryWrite<AgColocador>().UpdateAsync(agColocadoresToUpdate);

            _logger.LogInformation($"Update - {nameof(AgColocador)} {updatedSocGerente.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateAgColocadoresCommand request)
        {

            var descripcionSpec = new AgColocadoresSpecificationDescripcion(request.Descripcion!.ToUpper(), request.Id);
            var descripcionExists = await _unitOfWork.RepositoryRead<AgColocador>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Update - {nameof(AgColocador)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(AgColocador), request.Descripcion);
            }

            var idCAFCISpec = new AgColocadoresSpecificationCAFCI(request.IdCAFCI!.ToUpper(), request.Id);
            var idCAFCIExists = await _unitOfWork.RepositoryRead<AgColocador>().GetByIdWithSpec(idCAFCISpec);

            if (idCAFCIExists != null)
            {
                _logger.LogError($"Update - {nameof(AgColocador)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(AgColocador), request.Descripcion);
            }
        }
    }
}
