using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Update
{
    public class UpdateCondIngEgrFondosCommand : IRequest
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int FondoId { get; set; }
        public int TpValorCptFondoId { get; set; }
    }
}
