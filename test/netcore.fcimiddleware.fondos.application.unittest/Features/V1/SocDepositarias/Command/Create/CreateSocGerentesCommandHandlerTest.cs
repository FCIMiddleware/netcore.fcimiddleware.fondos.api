using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Command.Create
{
    public class CreateSocDepositariasCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateSocDepositariasCommandHandler>> _logger;
        private CancellationToken _cancellationToken;
        private CreateSocDepositariasCommandHandler _handler;

        public CreateSocDepositariasCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateSocDepositariasCommandHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreateSocDepositariasCommandHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateSocDepositariasCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var socDepositariaInput = new CreateSocDepositariasCommand { Descripcion = "Nuevo Fondo" };

            var result = await _handler.Handle(socDepositariaInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateSocDepositariasCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var socDepositariaInput = new CreateSocDepositariasCommand { Descripcion = "Nuevo Fondo", IdCAFCI = "CAFCI_A" };

            var result = await _handler.Handle(socDepositariaInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateSocDepositariasCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.FirstOrDefaultAsync();
            var socDepositariaInput = new CreateSocDepositariasCommand { Descripcion = searchEdit!.Descripcion };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(SocDepositaria)}\", ya contiene el valor ({socDepositariaInput.Descripcion})", ex.Message);
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";
            var socDepositariaInput = new CreateSocDepositariasCommand { Descripcion = newDescripcion, IdCAFCI = searchEdit!.IdCAFCI };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(SocDepositaria)}\", ya contiene el valor ({socDepositariaInput.Descripcion})", ex.Message);
        }
    }
}
