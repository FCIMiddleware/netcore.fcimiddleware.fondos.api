using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetAll
{
    public class GetAllCondIngEgrFondosQueryHandler : IRequestHandler<GetAllCondIngEgrFondosQuery, PaginationVm<CondIngEgrFondoVm>>
    {
        private readonly ILogger<GetAllCondIngEgrFondosQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCondIngEgrFondosQueryHandler(
            ILogger<GetAllCondIngEgrFondosQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<CondIngEgrFondoVm>> Handle(GetAllCondIngEgrFondosQuery request, CancellationToken cancellationToken)
        {
            var fondoSpecificationParams = new CondIngEgrFondosSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new CondIngEgrFondosSpecification(fondoSpecificationParams);
            var condIngEgrFondo = await _unitOfWork.RepositoryRead<CondIngEgrFondo>().GetAllWithSpec(spec);

            var specCount = new CondIngEgrFondosForCountingSpecification(fondoSpecificationParams);
            var totalCondIngEgrFondos = await _unitOfWork.RepositoryRead<CondIngEgrFondo>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalCondIngEgrFondos) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<CondIngEgrFondo>, IReadOnlyList<CondIngEgrFondoVm>>(condIngEgrFondo);

            var pagination = new PaginationVm<CondIngEgrFondoVm>
            {
                Count = totalCondIngEgrFondos,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
