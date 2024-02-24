using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Create;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocGerentes.Command.Create
{
    public class CreateSocGerentesCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateSocGerentesCommandHandler>> _logger;
        private CancellationToken _cancellationToken;
        private CreateSocGerentesCommandHandler _handler;

        public CreateSocGerentesCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateSocGerentesCommandHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreateSocGerentesCommandHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateSocGerentesCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var socGerenteInput = new CreateSocGerentesCommand { Descripcion = "Nuevo Fondo" };

            var result = await _handler.Handle(socGerenteInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateSocGerentesCommand_InputFondo_ReturnsFondo_With_IdCAFCI()
        {
            var socGerenteInput = new CreateSocGerentesCommand { Descripcion = "Nuevo Fondo", IdCAFCI = "CAFCI_A" };

            var result = await _handler.Handle(socGerenteInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateSocGerentesCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.FirstOrDefaultAsync();
            var socGerenteInput = new CreateSocGerentesCommand { Descripcion = searchEdit!.Descripcion };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socGerenteInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(SocGerente)}\", ya contiene el valor ({socGerenteInput.Descripcion})", ex.Message);
        }

        [Fact]
        public async Task CreatePaisesCommand_InputFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.FirstOrDefaultAsync();
            var newDescripcion = "Fondo Editado";
            var socGerenteInput = new CreateSocGerentesCommand { Descripcion = newDescripcion, IdCAFCI = searchEdit!.IdCAFCI };

            var ex = await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socGerenteInput, _cancellationToken));

            Assert.Equal($"Entidad \"{nameof(SocGerente)}\", ya contiene el valor ({socGerenteInput.Descripcion})", ex.Message);
        }
    }
}
