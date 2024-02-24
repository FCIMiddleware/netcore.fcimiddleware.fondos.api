using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.TpValorCptFondos.Command.Create
{
    public class CreateTpValorCptFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateTpValorCptFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private CreateTpValorCptFondosCommandHandler _handler;

        public CreateTpValorCptFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateTpValorCptFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreateTpValorCptFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateTpValorCptFondosCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondoInput = new CreateTpValorCptFondosCommand { Descripcion = "Nuevo Fondo", FondoId = fondo.Id };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            var result = await _handler.Handle(tpValorCptFondoInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateTpValorCptFondosCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondoInput = new CreateTpValorCptFondosCommand { Descripcion = "Nuevo Fondo", IdCAFCI = "CAFCI_A", FondoId = fondo.Id };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);
            var result = await _handler.Handle(tpValorCptFondoInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateTpValorCptFondosCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.FirstOrDefaultAsync();
            var tpValorCptFondoInput = new CreateTpValorCptFondosCommand { Descripcion = searchEdit!.Descripcion, FondoId = fondo.Id };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(TpValorCptFondo)}\", ya contiene el valor ({tpValorCptFondoInput.Descripcion})", ex.Message);
        }

        [Fact]
        public async Task CreateTpValorCptFondosCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";
            var tpValorCptFondoInput = new CreateTpValorCptFondosCommand { Descripcion = newDescripcion, IdCAFCI = searchEdit!.IdCAFCI, FondoId = fondo.Id };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(TpValorCptFondo)}\", ya contiene el valor ({tpValorCptFondoInput.Descripcion})", ex.Message);
        }

        [Fact]
        public async Task CreateTpValorCptFondosCommand_InputFondo_ThrowsNotFoundException_FondoId()
        {
            var tpValorCptFondoInput = new CreateTpValorCptFondosCommand { Descripcion = "Fondo Editado", IdCAFCI = "CAFCIINTERFAZ", FondoId = 10000 };

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));

            Assert.Equal($"Entity \"{nameof(Fondo)}\" ({tpValorCptFondoInput.FondoId})  no fue encontrado", ex.Message);
        }
    }
}
