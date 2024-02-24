using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesSpecificationId : BaseSpecification<Pais>
    {
        public PaisesSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
