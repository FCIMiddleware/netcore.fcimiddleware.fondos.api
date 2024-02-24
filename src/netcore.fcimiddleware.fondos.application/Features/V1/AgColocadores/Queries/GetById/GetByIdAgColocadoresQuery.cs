using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.GetById
{
    public class GetByIdAgColocadoresQuery : IdBaseQuery, IRequest<AgColocador>
    {
    }
}
