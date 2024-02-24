using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos
{
    public class TpValorCptFondosSpecificationDescripcion : BaseSpecification<TpValorCptFondo>
    {
        public TpValorCptFondosSpecificationDescripcion(string descripcion, int id = -1)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion) && (p.Id! != id))
        {
        }
    }
}
