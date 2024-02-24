using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Specifications.Monedas;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById
{
    public class GetByIdMonedasQueryHandler : IRequestHandler<GetByIdMonedasQuery, Moneda>
    {
        private readonly ILogger<GetByIdMonedasQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdMonedasQueryHandler(
            ILogger<GetByIdMonedasQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Moneda> Handle(GetByIdMonedasQuery request, CancellationToken cancellationToken)
        {
            var monedasSpec = new MonedasSpecificationId(request.Id);
            return await _unitOfWork.RepositoryRead<Moneda>().GetByIdWithSpec(monedasSpec);
        }
    }
}
