using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.CondIngEgrFondos.Command.Delete
{
    public class DeleteCondIngEgrFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<DeleteCondIngEgrFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private DeleteCondIngEgrFondosCommandHandler _handler;

        public DeleteCondIngEgrFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<DeleteCondIngEgrFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new DeleteCondIngEgrFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task DeleteCondIngEgrFondosCommand_InputFondo_ReturnsUnit()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondosInput = new DeleteCondIngEgrFondosCommand { Id = search!.Id };
            
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(condIngEgrFondosInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeleteCondIngEgrFondosCommand_InputFondo_ThrowsNotFoundException()
        {
            var condIngEgrFondosInput = new DeleteCondIngEgrFondosCommand { Id = 10000 };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(condIngEgrFondosInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteCondIngEgrFondosCommand_InputFondo_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondosInput = new DeleteCondIngEgrFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(condIngEgrFondosInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteCondIngEgrFondosCommand_InputFondo_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var condIngEgrFondosInput = new DeleteCondIngEgrFondosCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdCondIngEgrFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(condIngEgrFondosInput, _cancellationToken));
        }
    }
}
