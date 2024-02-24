using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasSpecificationDescripcion : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasSpecificationDescripcion(string descripcion, int id = -1)
            : base(p => p.Descripcion!.ToUpper().Equals(descripcion) && (p.Id! != id))
        {
        }
    }
}
