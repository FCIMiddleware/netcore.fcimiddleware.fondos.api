using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.AgColocadores
{
    public class AgColocadoresSpecification : BaseSpecification<AgColocador>
    {
        public AgColocadoresSpecification(AgColocadoresSpecificationParams socAgColocadoresParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(socAgColocadoresParam.Search) || p.Descripcion!.Contains(socAgColocadoresParam.Search)
                )
        {
            ApplyPaging(socAgColocadoresParam.PageSize * (socAgColocadoresParam.PageIndex - 1), socAgColocadoresParam.PageSize);
            if (!string.IsNullOrEmpty(socAgColocadoresParam.Sort))
            {
                switch (socAgColocadoresParam.Sort)
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
