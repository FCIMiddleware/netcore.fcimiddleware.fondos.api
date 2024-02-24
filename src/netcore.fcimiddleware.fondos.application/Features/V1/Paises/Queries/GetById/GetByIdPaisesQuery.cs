using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById
{
    public class GetByIdPaisesQuery : IdBaseQuery, IRequest<Pais>
    {
    }
}
