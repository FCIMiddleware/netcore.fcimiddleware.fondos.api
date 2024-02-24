using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Fondos
{
    public class FondosForCountingSpecification : BaseSpecification<Fondo>
    {
        public FondosForCountingSpecification(FondosSpecificationParams fondosParams) 
            : base (
                  x => 
                  string.IsNullOrEmpty(fondosParams.Search) || x.Descripcion!.Contains(fondosParams.Search)
                  )
        { }
    }
}
