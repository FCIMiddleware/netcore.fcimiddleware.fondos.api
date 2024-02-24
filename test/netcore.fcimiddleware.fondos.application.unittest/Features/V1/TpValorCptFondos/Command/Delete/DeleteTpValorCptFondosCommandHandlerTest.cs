using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.TpValorCptFondos.Command.Delete
{
    public class DeleteTpValorCptFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<DeleteTpValorCptFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private DeleteTpValorCptFondosCommandHandler _handler;

        public DeleteTpValorCptFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<DeleteTpValorCptFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new DeleteTpValorCptFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task DeleteTpValorCptFondosCommand_InputFondo_ReturnsUnit()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var tpvalorcptFondosInput = new DeleteTpValorCptFondosCommand { Id = search!.Id };
            
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(tpvalorcptFondosInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeleteTpValorCptFondosCommand_InputFondo_ThrowsNotFoundException()
        {
            var tpvalorcptFondosInput = new DeleteTpValorCptFondosCommand { Id = 10000 };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(tpvalorcptFondosInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteTpValorCptFondosCommand_InputFondo_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var tpvalorcptFondosInput = new DeleteTpValorCptFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(tpvalorcptFondosInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteTpValorCptFondosCommand_InputFondo_ThrowsSincronizedException()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var search = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var tpvalorcptFondosInput = new DeleteTpValorCptFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(tpvalorcptFondosInput, _cancellationToken));
        }
    }
}
