using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Paises.Command.Create
{
    public class CreatePaisesCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreatePaisesCommandHandler>> _logger;
        private CancellationToken _cancellationToken;
        private CreatePaisesCommandHandler _handler;

        public CreatePaisesCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreatePaisesCommandHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreatePaisesCommandHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var paisInput = new CreatePaisesCommand { Descripcion = "Nuevo Fondo" };

            var result = await _handler.Handle(paisInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var paisInput = new CreatePaisesCommand { Descripcion = "Nuevo Fondo", IdCAFCI = "CAFCI_A" };

            var result = await _handler.Handle(paisInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Paises!.FirstOrDefaultAsync();
            var paisInput = new CreatePaisesCommand { Descripcion = searchEdit!.Descripcion };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(paisInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(Pais)}\", ya contiene el valor ({paisInput.Descripcion})", ex.Message);
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Paises!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";
            var paisInput = new CreatePaisesCommand { Descripcion = newDescripcion, IdCAFCI = searchEdit!.IdCAFCI };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(paisInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(Pais)}\", ya contiene el valor ({paisInput.Descripcion})", ex.Message);
        }
    }
}
