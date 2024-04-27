using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Monedas
{
    public class MonedasForCountingSpecification : BaseSpecification<Moneda>
    {
        public MonedasForCountingSpecification(MonedasSpecificationParams monedasParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(monedasParams.Search) || x.Descripcion!.ToUpper().Contains(monedasParams.Search.ToUpper())
                  )
        { }
    }
}
