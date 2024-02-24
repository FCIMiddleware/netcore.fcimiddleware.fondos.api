using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesForCountingSpecification : BaseSpecification<Pais>
    {
        public PaisesForCountingSpecification(PaisesSpecificationParams paisesParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(paisesParams.Search) || x.Descripcion!.Contains(paisesParams.Search)
                  )
        { }
    }
}
