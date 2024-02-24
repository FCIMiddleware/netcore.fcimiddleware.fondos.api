using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Update;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.CondIngEgrFondos.Command.Update
{
    public class UpdateCondIngEgrFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<UpdateCondIngEgrFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private UpdateCondIngEgrFondosCommandHandler _handler;

        public UpdateCondIngEgrFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<UpdateCondIngEgrFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new UpdateCondIngEgrFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_UpdateFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var edit = "Fondo Editado";

            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = search.Id, Descripcion = edit, FondoId = fondo.Id, TpValorCptFondoId = tpValorCptFondo.Id };
            
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tpValorCptFondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            var result = await _handler.Handle(condIngEgrFondoInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_UpdateFondo_ThrowsNotFoundException()
        {
            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = 10000, Descripcion = "update" };
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_UpdateFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var searchReal = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.Id != searchEdit!.Id && x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            
            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = searchEdit!.Id, Descripcion = searchReal!.Descripcion };            
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchEdit);

            //_mediator
            //    .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(tpValorCptFondo);

            //_mediator
            //    .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(fondo);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_InputFondo_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_InputFondo_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_InputFondo_ThrowsAlreadyExistsException_FondoId()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = search.Id, Descripcion = search.Descripcion, FondoId = 1000, TpValorCptFondoId = tpValorCptFondo.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tpValorCptFondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(()=>null);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task UpdateCondIngEgrFondosCommand_InputFondo_ThrowsAlreadyExistsException_TpValorCptFondoId()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new UpdateCondIngEgrFondosCommand { Id = search.Id, Descripcion = search.Descripcion, FondoId = fondo.Id, TpValorCptFondoId = 1000 };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }
    }
}
