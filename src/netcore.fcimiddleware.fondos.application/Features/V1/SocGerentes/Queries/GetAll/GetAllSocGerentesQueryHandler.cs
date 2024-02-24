using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetAll
{
    public class GetAllSocGerentesQueryHandler : IRequestHandler<GetAllSocGerentesQuery, PaginationVm<SocGerenteVm>>
    {
        private readonly ILogger<GetAllSocGerentesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllSocGerentesQueryHandler(
            ILogger<GetAllSocGerentesQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<SocGerenteVm>> Handle(GetAllSocGerentesQuery request, CancellationToken cancellationToken)
        {
            var socGerenteSpecificationParams = new SocGerentesSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new SocGerentesSpecification(socGerenteSpecificationParams);
            var socGerentes = await _unitOfWork.RepositoryRead<SocGerente>().GetAllWithSpec(spec);

            var specCount = new SocGerentesForCountingSpecification(socGerenteSpecificationParams);
            var totalSocGerentes = await _unitOfWork.RepositoryRead<SocGerente>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalSocGerentes) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<SocGerente>, IReadOnlyList<SocGerenteVm>>(socGerentes);

            var pagination = new PaginationVm<SocGerenteVm>
            {
                Count = totalSocGerentes,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
