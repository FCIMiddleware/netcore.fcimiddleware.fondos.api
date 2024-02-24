using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos
{
    public class TpValorCptFondosSpecificationCAFCI : BaseSpecification<TpValorCptFondo>
    {
        public TpValorCptFondosSpecificationCAFCI(string idCAFCI, int id = -1)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI) && (p.Id! != id))
        {
        }
    }
}
