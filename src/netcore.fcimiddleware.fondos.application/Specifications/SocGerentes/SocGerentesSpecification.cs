using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Specifications.SocGerentes
{
    public class SocGerentesSpecification : BaseSpecification<SocGerente>
    {
        public SocGerentesSpecification(SocGerentesSpecificationParams socGerentesParam)
            : base(
                  p =>
                  string.IsNullOrEmpty(socGerentesParam.Search) || p.Descripcion!.Contains(socGerentesParam.Search)
                )
        {
            ApplyPaging(socGerentesParam.PageSize * (socGerentesParam.PageIndex - 1), socGerentesParam.PageSize);
            if (!string.IsNullOrEmpty(socGerentesParam.Sort))
            {
                switch (socGerentesParam.Sort)
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
