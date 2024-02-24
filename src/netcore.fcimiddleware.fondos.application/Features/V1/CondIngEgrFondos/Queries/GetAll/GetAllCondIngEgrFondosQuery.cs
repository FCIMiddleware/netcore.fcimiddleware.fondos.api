using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetAll
{
    public class GetAllCondIngEgrFondosQuery : PaginationBaseQuery, IRequest<PaginationVm<CondIngEgrFondoVm>>
    {
    }
}
