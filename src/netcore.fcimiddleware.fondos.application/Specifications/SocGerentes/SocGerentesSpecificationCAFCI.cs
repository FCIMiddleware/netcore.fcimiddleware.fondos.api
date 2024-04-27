using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocGerentes
{
    public class SocGerentesSpecificationCAFCI : BaseSpecification<SocGerente>
    {
        public SocGerentesSpecificationCAFCI(string idCAFCI)
            : base(p => p.IdCAFCI!.ToUpper().Equals(idCAFCI.ToUpper()))
        {
        }
    }
}
