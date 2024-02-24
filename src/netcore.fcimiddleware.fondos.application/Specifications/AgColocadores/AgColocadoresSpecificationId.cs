using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.AgColocadores
{
    public class AgColocadoresSpecificationId : BaseSpecification<AgColocador>
    {
        public AgColocadoresSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
