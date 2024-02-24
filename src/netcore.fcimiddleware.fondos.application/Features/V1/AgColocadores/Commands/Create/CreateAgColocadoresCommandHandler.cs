using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Specifications.AgColocadores;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Create
{
    public class CreateAgColocadoresCommandHandler : IRequestHandler<CreateAgColocadoresCommand, int>
    {
        private readonly ILogger<CreateAgColocadoresCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAgColocadoresCommandHandler(
            ILogger<CreateAgColocadoresCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateAgColocadoresCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var agColocadorEntity = _mapper.Map<AgColocador>(request);

            var newAgColocador = await _unitOfWork.RepositoryWrite<AgColocador>().AddAsync(agColocadorEntity);

            _logger.LogInformation($"Create - {nameof(AgColocador)} {newAgColocador.Id} fue creado existosamente");

            return newAgColocador.Id;
        }

        private async Task ValidateData(CreateAgColocadoresCommand request)
        {
            var descripcionSpec = new AgColocadoresSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<AgColocador>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(AgColocador)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(AgColocador), request.Descripcion);
            }

            var cafciSpec = new AgColocadoresSpecificationCAFCI(request.IdCAFCI!.ToUpper());
            var idCAFCIExists = await _unitOfWork.RepositoryRead<AgColocador>().GetByIdWithSpec(cafciSpec);

            if (idCAFCIExists != null)
            {
                _logger.LogError($"Create - {nameof(AgColocador)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(AgColocador), request.Descripcion);
            }
        }
    }
}
