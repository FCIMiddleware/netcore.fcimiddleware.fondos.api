using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.Fondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetAll
{
    public class GetAllFondosQueryHandler : IRequestHandler<GetAllFondosQuery, PaginationVm<FondoVm>>
    {
        private readonly ILogger<GetAllFondosQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFondosQueryHandler(
            ILogger<GetAllFondosQueryHandler> logger, 
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<FondoVm>> Handle(GetAllFondosQuery request, CancellationToken cancellationToken)
        {
            var fondoSpecificationParams = new FondosSpecificationParams()
            {
                PageIndex=request.PageIndex,
                PageSize=request.PageSize,
                Search=request.Search,
                Sort=request.Sort
            };

            var spec = new FondosSpecification(fondoSpecificationParams);
            var fondos = await _unitOfWork.RepositoryRead<Fondo>().GetAllWithSpec(spec);

            var specCount = new FondosForCountingSpecification(fondoSpecificationParams);
            var totalFondos = await _unitOfWork.RepositoryRead<Fondo>().CountAsync(specCount);
            
            var rounded = Math.Ceiling(Convert.ToDecimal(totalFondos) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Fondo>, IReadOnlyList<FondoVm>>(fondos);

            var pagination = new PaginationVm<FondoVm>
            {
                Count = totalFondos,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
