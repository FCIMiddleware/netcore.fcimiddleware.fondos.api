using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasSpecificationId : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
