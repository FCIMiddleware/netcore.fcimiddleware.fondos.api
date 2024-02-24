using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetById
{
    public class GetByIdCondIngEgrFondosQueryHandler : IRequestHandler<GetByIdCondIngEgrFondosQuery, CondIngEgrFondo>
    {
        private readonly ILogger<GetByIdCondIngEgrFondosQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdCondIngEgrFondosQueryHandler(
            ILogger<GetByIdCondIngEgrFondosQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CondIngEgrFondo> Handle(GetByIdCondIngEgrFondosQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RepositoryRead<CondIngEgrFondo>().GetByIdAsync(request.Id);
        }
    }
}
