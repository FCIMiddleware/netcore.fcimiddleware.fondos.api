using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create
{
    public class CreateCondIngEgrFondosCommand : IRequest<int>
    {
        public string Descripcion { get; set; }
        public int FondoId { get; set; }
        public int TpValorCptFondoId { get; set; }
    }
}
