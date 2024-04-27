using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Specifications.Monedas;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Create
{
    public class CreateMonedasCommandHandler : IRequestHandler<CreateMonedasCommand, int>
    {
        private readonly ILogger<CreateMonedasCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMonedasCommandHandler(
            ILogger<CreateMonedasCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateMonedasCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var monedaEntity = _mapper.Map<Moneda>(request);

            var newMoneda = await _unitOfWork.RepositoryWrite<Moneda>().AddAsync(monedaEntity);

            _logger.LogInformation($"Create - {nameof(Moneda)} {newMoneda.Id} fue creado existosamente");

            return newMoneda.Id;
        }

        private async Task ValidateData(CreateMonedasCommand request)
        {
            var descripcionSpec = new MonedasSpecificationDescripcion(request.Descripcion);
            var descripcionExists = await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(Moneda)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(Moneda), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var cafciSpec = new MonedasSpecificationCAFCI(request.IdCAFCI!.ToUpper());
                var idCAFCIExists = await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(cafciSpec);

                if (idCAFCIExists != null)
                {
                    _logger.LogError($"Create - {nameof(Moneda)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(Moneda), request.Descripcion);
                }
            }            
        }
    }
}
