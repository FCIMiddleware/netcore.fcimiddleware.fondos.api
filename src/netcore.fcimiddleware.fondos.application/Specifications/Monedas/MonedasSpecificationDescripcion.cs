using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Monedas
{
    public class MonedasSpecificationDescripcion : BaseSpecification<Moneda>
    {
        public MonedasSpecificationDescripcion(string descripcion)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion.ToUpper()))
        {
        }
    }
}
