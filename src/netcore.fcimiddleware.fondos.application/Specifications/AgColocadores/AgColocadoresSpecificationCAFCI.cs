using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.AgColocadores
{
    public class AgColocadoresSpecificationCAFCI : BaseSpecification<AgColocador>
    {
        public AgColocadoresSpecificationCAFCI(string idCAFCI, int id = -1)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI.ToUpper()) && (p.Id! != id))
        {
        }
    }
}
