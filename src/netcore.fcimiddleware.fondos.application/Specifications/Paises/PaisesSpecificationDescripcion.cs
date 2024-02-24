using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesSpecificationDescripcion : BaseSpecification<Pais>
    {
        public PaisesSpecificationDescripcion(string descripcion, int id = -1)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion) && (p.Id! != id))
        {
        }
    }
}
