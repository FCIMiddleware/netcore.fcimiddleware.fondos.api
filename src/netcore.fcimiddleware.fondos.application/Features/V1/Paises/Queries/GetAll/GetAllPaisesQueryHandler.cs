using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.Paises;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetAll
{
    public class GetAllPaisesQueryHandler : IRequestHandler<GetAllPaisesQuery, PaginationVm<PaisVm>>
    {
        private readonly ILogger<GetAllPaisesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPaisesQueryHandler(
            ILogger<GetAllPaisesQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<PaisVm>> Handle(GetAllPaisesQuery request, CancellationToken cancellationToken)
        {
            var paisSpecificationParams = new PaisesSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new PaisesSpecification(paisSpecificationParams);
            var paises = await _unitOfWork.RepositoryRead<Pais>().GetAllWithSpec(spec);

            var specCount = new PaisesForCountingSpecification(paisSpecificationParams);
            var totalPaises = await _unitOfWork.RepositoryRead<Pais>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalPaises) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Pais>, IReadOnlyList<PaisVm>>(paises);

            var pagination = new PaginationVm<PaisVm>
            {
                Count = totalPaises,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
