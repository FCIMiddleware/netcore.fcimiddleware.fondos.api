using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.TpValorCptFondos
{
    public class TpValorCptFondosSpecification : BaseSpecification<TpValorCptFondo>
    {
        public TpValorCptFondosSpecification(TpValorCptFondosSpecificationParams tpValorCptFondoParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(tpValorCptFondoParam.Search) || p.Descripcion!.Contains(tpValorCptFondoParam.Search)
                )
        {
            ApplyPaging(tpValorCptFondoParam.PageSize * (tpValorCptFondoParam.PageIndex - 1), tpValorCptFondoParam.PageSize);
            if (!string.IsNullOrEmpty(tpValorCptFondoParam.Sort))
            {
                switch (tpValorCptFondoParam.Sort)
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
        }
    }
}
