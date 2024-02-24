using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetById
{
    public class GetByIdSocGerentesQuery : IdBaseQuery, IRequest<SocGerente>
    {
    }
}
