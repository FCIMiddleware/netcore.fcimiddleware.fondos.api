using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocDepositarias
{
    public class SocDepositariasSpecification : BaseSpecification<SocDepositaria>
    {
        public SocDepositariasSpecification(SocDepositariasSpecificationParams socDepositariasParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(socDepositariasParam.Search) || p.Descripcion!.ToUpper().Contains(socDepositariasParam.Search.ToUpper())
                )
        {
            ApplyPaging(socDepositariasParam.PageSize * (socDepositariasParam.PageIndex - 1), socDepositariasParam.PageSize);
            if (!string.IsNullOrEmpty(socDepositariasParam.Sort))
            {
                switch (socDepositariasParam.Sort)
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
