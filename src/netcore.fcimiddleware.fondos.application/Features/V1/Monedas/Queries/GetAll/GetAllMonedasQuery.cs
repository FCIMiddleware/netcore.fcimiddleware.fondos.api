using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetAll
{
    public class GetAllMonedasQuery : PaginationBaseQuery, IRequest<PaginationVm<MonedaVm>>
    {
    }
}
