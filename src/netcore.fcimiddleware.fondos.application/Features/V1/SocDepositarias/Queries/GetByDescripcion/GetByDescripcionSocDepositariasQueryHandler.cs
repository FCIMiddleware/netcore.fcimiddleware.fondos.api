﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetByDescripcion
{
    public class GetByDescripcionSocDepositariasQueryHandler : IRequestHandler<GetByDescripcionSocDepositariasQuery, PaginationVm<SocDepositariaListVm>>
    {
        private readonly ILogger<GetByDescripcionSocDepositariasQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByDescripcionSocDepositariasQueryHandler(
            ILogger<GetByDescripcionSocDepositariasQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<SocDepositariaListVm>> Handle(GetByDescripcionSocDepositariasQuery request, CancellationToken cancellationToken)
        {
            var socDepositariasSpecificationParams = new SocDepositariasSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new SocDepositariasSpecification(socDepositariasSpecificationParams);
            var monedas = await _unitOfWork.RepositoryRead<SocDepositaria>().GetAllWithSpec(spec);

            var specCount = new SocDepositariasForCountingSpecification(socDepositariasSpecificationParams);
            var totalSocDepositarias = await _unitOfWork.RepositoryRead<SocDepositaria>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalSocDepositarias) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<SocDepositaria>, IReadOnlyList<SocDepositariaListVm>>(monedas);

            var pagination = new PaginationVm<SocDepositariaListVm>
            {
                Count = totalSocDepositarias,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
