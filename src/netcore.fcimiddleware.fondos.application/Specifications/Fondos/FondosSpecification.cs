using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Fondos
{
    public class FondosSpecification : BaseSpecification<Fondo>
    {
        public FondosSpecification(FondosSpecificationParams fondosParam)
            : base(
                  p => 
                  string.IsNullOrEmpty(fondosParam.Search) || p.Descripcion!.Contains(fondosParam.Search)
                )
        {
            ApplyPaging(fondosParam.PageSize * (fondosParam.PageIndex - 1), fondosParam.PageSize);
            if(!string.IsNullOrEmpty(fondosParam.Sort)) 
            {
                switch(fondosParam.Sort)
                {
                    case "descripcionAsc":
                        AddOrderBy(p => p.Descripcion); break;
                    case "descripcionDesc":
                        AddOrderByDescending(p => p.Descripcion); break;
                    case "idCAFCIAsc":
                        AddOrderBy(p => p.IdCAFCI!); break;
                    case "idCAFCIDesc":
                        AddOrderByDescending(p => p.IdCAFCI!); break;
                    default:
                        AddOrderBy(p => p.Descripcion); break;
                }
            }
            else
                AddOrderBy(p => p.Id);
        }
    }
}
