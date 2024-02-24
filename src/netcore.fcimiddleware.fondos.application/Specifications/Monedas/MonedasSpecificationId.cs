using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Monedas
{
    public class MonedasSpecificationId : BaseSpecification<Moneda>
    {
        public MonedasSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
