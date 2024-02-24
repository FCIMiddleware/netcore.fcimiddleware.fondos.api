using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Update
{
    public class UpdateMonedasCommand : IRequest
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
    }
}
