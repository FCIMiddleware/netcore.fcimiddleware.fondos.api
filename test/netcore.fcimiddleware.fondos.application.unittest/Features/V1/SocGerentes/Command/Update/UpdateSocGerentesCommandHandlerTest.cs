using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocGerentes.Command.Update
{
    public class UpdateSocGerentesCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<UpdateSocGerentesCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private UpdateSocGerentesCommandHandler _handler;

        public UpdateSocGerentesCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<UpdateSocGerentesCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new UpdateSocGerentesCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_Returns_Without_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var edit = "Fondo Editado";

            var socGerenteInput = new UpdateSocGerentesCommand { Id = search.Id, Descripcion = edit };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocGerentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(socGerenteInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_Returns_With_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var edit = "Fondo Editado";
            var editCAFIC = "CAFCI_A Editado";

            var socGerenteInput = new UpdateSocGerentesCommand { Id = search!.Id, Descripcion = edit, IdCAFCI = editCAFIC };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocGerentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);
            var result = await _handler.Handle(socGerenteInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_ThrowsNotFoundException()
        {
            var socGerenteInput = new UpdateSocGerentesCommand { Id = 10000, Descripcion = "update" };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(socGerenteInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var socGerenteInput = new UpdateSocGerentesCommand { Id = searchEdit!.Id, Descripcion = searchReal!.Descripcion };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocGerentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socGerenteInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var edit = "Fondo Editado";
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var socGerenteInput = new UpdateSocGerentesCommand { Id = searchEdit!.Id, Descripcion = edit, IdCAFCI = searchReal!.IdCAFCI };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocGerentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socGerenteInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var socGerenteInput = new UpdateSocGerentesCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocGerentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(socGerenteInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocGerentesCommand_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var socGerenteInput = new UpdateSocGerentesCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocGerentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(socGerenteInput, _cancellationToken));
        }
    }
}
