using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.CondIngEgrFondos
{
    public class CondIngEgrFondosSpecification : BaseSpecification<CondIngEgrFondo>
    {
        public CondIngEgrFondosSpecification(CondIngEgrFondosSpecificationParams fondosParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(fondosParam.Search) || p.Descripcion!.Contains(fondosParam.Search)
                )
        {
            ApplyPaging(fondosParam.PageSize * (fondosParam.PageIndex - 1), fondosParam.PageSize);
            if (!string.IsNullOrEmpty(fondosParam.Sort))
            {
                switch (fondosParam.Sort)
                {
                    case "descripcionAsc":
                        AddOrderBy(p => p.Descripcion); break;
                    case "descripcionDesc":
                        AddOrderByDescending(p => p.Descripcion); break;
                    default:
                        AddOrderBy(p => p.Descripcion); break;
                }
            }
            else
                AddOrderBy(p => p.Id);
        }
    }
}
