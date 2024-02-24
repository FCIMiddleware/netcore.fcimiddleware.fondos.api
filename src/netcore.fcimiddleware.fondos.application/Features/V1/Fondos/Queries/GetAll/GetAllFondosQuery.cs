using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetAll
{
    public class GetAllFondosQuery : PaginationBaseQuery, IRequest<PaginationVm<FondoVm>>
    {
       
    }
}
