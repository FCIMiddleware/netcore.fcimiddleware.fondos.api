using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.AgColocadores
{
    public class AgColocadoresForCountingSpecification : BaseSpecification<AgColocador>
    {
        public AgColocadoresForCountingSpecification(AgColocadoresSpecificationParams agColocadoresParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(agColocadoresParams.Search) || x.Descripcion!.Contains(agColocadoresParams.Search)
                  )
        { }
    }
}
