using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos
{
    public class TpValorCptFondosForCountingSpecification : BaseSpecification<TpValorCptFondo>
    {
        public TpValorCptFondosForCountingSpecification(TpValorCptFondosSpecificationParams tpValorCptFondoParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(tpValorCptFondoParams.Search) || x.Descripcion!.Contains(tpValorCptFondoParams.Search)
                  )
        { }
    }
}
