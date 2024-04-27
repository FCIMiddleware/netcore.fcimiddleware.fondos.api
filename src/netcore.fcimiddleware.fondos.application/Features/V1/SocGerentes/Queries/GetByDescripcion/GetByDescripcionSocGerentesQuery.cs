using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetByDescripcion
{
    public class GetByDescripcionSocGerentesQuery : PaginationBaseQuery, IRequest<PaginationVm<SocGerenteListVm>>
    {
    }
}
