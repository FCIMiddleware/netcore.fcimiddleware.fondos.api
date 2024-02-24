using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos
{
    public class TpValorCptFondosSpecificationId : BaseSpecification<TpValorCptFondo>
    {
        public TpValorCptFondosSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
