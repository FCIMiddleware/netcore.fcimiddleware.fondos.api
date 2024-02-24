using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create
{
    public class CreateTpValorCptFondosCommandHandler : IRequestHandler<CreateTpValorCptFondosCommand, int>
    {
        private readonly ILogger<CreateTpValorCptFondosCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateTpValorCptFondosCommandHandler(
            ILogger<CreateTpValorCptFondosCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateTpValorCptFondosCommand request, CancellationToken cancellationToken)
        {
            await ValidateData(request);
            var tpValorCptFondoEntity = _mapper.Map<TpValorCptFondo>(request);

            var newTpValorCptFondo = await _unitOfWork.RepositoryWrite<TpValorCptFondo>().AddAsync(tpValorCptFondoEntity);

            _logger.LogInformation($"Create - {nameof(TpValorCptFondo)} {newTpValorCptFondo.Id} fue creado existosamente");

            return newTpValorCptFondo.Id;
        }

        private async Task ValidateData(CreateTpValorCptFondosCommand request)
        {
            var descripcionSpec = new TpValorCptFondosSpecificationDescripcion(request.Descripcion!.ToUpper());
            var descripcionExists = await _unitOfWork.RepositoryRead<TpValorCptFondo>().GetByIdWithSpec(descripcionSpec);
            if (descripcionExists != null)
            {
                _logger.LogError($"Create - {nameof(TpValorCptFondo)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(TpValorCptFondo), request.Descripcion);
            }

            var cafciSpec = new TpValorCptFondosSpecificationCAFCI(request.IdCAFCI!.ToUpper());
            var idCAFCIExists = await _unitOfWork.RepositoryRead<TpValorCptFondo>().GetByIdWithSpec(cafciSpec);
            if (idCAFCIExists != null)
            {
                _logger.LogError($"Create - {nameof(TpValorCptFondo)} {idCAFCIExists.IdCAFCI} ya existe");
                throw new AlreadyExistsException(nameof(TpValorCptFondo), request.Descripcion);
            }

            var fondoExists = await _mediator.Send(new GetByIdFondosQuery() { Id = request.FondoId });
            if (fondoExists == null)
            {
                _logger.LogError($"Create - {nameof(Fondo)} {request.FondoId} no existe");
                throw new NotFoundException(nameof(Fondo), request.FondoId);
            }
        }
    }
}
