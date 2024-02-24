using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById
{
    public class GetByIdSocDepositariasQueryHandler : IRequestHandler<GetByIdSocDepositariasQuery, SocDepositaria>
    {
        private readonly ILogger<GetByIdSocDepositariasQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdSocDepositariasQueryHandler(
            ILogger<GetByIdSocDepositariasQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SocDepositaria> Handle(GetByIdSocDepositariasQuery request, CancellationToken cancellationToken)
        {
            var socDepositariaSpec = new SocDepositariasSpecificationId(request.Id);
            return await _unitOfWork.RepositoryRead<SocDepositaria>().GetByIdWithSpec(socDepositariaSpec);
        }
    }
}
