using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Create
{
    public class CreateSocDepositariasCommandHandler : IRequestHandler<CreateSocDepositariasCommand, int>
    {
        private readonly ILogger<CreateSocDepositariasCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSocDepositariasCommandHandler(
            ILogger<CreateSocDepositariasCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSocDepositariasCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var socGerenteEntity = _mapper.Map<SocDepositaria>(request);

            var newSocGerente = await _unitOfWork.RepositoryWrite<SocDepositaria>().AddAsync(socGerenteEntity);

            _logger.LogInformation($"Create - {nameof(SocDepositaria)} {newSocGerente.Id} fue creado existosamente");

            return newSocGerente.Id;
        }

        private async Task ValidateData(CreateSocDepositariasCommand request)
        {
            var descripcionSpec = new SocDepositariasSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<SocDepositaria>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(SocDepositaria)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(SocDepositaria), request.Descripcion);
            }

            var cafciSpec = new SocDepositariasSpecificationCAFCI(request.IdCAFCI!.ToUpper());
            var idCAFCIExists = await _unitOfWork.RepositoryRead<SocDepositaria>().GetByIdWithSpec(cafciSpec);

            if (idCAFCIExists != null)
            {
                _logger.LogError($"Create - {nameof(SocDepositaria)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(SocDepositaria), request.Descripcion);
            }
        }
    }
}
