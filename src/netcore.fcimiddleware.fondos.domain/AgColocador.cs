using netcore.fcimiddleware.fondos.domain.Common;

namespace netcore.fcimiddleware.fondos.domain
{
    public class AgColocador : BaseDomainModel
    {
        public string Descripcion { get; set; }
        public string? IdCAFCI { get; set; }
    }
}
