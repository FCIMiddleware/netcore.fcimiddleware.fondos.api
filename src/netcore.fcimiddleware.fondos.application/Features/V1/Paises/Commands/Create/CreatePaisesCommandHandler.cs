using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Specifications.Paises;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Create
{
    public class CreatePaisesCommandHandler : IRequestHandler<CreatePaisesCommand, int>
    {
        private readonly ILogger<CreatePaisesCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaisesCommandHandler(
            ILogger<CreatePaisesCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreatePaisesCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var paisEntity = _mapper.Map<Pais>(request);

            var newPais = await _unitOfWork.RepositoryWrite<Pais>().AddAsync(paisEntity);

            _logger.LogInformation($"Create - {nameof(Pais)} {newPais.Id} fue creado existosamente");

            return newPais.Id;
        }

        private async Task ValidateData(CreatePaisesCommand request)
        {
            var descripcionSpec = new PaisesSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<Pais>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(Pais)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(Pais), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var cafciSpec = new PaisesSpecificationCAFCI(request.IdCAFCI!.ToUpper());
                var idCAFCIExists = await _unitOfWork.RepositoryRead<Pais>().GetByIdWithSpec(cafciSpec);

                if (idCAFCIExists != null)
                {
                    _logger.LogError($"Create - {nameof(Pais)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(Pais), request.Descripcion);
                }
            }            
        }
    }
}
