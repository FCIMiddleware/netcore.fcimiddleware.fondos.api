using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Monedas.Command.Create
{
    public class CreateMonedasCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateMonedasCommandHandler>> _logger;
        private CancellationToken _cancellationToken;
        private CreateMonedasCommandHandler _handler;

        public CreateMonedasCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateMonedasCommandHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreateMonedasCommandHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateMonedasCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var paisInput = new CreateMonedasCommand { Descripcion = "Nuevo Fondo" };

            var result = await _handler.Handle(paisInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateMonedasCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var paisInput = new CreateMonedasCommand { Descripcion = "Nuevo Fondo", IdCAFCI = "CAFCI_A" };

            var result = await _handler.Handle(paisInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateMonedasCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.FirstOrDefaultAsync();
            var paisInput = new CreateMonedasCommand { Descripcion = searchEdit!.Descripcion };

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(paisInput, _cancellationToken));;
        }

        [Fact]
        public async Task CreateMonedasCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";
            var paisInput = new CreateMonedasCommand { Descripcion = newDescripcion, IdCAFCI = searchEdit!.IdCAFCI };

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(paisInput, _cancellationToken));
        }
    }
}
