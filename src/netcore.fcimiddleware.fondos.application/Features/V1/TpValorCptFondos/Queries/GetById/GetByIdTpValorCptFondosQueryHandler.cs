using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById
{
    public class GetByIdTpValorCptFondosQueryHandler : IRequestHandler<GetByIdTpValorCptFondosQuery, TpValorCptFondo>
    {
        private readonly ILogger<GetByIdTpValorCptFondosQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdTpValorCptFondosQueryHandler(
            ILogger<GetByIdTpValorCptFondosQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TpValorCptFondo> Handle(GetByIdTpValorCptFondosQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RepositoryRead<TpValorCptFondo>().GetByIdAsync(request.Id);
        }
    }
}
