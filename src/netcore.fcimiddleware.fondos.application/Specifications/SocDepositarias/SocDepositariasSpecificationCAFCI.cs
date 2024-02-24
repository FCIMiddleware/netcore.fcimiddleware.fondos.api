using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasSpecificationCAFCI : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasSpecificationCAFCI(string idCAFCI, int id = -1)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI) && (p.Id! != id))
        {
        }
    }
}
