using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasSpecificationCAFCI : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasSpecificationCAFCI(string idCAFCI)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI.ToUpper()))
        {
        }
    }
}
