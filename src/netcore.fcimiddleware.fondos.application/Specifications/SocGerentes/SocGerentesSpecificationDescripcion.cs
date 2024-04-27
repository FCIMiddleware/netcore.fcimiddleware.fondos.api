using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocGerentes
{
    public class SocGerentesSpecificationDescripcion : BaseSpecification<SocGerente>
    {
        public SocGerentesSpecificationDescripcion(string descripcion)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion.ToUpper()))
        {
        }
    }
}
