using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasForCountingSpecification : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasForCountingSpecification(SocDepositariasSpecificationParams socDepositariasParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(socDepositariasParams.Search) || x.Descripcion!.Contains(socDepositariasParams.Search)
                  )
        { }
    }
}
