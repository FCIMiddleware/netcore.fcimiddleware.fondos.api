using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Specifications.Paises;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById
{
    public class GetByIdPaisesQueryHandler : IRequestHandler<GetByIdPaisesQuery, Pais>
    {
        private readonly ILogger<GetByIdPaisesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdPaisesQueryHandler(
            ILogger<GetByIdPaisesQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Pais> Handle(GetByIdPaisesQuery request, CancellationToken cancellationToken)
        {
            var paisSpec = new PaisesSpecificationId(request.Id);
            return await _unitOfWork.RepositoryRead<Pais>().GetByIdWithSpec(paisSpec);
        }
    }
}
