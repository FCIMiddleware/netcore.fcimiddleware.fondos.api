using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Create
{
    public class CreateSocGerentesCommandHandler : IRequestHandler<CreateSocGerentesCommand, int>
    {
        private readonly ILogger<CreateSocGerentesCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSocGerentesCommandHandler(
            ILogger<CreateSocGerentesCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSocGerentesCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var socGerenteEntity = _mapper.Map<SocGerente>(request);

            var newSocGerente = await _unitOfWork.RepositoryWrite<SocGerente>().AddAsync(socGerenteEntity);

            _logger.LogInformation($"Create - {nameof(SocGerente)} {newSocGerente.Id} fue creado existosamente");

            return newSocGerente.Id;
        }

        private async Task ValidateData(CreateSocGerentesCommand request)
        {
            var descripcionSpec = new SocGerentesSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<SocGerente>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(SocGerente)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(SocGerente), request.Descripcion);
            }

            var cafciSpec = new SocGerentesSpecificationCAFCI(request.IdCAFCI!.ToUpper());
            var idCAFCIExists = await _unitOfWork.RepositoryRead<SocGerente>().GetByIdWithSpec(cafciSpec);

            if (idCAFCIExists != null)
            {
                _logger.LogError($"Create - {nameof(SocGerente)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(SocGerente), request.Descripcion);
            }
        }
    }
}
