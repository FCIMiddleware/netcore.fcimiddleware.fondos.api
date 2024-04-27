using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetByDescripcion
{
    public class GetByDescripcionSocDepositariasQuery : PaginationBaseQuery, IRequest<PaginationVm<SocDepositariaListVm>>
    {
    }
}
