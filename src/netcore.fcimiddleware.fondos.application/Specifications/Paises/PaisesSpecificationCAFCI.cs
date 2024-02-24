using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesSpecificationCAFCI : BaseSpecification<Pais>
    {
        public PaisesSpecificationCAFCI(string idCAFCI, int id = -1)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI) && (p.Id! != id))
        {
        }
    }
}
