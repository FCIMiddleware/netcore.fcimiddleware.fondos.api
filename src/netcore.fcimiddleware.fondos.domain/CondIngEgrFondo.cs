using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.domain
{
    public class CondIngEgrFondo : BaseDomainModel
    {
        public string Descripcion { get; set; }
        public int FondoId { get; set; }
        public Fondo Fondos { get; set; }
        public int TpValorCptFondoId { get; set; }
        public TpValorCptFondo TpValorCptFondo { get; set; }
    }
}
