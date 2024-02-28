using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Paises
{
    public class PaisesSpecification : BaseSpecification<Pais>
    {
        public PaisesSpecification(PaisesSpecificationParams paisesParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(paisesParam.Search) || p.Descripcion!.Contains(paisesParam.Search)
                )
        {
            ApplyPaging(paisesParam.PageSize * (paisesParam.PageIndex - 1), paisesParam.PageSize);
            if (!string.IsNullOrEmpty(paisesParam.Sort))
            {
                switch (paisesParam.Sort)
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
