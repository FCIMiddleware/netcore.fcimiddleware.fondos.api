using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Fondos.Command.Create
{
    public class CreateFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<FondosCommandHandler>> _logger;
        private CancellationToken _cancellationToken;
        private FondosCommandHandler _handler;
        private Moneda _moneda;
        private Pais _pais;
        private SocGerente _socGerente;
        private SocDepositaria _socDepositaria;

        public CreateFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<FondosCommandHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new FondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
            _moneda = _unitOfWork.Object.ApplicationReadDbContext.Monedas!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
            _pais = _unitOfWork.Object.ApplicationReadDbContext.Paises!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
            _socGerente = _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
            _socDepositaria = _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            
            var fondoInput = new CreateFondosCommand { 
                Descripcion = "Nuevo Fondo",
                MonedaId = _moneda.Id, 
                PaisId = _pais.Id, 
                SocDepositariaId = _socDepositaria.Id, 
                SocGerenteId = _socGerente.Id 
            };

            var result = await _handler.Handle(fondoInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var fondoInput = new CreateFondosCommand { 
                Descripcion = "Nuevo Fondo", 
                IdCAFCI = "CAFCI_A",
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };

            var result = await _handler.Handle(fondoInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.FirstOrDefaultAsync();
            var fondoInput = new CreateFondosCommand { 
                Descripcion = searchEdit!.Descripcion,
                IdCAFCI = null,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";

            var fondoInput = new CreateFondosCommand { 
                Descripcion = newDescripcion, 
                IdCAFCI = searchEdit!.IdCAFCI,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ThrowsNotFoundException_Moneda()
        {
            var newDescripcion = "Fondo_ThrowsNotFoundException_Moneda";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_Moneda";

            var fondoInput = new CreateFondosCommand
            {
                Descripcion = newDescripcion,
                IdCAFCI = newCAFIC,
                MonedaId = 100000,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ThrowsNotFoundException_Pais()
        {
            var newDescripcion = "Fondo_ThrowsNotFoundException_Pais";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_Pais";

            var fondoInput = new CreateFondosCommand
            {
                Descripcion = newDescripcion,
                IdCAFCI = newCAFIC,
                MonedaId = _moneda.Id,
                PaisId = 100000,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ThrowsNotFoundException_SocGerente()
        {
            var newDescripcion = "Fondo_ThrowsNotFoundException_SocGerente";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_SocGerente";

            var fondoInput = new CreateFondosCommand
            {
                Descripcion = newDescripcion,
                IdCAFCI = newCAFIC,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = 100000
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task CreateFondosCommand_InputFondo_ThrowsNotFoundException_SocDepositaria()
        {
            var newDescripcion = "Fondo_ThrowsNotFoundException_SocDepositaria";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_SocDepositaria";

            var fondoInput = new CreateFondosCommand
            {
                Descripcion = newDescripcion,
                IdCAFCI = newCAFIC,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = 100000,
                SocGerenteId = _socGerente.Id
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }
    }
}
