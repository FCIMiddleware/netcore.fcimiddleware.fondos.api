using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Command.Update
{
    public class UpdateSocDepositariasCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<UpdateSocDepositariasCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private UpdateSocDepositariasCommandHandler _handler;

        public UpdateSocDepositariasCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<UpdateSocDepositariasCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new UpdateSocDepositariasCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_Returns_Without_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var edit = "Fondo Editado";

            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = search.Id, Descripcion = edit };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(socDepositariaInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_Returns_With_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var edit = "Fondo Editado";
            var editCAFIC = "CAFCI_A Editado";

            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = search!.Id, Descripcion = edit, IdCAFCI = editCAFIC };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);
            var result = await _handler.Handle(socDepositariaInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_ThrowsNotFoundException()
        {
            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = 10000, Descripcion = "update" };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = searchEdit!.Id, Descripcion = searchReal!.Descripcion };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var edit = "Fondo Editado";
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = searchEdit!.Id, Descripcion = edit, IdCAFCI = searchReal!.IdCAFCI };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateSocDepositariasCommand_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var socDepositariaInput = new UpdateSocDepositariasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }
    }
}
