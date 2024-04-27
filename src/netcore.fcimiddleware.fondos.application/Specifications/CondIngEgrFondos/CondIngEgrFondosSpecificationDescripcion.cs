using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos
{
    public class CondIngEgrFondosSpecificationDescripcion : BaseSpecification<CondIngEgrFondo>
    {
        public CondIngEgrFondosSpecificationDescripcion(string descripcion, int id = -1)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion.ToUpper()) && (p.Id! != id))
        {
        }
    }
}
