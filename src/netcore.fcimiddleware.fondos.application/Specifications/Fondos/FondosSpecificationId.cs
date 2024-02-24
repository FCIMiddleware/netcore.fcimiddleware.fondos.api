using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Fondos
{
    public class FondosSpecificationId : BaseSpecification<Fondo>
    {
        public FondosSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
