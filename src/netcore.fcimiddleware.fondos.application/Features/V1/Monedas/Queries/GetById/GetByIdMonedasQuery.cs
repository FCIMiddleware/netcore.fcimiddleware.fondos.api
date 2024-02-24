using MediatR;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById
{
    public class GetByIdMonedasQuery : IdBaseQuery, IRequest<Moneda>
    {
    }
}
