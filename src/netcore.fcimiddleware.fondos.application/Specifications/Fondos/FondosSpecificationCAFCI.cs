using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Fondos
{
    public class FondosSpecificationCAFCI : BaseSpecification<Fondo>
    {
        public FondosSpecificationCAFCI(string idCAFCI, int id = -1) 
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI.ToUpper()) && (p.Id! != id))
        {
        }
    }
}
