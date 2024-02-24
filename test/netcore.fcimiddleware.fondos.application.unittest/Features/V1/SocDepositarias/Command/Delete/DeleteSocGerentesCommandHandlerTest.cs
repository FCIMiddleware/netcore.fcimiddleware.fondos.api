using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Command.Delete
{
    public class DeleteSocDepositariasCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<DeleteSocDepositariasCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private DeleteSocDepositariasCommandHandler _handler;

        public DeleteSocDepositariasCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<DeleteSocDepositariasCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new DeleteSocDepositariasCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task DeleteSocDepositariasCommand_InputPais_ReturnsUnit()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var socDepositariaInput = new DeleteSocDepositariasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(socDepositariaInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeleteSocDepositariasCommand_InputPais_ThrowsNotFoundException()
        {
            var socDepositariaInput = new DeleteSocDepositariasCommand { Id = 10000 };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteSocDepositariasCommand_InputPais_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();

            var socDepositariaInput = new DeleteSocDepositariasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }

        [Fact]
        public async Task DeleteSocDepositariasCommand_InputPais_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();

            var socDepositariaInput = new DeleteSocDepositariasCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdSocDepositariasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(socDepositariaInput, _cancellationToken));
        }
    }
}
