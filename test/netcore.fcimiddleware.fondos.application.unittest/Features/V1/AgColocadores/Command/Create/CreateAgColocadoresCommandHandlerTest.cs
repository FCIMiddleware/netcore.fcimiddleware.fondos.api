using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.AgColocadores.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.AgColocadores.Command.Create
{
    public class CreateAgColocadoresCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateAgColocadoresCommandHandler>> _logger;
        private CancellationToken _cancellationToken;
        private CreateAgColocadoresCommandHandler _handler;

        public CreateAgColocadoresCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateAgColocadoresCommandHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreateAgColocadoresCommandHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateAgColocadoresCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var agColocadoresInput = new CreateAgColocadoresCommand { Descripcion = "Nuevo Fondo" };

            var result = await _handler.Handle(agColocadoresInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateAgColocadoresCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var agColocadoresInput = new CreateAgColocadoresCommand { Descripcion = "Nuevo Fondo", IdCAFCI = "CAFCI_A" };

            var result = await _handler.Handle(agColocadoresInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateAgColocadoresCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.AgColocadores!.FirstOrDefaultAsync();
            var agColocadoresInput = new CreateAgColocadoresCommand { Descripcion = searchEdit!.Descripcion };

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(agColocadoresInput, _cancellationToken));
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.AgColocadores!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";
            var agColocadoresInput = new CreateAgColocadoresCommand { Descripcion = newDescripcion, IdCAFCI = searchEdit!.IdCAFCI };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(agColocadoresInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(AgColocador)}\", ya contiene el valor ({agColocadoresInput.Descripcion})", ex.Message);
        }
    }
}
