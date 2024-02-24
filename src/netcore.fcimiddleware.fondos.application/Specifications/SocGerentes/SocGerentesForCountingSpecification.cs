using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocGerentes
{
    public class SocGerentesForCountingSpecification : BaseSpecification<SocGerente>
    {
        public SocGerentesForCountingSpecification(SocGerentesSpecificationParams socGerentesParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(socGerentesParams.Search) || x.Descripcion!.Contains(socGerentesParams.Search)
                  )
        { }
    }
}
