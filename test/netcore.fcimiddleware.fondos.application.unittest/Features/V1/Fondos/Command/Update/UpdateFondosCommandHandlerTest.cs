using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Fondos.Command.Update
{
    public class UpdateFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<UpdateFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private UpdateFondosCommandHandler _handler;
        private Moneda _moneda;
        private Pais _pais;
        private SocGerente _socGerente;
        private SocDepositaria _socDepositaria;

        public UpdateFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<UpdateFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new UpdateFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
            _moneda = _unitOfWork.Object.ApplicationReadDbContext.Monedas!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
            _pais = _unitOfWork.Object.ApplicationReadDbContext.Paises!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
            _socGerente = _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
            _socDepositaria = _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefault();
        }

        [Fact]
        public async Task UpdateFondosCommand_UpdateFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var edit = "Fondo Editado";

            var fondoInput = new UpdateFondosCommand { 
                Id = search.Id, 
                Descripcion = edit,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(fondoInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateFondosCommand_UpdateFondo_ReturnsFondo_With_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var edit = "Fondo Editado";
            var editCAFIC = "CAFCI_A Editado";
            
            var fondoInput = new UpdateFondosCommand { 
                Id = search!.Id, 
                Descripcion = edit, 
                IdCAFCI = editCAFIC,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);
            var result = await _handler.Handle(fondoInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateFondosCommand_UpdateFondo_ThrowsNotFoundException()
        {            
            var fondoInput = new UpdateFondosCommand { 
                Id = 10000, 
                Descripcion = "update",
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateFondosCommand_UpdateFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var fondoInput = new UpdateFondosCommand { 
                Id = searchEdit!.Id, 
                Descripcion = searchReal!.Descripcion,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateFondosCommand_UpdateFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var edit = "Fondo Editado";
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var fondoInput = new UpdateFondosCommand { 
                Id = searchEdit!.Id, 
                Descripcion = edit, 
                IdCAFCI = searchReal!.IdCAFCI,
                MonedaId = _moneda.Id,
                PaisId = _pais.Id,
                SocDepositariaId = _socDepositaria.Id,
                SocGerenteId = _socGerente.Id
            };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateFondosCommand_InputFondo_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var fondoInput = new UpdateFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateFondosCommand_InputFondo_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var fondoInput = new UpdateFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(fondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateFondosCommand_InputFondo_ThrowsNotFoundException_Moneda()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var newDescripcion = "Fondo_ThrowsNotFoundException_Moneda";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_Moneda";

            var fondoInput = new UpdateFondosCommand
            {
                Id = search!.Id,
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
        public async Task UpdateFondosCommand_InputFondo_ThrowsNotFoundException_Pais()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var newDescripcion = "Fondo_ThrowsNotFoundException_Pais";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_Pais";

            var fondoInput = new UpdateFondosCommand
            {
                Id = search!.Id,
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
        public async Task UpdateFondosCommand_InputFondo_ThrowsNotFoundException_SocGerente()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var newDescripcion = "Fondo_ThrowsNotFoundException_SocGerente";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_SocGerente";

            var fondoInput = new UpdateFondosCommand
            {
                Id = search!.Id,
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
        public async Task UpdateFondosCommand_InputFondo_ThrowsNotFoundException_SocDepositaria()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var newDescripcion = "Fondo_ThrowsNotFoundException_SocDepositaria";
            var newCAFIC = "CAFCI_ThrowsNotFoundException_SocDepositaria";

            var fondoInput = new UpdateFondosCommand
            {
                Id = search!.Id,
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
