using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Specifications.Fondos;
using netcore.fcimiddleware.fondos.application.Specifications.Monedas;
using netcore.fcimiddleware.fondos.application.Specifications.Paises;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create
{
    public class FondosCommandHandler : IRequestHandler<CreateFondosCommand, int>
    {
        private readonly ILogger<FondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public FondosCommandHandler(
            ILogger<FondosCommandHandler> logger, 
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateFondosCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var fondoEntity = _mapper.Map<Fondo>(request);
            
            var newFondo = await _unitOfWork.RepositoryWrite<Fondo>().AddAsync(fondoEntity);
            
            _logger.LogInformation($"Create - {nameof(Fondo)} {newFondo.Id} fue creado existosamente");

            return newFondo.Id;
        }

        private async Task ValidateData(CreateFondosCommand request)
        {
            var descripcionSpec = new FondosSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<Fondo>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(Fondo)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(Fondo), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var cafciSpec = new FondosSpecificationCAFCI(request.IdCAFCI!.ToUpper());
                var idCAFCIExists = await _unitOfWork.RepositoryRead<Fondo>().GetByIdWithSpec(cafciSpec);

                if (idCAFCIExists != null)
                {
                    _logger.LogError($"Create - {nameof(Fondo)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(Fondo), request.Descripcion);
                }
            }            

            var monedaSpec = new MonedasSpecificationId(request.MonedaId);
            var monedaExists = await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(monedaSpec);

            if(monedaExists == null)
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
