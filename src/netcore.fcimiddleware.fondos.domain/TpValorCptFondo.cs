using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.domain
{
    public class TpValorCptFondo : BaseDomainModel
    {
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public int FondoId { get; set; }
        public Fondo Fondos { get; set; }
        public ICollection<CondIngEgrFondo> CondIngEgrFondos { get; set; }
    }
}
