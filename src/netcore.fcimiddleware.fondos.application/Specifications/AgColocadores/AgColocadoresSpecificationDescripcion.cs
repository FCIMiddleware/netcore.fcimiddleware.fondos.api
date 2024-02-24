using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.AgColocadores
{
    public class AgColocadoresSpecificationDescripcion : BaseSpecification<AgColocador>
    {
        public AgColocadoresSpecificationDescripcion(string descripcion, int id = -1)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion) && (p.Id! != id))
        {
        }
    }
}
