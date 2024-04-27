﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using netcore.fcimiddleware.fondos.application.Contracts.Persistence;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Specifications.Monedas;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetByDescripcion
{
    public class GetByDescripcionMonedasQueryHandler : IRequestHandler<GetByDescripcionMonedasQuery, PaginationVm<MonedaListVm>>
    {
        private readonly ILogger<GetByDescripcionMonedasQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByDescripcionMonedasQueryHandler(
            ILogger<GetByDescripcionMonedasQueryHandler> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationVm<MonedaListVm>> Handle(GetByDescripcionMonedasQuery request, CancellationToken cancellationToken)
        {
            var monedaSpecificationParams = new MonedasSpecificationParams()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort
            };

            var spec = new MonedasSpecification(monedaSpecificationParams);
            var monedas = await _unitOfWork.RepositoryRead<Moneda>().GetAllWithSpec(spec);

            var specCount = new MonedasForCountingSpecification(monedaSpecificationParams);
            var totalMonedas = await _unitOfWork.RepositoryRead<Moneda>().CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalMonedas) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Moneda>, IReadOnlyList<MonedaListVm>>(monedas);

            var pagination = new PaginationVm<MonedaListVm>
            {
                Count = totalMonedas,
                Data = data,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagination;
        }
    }
}
