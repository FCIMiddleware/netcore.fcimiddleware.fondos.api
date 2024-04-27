using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Monedas
{
    public class MonedasSpecificationCAFCI : BaseSpecification<Moneda>
    {
        public MonedasSpecificationCAFCI(string idCAFCI, int id = -1)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI.ToUpper()) && (p.Id! != id))
        {
        }
    }
}
