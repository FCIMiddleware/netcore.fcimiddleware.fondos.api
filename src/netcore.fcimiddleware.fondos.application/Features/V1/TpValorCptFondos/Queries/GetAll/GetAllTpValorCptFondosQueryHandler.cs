using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetAll
{
    public class GetAllTpValorCptFondosQueryHandler : IRequestHandler<GetAllTpValorCptFondosQuery, PaginationVm<TpValorCptFondoVm>>
    {
        private readonly ILogger<GetAllTpValorCptFondosQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllTpValorCptFondosQueryHandler(
            ILogger<GetAllTpValorCptFondosQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<TpValorCptFondoVm>> Handle(GetAllTpValorCptFondosQuery request, CancellationToken cancellationToken)
        {
            var tpValorCptFondoSpecificationParams = new TpValorCptFondosSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new TpValorCptFondosSpecification(tpValorCptFondoSpecificationParams);
            var fondos = await _unitOfWork.RepositoryRead<TpValorCptFondo>().GetAllWithSpec(spec);

            var specCount = new TpValorCptFondosForCountingSpecification(tpValorCptFondoSpecificationParams);
            var totalFondos = await _unitOfWork.RepositoryRead<TpValorCptFondo>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalFondos) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<TpValorCptFondo>, IReadOnlyList<TpValorCptFondoVm>>(fondos);

            var pagination = new PaginationVm<TpValorCptFondoVm>
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
