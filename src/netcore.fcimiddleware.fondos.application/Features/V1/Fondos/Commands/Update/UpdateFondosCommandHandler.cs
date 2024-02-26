using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.Fondos;
using netcore.fcimiddleware.fondos.application.Specifications.Monedas;
using netcore.fcimiddleware.fondos.application.Specifications.Paises;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Update
{
    public class UpdateFondosCommandHandler : IRequestHandler<UpdateFondosCommand>
    {
        private readonly ILogger<UpdateFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateFondosCommandHandler(
            ILogger<UpdateFondosCommandHandler> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateFondosCommand request, CancellationToken cancellationToken)
        {
            var fondoToUpdate = await _mediator.Send(new GetByIdFondosQuery() { Id = request.Id });
            if (fondoToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(Fondo)} id {request.Id}");
                throw new NotFoundException(nameof(Fondo), request.Id);
            }
            
            if (fondoToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(Fondo)} ya se encuentra anulado");
                throw new DeletedException(nameof(Fondo), request.Id);
            }

            if (fondoToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(Fondo)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(Fondo), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, fondoToUpdate, typeof(UpdateFondosCommand), typeof(Fondo));

            var updatedFondo = await _unitOfWork.RepositoryWrite<Fondo>().UpdateAsync(fondoToUpdate);

            _logger.LogInformation($"Update - {nameof(Fondo)} {updatedFondo.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateFondosCommand request)
        {

            var descripcionSpec = new FondosSpecificationDescripcion(request.Descripcion!.ToUpper(),request.Id);
            var descripcionExists = await _unitOfWork.RepositoryRead<Fondo>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Update - {nameof(Fondo)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(Fondo), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var idCAFCISpec = new FondosSpecificationCAFCI(request.IdCAFCI!.ToUpper(), request.Id);
                var idCAFCIExists = await _unitOfWork.RepositoryRead<Fondo>().GetByIdWithSpec(idCAFCISpec);

                if (idCAFCIExists != null)
                {
                    _logger.LogError($"Update - {nameof(Fondo)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(Fondo), request.Descripcion);
                }
            }

            var monedaSpec = new MonedasSpecificationId(request.MonedaId);
            var monedaExists = await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(monedaSpec);

            if (monedaExists == null)
            {
                _logger.LogError($"Create - {nameof(Moneda)} {request.MonedaId} no existe");
                throw new NotFoundException(nameof(Moneda), request.MonedaId);
            }

            var paisSpec = new PaisesSpecificationId(request.PaisId);
            var paisExists = await _unitOfWork.RepositoryRead<Pais>().GetByIdWithSpec(paisSpec);

            if (paisExists == null)
            {
                _logger.LogError($"Create - {nameof(Pais)} {request.PaisId} no existe");
                throw new NotFoundException(nameof(Pais), request.PaisId);
            }

            var socDepositariaSpec = new SocDepositariasSpecificationId(request.SocDepositariaId);
            var socDepositariaExists = await _unitOfWork.RepositoryRead<SocDepositaria>().GetByIdWithSpec(socDepositariaSpec);

            if (socDepositariaExists == null)
            {
                _logger.LogError($"Create - {nameof(SocDepositaria)} {request.SocDepositariaId} no existe");
                throw new NotFoundException(nameof(SocDepositaria), request.SocDepositariaId);
            }

            var socGerenteSpec = new SocGerentesSpecificationId(request.SocGerenteId);
            var socGerenteExists = await _unitOfWork.RepositoryRead<SocGerente>().GetByIdWithSpec(socGerenteSpec);

            if (socGerenteExists == null)
            {
                _logger.LogError($"Create - {nameof(SocGerente)} {request.SocGerenteId} no existe");
                throw new NotFoundException(nameof(SocGerente), request.SocGerenteId);
            }
        }

    }
}
