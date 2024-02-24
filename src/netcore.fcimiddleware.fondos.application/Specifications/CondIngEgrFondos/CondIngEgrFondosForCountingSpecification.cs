using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos
{
    public class CondIngEgrFondosForCountingSpecification : BaseSpecification<CondIngEgrFondo>
    {
        public CondIngEgrFondosForCountingSpecification(CondIngEgrFondosSpecificationParams condIngEgrFondosParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(condIngEgrFondosParams.Search) || x.Descripcion!.Contains(condIngEgrFondosParams.Search)
                  )
        { }
    }
}
