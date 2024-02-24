using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocGerentes
{
    public class SocGerentesSpecificationCAFCI : BaseSpecification<SocGerente>
    {
        public SocGerentesSpecificationCAFCI(string idCAFCI, int id = -1)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI) && (p.Id! != id))
        {
        }
    }
}
