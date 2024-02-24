using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.domain
{
    public class Fondo : BaseDomainModel
    {
        //public Guid GUID { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public int MonedaId { get; set; }
        public Moneda Monedas { get; set; }
        public int SocGerenteId { get; set; }
        public SocGerente SocGerentes { get; set; }
        public int PaisId { get; set; }
        public Pais Paises { get; set; }
        public int SocDepositariaId { get; set; }
        public SocDepositaria SocDepositarias { get; set; }
        public ICollection<TpValorCptFondo> TpValorCptFondos { get; set; }
    }
}
