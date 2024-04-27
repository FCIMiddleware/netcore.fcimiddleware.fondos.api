using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Specifications.Fondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById
{
    public class GetByIdFondosQueryHandler : IRequestHandler<GetByIdFondosQuery, Fondo>
    {
        private readonly ILogger<GetByIdFondosQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdFondosQueryHandler(
            ILogger<GetByIdFondosQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Fondo> Handle(GetByIdFondosQuery request, CancellationToken cancellationToken)
        {
            var fondoSpec = new FondosSpecificationId(request.Id);
            return await _unitOfWork.RepositoryRead<Fondo>().GetByIdWithSpec(fondoSpec);            
        }
    }
}
