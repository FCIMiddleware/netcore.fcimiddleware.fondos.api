using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetAll
{
    public class GetAllAgColocadoresQuery : PaginationBaseQuery, IRequest<PaginationVm<AgColocadorVm>>
    {
    }
}
