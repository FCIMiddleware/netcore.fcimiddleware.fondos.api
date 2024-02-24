using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.AgColocadores;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetAll
{
    public class GetAllAgColocadoresQueryHandler : IRequestHandler<GetAllAgColocadoresQuery, PaginationVm<AgColocadorVm>>
    {
        private readonly ILogger<GetAllAgColocadoresQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAgColocadoresQueryHandler(
            ILogger<GetAllAgColocadoresQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<AgColocadorVm>> Handle(GetAllAgColocadoresQuery request, CancellationToken cancellationToken)
        {
            var agColocadoresSpecificationParams = new AgColocadoresSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new AgColocadoresSpecification(agColocadoresSpecificationParams);
            var agColocadores = await _unitOfWork.RepositoryRead<AgColocador>().GetAllWithSpec(spec);

            var specCount = new AgColocadoresForCountingSpecification(agColocadoresSpecificationParams);
            var totalAgColocadores = await _unitOfWork.RepositoryRead<AgColocador>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalAgColocadores) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<AgColocador>, IReadOnlyList<AgColocadorVm>>(agColocadores);

            var pagination = new PaginationVm<AgColocadorVm>
            {
                Count = totalAgColocadores,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
