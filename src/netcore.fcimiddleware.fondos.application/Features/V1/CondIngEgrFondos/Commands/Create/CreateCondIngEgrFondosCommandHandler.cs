using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create
{
    public class CreateCondIngEgrFondosCommandHandler : IRequestHandler<CreateCondIngEgrFondosCommand, int>
    {
        private readonly ILogger<CreateCondIngEgrFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateCondIngEgrFondosCommandHandler(
            ILogger<CreateCondIngEgrFondosCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateCondIngEgrFondosCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var condIngEgrFondoEntity = _mapper.Map<CondIngEgrFondo>(request);

            var newCondIngEgrFondo = await _unitOfWork.RepositoryWrite<CondIngEgrFondo>().AddAsync(condIngEgrFondoEntity);

            _logger.LogInformation($"Create - {nameof(CondIngEgrFondo)} {newCondIngEgrFondo.Id} fue creado existosamente");

            return newCondIngEgrFondo.Id;
        }

        private async Task ValidateData(CreateCondIngEgrFondosCommand request)
        {
            var descripcionSpec = new CondIngEgrFondosSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<CondIngEgrFondo>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(CondIngEgrFondo)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(CondIngEgrFondo), request.Descripcion);
            }

            var fondoExists = await _mediator.Send(new GetByIdFondosQuery() { Id = request.FondoId });
            if (fondoExists == null)
            {
                _logger.LogError($"Create - {nameof(Fondo)} {request.FondoId} no existe");
                throw new NotFoundException(nameof(Fondo), request.FondoId);
            }
            
            var tpValorCptFondoExists = await _mediator.Send(new GetByIdTpValorCptFondosQuery() { Id = request.TpValorCptFondoId });
            if (tpValorCptFondoExists == null)
            {
                _logger.LogError($"Create - {nameof(TpValorCptFondo)} {request.TpValorCptFondoId} no existe");
                throw new NotFoundException(nameof(TpValorCptFondo), request.TpValorCptFondoId);
            }
        }
    }
}
