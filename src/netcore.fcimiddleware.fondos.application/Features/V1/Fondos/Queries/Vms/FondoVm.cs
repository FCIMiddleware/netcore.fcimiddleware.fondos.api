using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.Vms
{
    public class FondoVm
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
        public int MonedaId { get; set; }
        public MonedaVm Monedas { get; set; }
        public int SocGerenteId { get; set; }
        public SocGerenteVm SocGerentes { get; set; }
        public int PaisId { get; set; }
        public int SocDepositariaId { get; set; }
        public SocDepositariaVm SocDepositarias { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsSincronized { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        
    }
}
