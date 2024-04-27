using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasSpecificationDescripcion : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasSpecificationDescripcion(string descripcion)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion.ToUpper()))
        {
        }
    }
}
