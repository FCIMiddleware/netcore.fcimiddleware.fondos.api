using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocGerentes
{
    public class SocGerentesSpecificationId : BaseSpecification<SocGerente>
    {
        public SocGerentesSpecificationId(int id)
            : base(p => p.Id == id)
        {
        }
    }
}
