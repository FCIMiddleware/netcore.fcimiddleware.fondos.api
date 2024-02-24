using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetAll
{
    public class GetAllTpValorCptFondosQuery : PaginationBaseQuery, IRequest<PaginationVm<TpValorCptFondoVm>>
    {
    }
}
