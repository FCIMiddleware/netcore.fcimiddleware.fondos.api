using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Fondos
{
    public class FondosForCountingSpecification : BaseSpecification<Fondo>
    {
        public FondosForCountingSpecification(FondosSpecificationParams fondosParams) 
            : base (
                  x => 
                  string.IsNullOrEmpty(fondosParams.Search) || x.Descripcion!.ToUpper().Contains(fondosParams.Search.ToUpper())
                  )
        {
            AddInclude(p => p.Monedas);
            AddInclude(p => p.SocGerentes);
            AddInclude(p => p.SocDepositarias);
            AddInclude(p => p.Paises);
        }
    }
}
