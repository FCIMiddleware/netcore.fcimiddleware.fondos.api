using MediatR;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Update
{
    public class UpdateFondosCommand : IRequest
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; } = string.Empty;
        public int MonedaId { get; set; }
        public int SocGerenteId { get; set; }
        public int PaisId { get; set; }
        public int SocDepositariaId { get; set; }
    }
}
