using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetById
{
    public class GetByIdSocGerentesQueryHandler : IRequestHandler<GetByIdSocGerentesQuery, SocGerente>
    {
        private readonly ILogger<GetByIdSocGerentesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdSocGerentesQueryHandler(
            ILogger<GetByIdSocGerentesQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SocGerente> Handle(GetByIdSocGerentesQuery request, CancellationToken cancellationToken)
        {
            var socGerenteSpec = new SocGerentesSpecificationId(request.Id);
            return await _unitOfWork.RepositoryRead<SocGerente>().GetByIdWithSpec(socGerenteSpec);
        }
    }
}
