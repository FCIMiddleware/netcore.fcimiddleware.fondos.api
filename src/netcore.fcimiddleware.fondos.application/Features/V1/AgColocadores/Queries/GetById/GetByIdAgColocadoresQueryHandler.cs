using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Specifications.AgColocadores;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetById
{
    public class GetByIdAgColocadoresQueryHandler : IRequestHandler<GetByIdAgColocadoresQuery, AgColocador>
    {
        private readonly ILogger<GetByIdAgColocadoresQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdAgColocadoresQueryHandler(
            ILogger<GetByIdAgColocadoresQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AgColocador> Handle(GetByIdAgColocadoresQuery request, CancellationToken cancellationToken)
        {
            var agColocadoresSpec = new AgColocadoresSpecificationId(request.Id);
            return await _unitOfWork.RepositoryRead<AgColocador>().GetByIdWithSpec(agColocadoresSpec);
        }
    }
}
