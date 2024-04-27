using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesSpecificationCAFCI : BaseSpecification<Pais>
    {
        public PaisesSpecificationCAFCI(string idCAFCI)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI.ToUpper()))
        {
        }
    }
}
