using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Fondos
{
    public class FondosSpecificationDescripcion : BaseSpecification<Fondo>
    {
        public FondosSpecificationDescripcion(string descripcion, int id = -1) 
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion) && (p.Id! != id))
        {
        }
    }
}
