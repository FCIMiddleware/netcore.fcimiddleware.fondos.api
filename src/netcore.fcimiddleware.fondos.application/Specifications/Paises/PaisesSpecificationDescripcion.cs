using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesSpecificationDescripcion : BaseSpecification<Pais>
    {
        public PaisesSpecificationDescripcion(string descripcion)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion.ToUpper()))
        {
        }
    }
}
