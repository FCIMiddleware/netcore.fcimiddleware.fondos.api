using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetAll
{
    public class GetAllSocGerentesQuery : PaginationBaseQuery, IRequest<PaginationVm<SocGerenteVm>>
    {
    }
}
