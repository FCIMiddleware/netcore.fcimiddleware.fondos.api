using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Monedas.Command.Delete
{
    public class DeleteMonedasCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<DeleteMonedasCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private DeleteMonedasCommandHandler _handler;

        public DeleteMonedasCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<DeleteMonedasCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new DeleteMonedasCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task DeleteMonedasCommand_InputMoneda_ReturnsUnit()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var paisInput = new DeleteMonedasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdMonedasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(paisInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeleteMonedasCommand_InputMoneda_ThrowsNotFoundException()
        {
            var paisInput = new DeleteMonedasCommand { Id = 10000 };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(paisInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteMonedasCommand_InputMoneda_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var paisInput = new DeleteMonedasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdMonedasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(paisInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteMonedasCommand_InputMoneda_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var paisInput = new DeleteMonedasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdMonedasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(paisInput, _cancellationToken));
        }
    }
}
