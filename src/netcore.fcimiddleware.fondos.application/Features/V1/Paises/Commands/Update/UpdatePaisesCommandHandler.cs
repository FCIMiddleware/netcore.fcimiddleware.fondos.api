using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.Paises;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Update
{
    public class UpdatePaisesCommandHandler : IRequestHandler<UpdatePaisesCommand>
    {
        private readonly ILogger<UpdatePaisesCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdatePaisesCommandHandler(ILogger<UpdatePaisesCommandHandler> logger, IMapper mapper, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdatePaisesCommand request, CancellationToken cancellationToken)
        {
            var paisToUpdate = await _mediator.Send(new GetByIdPaisesQuery() { Id = request.Id });

            if (paisToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(Pais)} id {request.Id}");
                throw new NotFoundException(nameof(Pais), request.Id);
            }

            if (paisToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(Pais)} ya se encuentra anulado");
                throw new DeletedException(nameof(Pais), request.Id);
            }

            if (paisToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(Pais)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(Pais), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, paisToUpdate, typeof(UpdatePaisesCommand), typeof(Pais));

            var updatedPais = await _unitOfWork.RepositoryWrite<Pais>().UpdateAsync(paisToUpdate);

            _logger.LogInformation($"Update - {nameof(Pais)} {updatedPais.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdatePaisesCommand request)
        {

            var descripcionSpec = new PaisesSpecificationDescripcion(request.Descripcion!.ToUpper(), request.Id);
            var descripcionExists = await _unitOfWork.RepositoryRead<Pais>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Update - {nameof(Pais)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(Pais), request.Descripcion);
            }

            var idCAFCISpec = new PaisesSpecificationCAFCI(request.IdCAFCI!.ToUpper(), request.Id);
            var idCAFCIExists = await _unitOfWork.RepositoryRead<Pais>().GetByIdWithSpec(idCAFCISpec);

            if (idCAFCIExists != null)
            {
                _logger.LogError($"Update - {nameof(Pais)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(Pais), request.Descripcion);
            }
        }
    }
}
