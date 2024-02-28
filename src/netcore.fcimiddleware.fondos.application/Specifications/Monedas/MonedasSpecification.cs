using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.Monedas
{
    public class MonedasSpecification : BaseSpecification<Moneda>
    {
        public MonedasSpecification(MonedasSpecificationParams monedasParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(monedasParam.Search) || p.Descripcion!.Contains(monedasParam.Search)
                )
        {
            ApplyPaging(monedasParam.PageSize * (monedasParam.PageIndex - 1), monedasParam.PageSize);
            if (!string.IsNullOrEmpty(monedasParam.Sort))
            {
                switch (monedasParam.Sort)
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
