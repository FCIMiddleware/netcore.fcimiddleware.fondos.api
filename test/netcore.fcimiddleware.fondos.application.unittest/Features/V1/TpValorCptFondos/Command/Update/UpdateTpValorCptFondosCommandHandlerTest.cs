using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.TpValorCptFondos.Command.Update
{
    public class UpdateTpValorCptFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<UpdateTpValorCptFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private UpdateTpValorCptFondosCommandHandler _handler;

        public UpdateTpValorCptFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<UpdateTpValorCptFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new UpdateTpValorCptFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_UpdateFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var edit = "Fondo Editado";

            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = search.Id, Descripcion = edit, FondoId = fondo.Id };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(tpValorCptFondoInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_UpdateFondo_ReturnsFondo_With_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var edit = "Fondo Editado";
            var editCAFIC = "CAFCI_A Editado";

            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = search!.Id, Descripcion = edit, IdCAFCI = editCAFIC, FondoId = fondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(tpValorCptFondoInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_UpdateFondo_ThrowsNotFoundException()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            
            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = 10000, Descripcion = "update", IdCAFCI = "cafciupdate", FondoId = fondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_UpdateFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = searchEdit!.Id, Descripcion = searchReal!.Descripcion, IdCAFCI= "caficunico", FondoId = fondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_UpdateFondo_ThrowsAlreadyExistsException_IdCAFCI()
        {
            var edit = "Fondo Editado";
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = searchEdit!.Id, Descripcion = edit, IdCAFCI = searchReal!.IdCAFCI, FondoId = fondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_InputFondo_ThrowsDeletedException()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = search!.Id, Descripcion = search!.Descripcion, IdCAFCI = search!.IdCAFCI, FondoId = fondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_InputFondo_ThrowsSincronizedException()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = search!.Id, Descripcion = search!.Descripcion, IdCAFCI = search!.IdCAFCI, FondoId = fondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateTpValorCptFondosCommand_InputFondo_ThrowsNotFoundException_FondoId()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            
            var tpValorCptFondoInput = new UpdateTpValorCptFondosCommand { Id = search!.Id, Descripcion = search.Descripcion, IdCAFCI = search.IdCAFCI, FondoId = 10000 };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=>null);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(tpValorCptFondoInput, _cancellationToken));

        }
    }
}
