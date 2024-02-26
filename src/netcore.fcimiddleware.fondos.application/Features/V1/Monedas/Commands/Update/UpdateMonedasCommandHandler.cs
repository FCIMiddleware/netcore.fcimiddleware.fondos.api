using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Specifications.Monedas;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Update
{
    public class UpdateMonedasCommandHandler : IRequestHandler<UpdateMonedasCommand>
    {
        private readonly ILogger<UpdateMonedasCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateMonedasCommandHandler(
            ILogger<UpdateMonedasCommandHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateMonedasCommand request, CancellationToken cancellationToken)
        {
            var monedaToUpdate = await _mediator.Send(new GetByIdMonedasQuery() { Id = request.Id });

            if (monedaToUpdate == null)
            {
                _logger.LogError($"Update - No se encontro el {nameof(Moneda)} id {request.Id}");
                throw new NotFoundException(nameof(Moneda), request.Id);
            }

            if (monedaToUpdate.IsDeleted)
            {
                _logger.LogError($"El {request.Id} de {nameof(Moneda)} ya se encuentra anulado");
                throw new DeletedException(nameof(Moneda), request.Id);
            }

            if (monedaToUpdate.IsSincronized)
            {
                _logger.LogError($"El {request.Id} de {nameof(Moneda)} está sincronizado y no puede modificarse");
                throw new SincronizedException(nameof(Moneda), request.Id);
            }

            await ValidateData(request);

            _mapper.Map(request, monedaToUpdate, typeof(UpdateMonedasCommand), typeof(Moneda));

            var updatedMoneda = await _unitOfWork.RepositoryWrite<Moneda>().UpdateAsync(monedaToUpdate);

            _logger.LogInformation($"Update - {nameof(Moneda)} {updatedMoneda.Id} fue actualizado existosamente");

            return Unit.Value;
        }

        private async Task ValidateData(UpdateMonedasCommand request)
        {

            var descripcionSpec = new MonedasSpecificationDescripcion(request.Descripcion!.ToUpper(), request.Id);
            var descripcionExists = await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(descripcionSpec);

            if (descripcionExists != null)
            {
                _logger.LogError($"Update - {nameof(Moneda)} {descripcionExists.Descripcion} ya existe");
                throw new AlreadyExistsException(nameof(Moneda), request.Descripcion);
            }

            if (request.IdCAFCI != null)
            {
                var idCAFCISpec = new MonedasSpecificationCAFCI(request.IdCAFCI!.ToUpper(), request.Id);
                var idCAFCIExists = await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(idCAFCISpec);

                if (idCAFCIExists != null)
                {
                    _logger.LogError($"Update - {nameof(Moneda)} {idCAFCIExists.IdCAFCI} ya existe");
                    throw new AlreadyExistsException(nameof(Moneda), request.Descripcion);
                }
            }            
        }
    }
}
