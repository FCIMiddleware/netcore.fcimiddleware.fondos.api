using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create
{
    public class CreateTpValorCptFondosCommand : IRequest<int>
    {
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
        public int FondoId { get; set; }
        public bool? IsSincronized { get; set; } = false;
    }
}
