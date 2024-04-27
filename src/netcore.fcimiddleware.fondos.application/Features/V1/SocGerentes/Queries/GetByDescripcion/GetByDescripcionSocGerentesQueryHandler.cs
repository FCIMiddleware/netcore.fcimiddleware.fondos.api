using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.application.Specifications.SocGerentes;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetByDescripcion
{
    public class GetByDescripcionSocGerentesQueryHandler : IRequestHandler<GetByDescripcionSocGerentesQuery, PaginationVm<SocGerenteListVm>>
    {
        private readonly ILogger<GetByDescripcionSocGerentesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByDescripcionSocGerentesQueryHandler(
            ILogger<GetByDescripcionSocGerentesQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<SocGerenteListVm>> Handle(GetByDescripcionSocGerentesQuery request, CancellationToken cancellationToken)
        {
            var socGerentesSpecificationParams = new SocGerentesSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new SocGerentesSpecification(socGerentesSpecificationParams);
            var socGerente = await _unitOfWork.RepositoryRead<SocGerente>().GetAllWithSpec(spec);

            var specCount = new SocGerentesForCountingSpecification(socGerentesSpecificationParams);
            var totalSocGerentes = await _unitOfWork.RepositoryRead<SocGerente>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalSocGerentes) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<SocGerente>, IReadOnlyList<SocGerenteListVm>>(socGerente);

            var pagination = new PaginationVm<SocGerenteListVm>
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
