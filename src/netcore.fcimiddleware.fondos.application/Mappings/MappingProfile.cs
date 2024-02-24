using AutoMapper;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.Vms;
using netcore.fcimiddleware.fondos.domain;

namespace netcore.fcimiddleware.fondos.application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateFondosCommand, Fondo>();
            CreateMap<UpdateFondosCommand, Fondo>();
            CreateMap<Fondo, FondoVm>();

            CreateMap<CreateTpValorCptFondosCommand, TpValorCptFondo>();
            CreateMap<UpdateTpValorCptFondosCommand, TpValorCptFondo>();
            CreateMap<TpValorCptFondo, TpValorCptFondoVm>();

            CreateMap<CreateCondIngEgrFondosCommand, CondIngEgrFondo>();
            CreateMap<UpdateCondIngEgrFondosCommand, CondIngEgrFondo>();
            CreateMap<CondIngEgrFondo, CondIngEgrFondoVm>();

            CreateMap<CreatePaisesCommand, Pais>();
            CreateMap<UpdatePaisesCommand, Pais>();
            CreateMap<Pais, PaisVm>();

            CreateMap<CreateMonedasCommand, Moneda>();
            CreateMap<UpdateMonedasCommand, Moneda>();
            CreateMap<Moneda, MonedaVm>();

            CreateMap<CreateSocGerentesCommand, SocGerente>();
            CreateMap<UpdateSocGerentesCommand, SocGerente>();
            CreateMap<SocGerente, SocGerenteVm>();

            CreateMap<CreateSocDepositariasCommand, SocDepositaria>();
            CreateMap<UpdateSocDepositariasCommand, SocDepositaria>();
            CreateMap<SocDepositaria, SocDepositariaVm>();

            CreateMap<CreateAgColocadoresCommand, AgColocador>();
            CreateMap<UpdateAgColocadoresCommand, AgColocador>();
            CreateMap<AgColocador, AgColocadorVm>();
        }
    }
}
