using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos
{
    public class CondIngEgrFondosSpecificationId : BaseSpecification<CondIngEgrFondo>
    {
        public CondIngEgrFondosSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
