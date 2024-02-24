using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetAll
{
    public class GetAllPaisesQuery : PaginationBaseQuery, IRequest<PaginationVm<PaisVm>>
    {
    }
}
