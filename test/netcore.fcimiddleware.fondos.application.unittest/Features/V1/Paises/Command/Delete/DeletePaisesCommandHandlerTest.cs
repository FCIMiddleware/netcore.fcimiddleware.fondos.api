using AutoMapper;
using Castle.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Commands.Delete;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Paises.Command.Delete
{
    public class DeletePaisesCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<DeletePaisesCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private DeletePaisesCommandHandler _handler;

        public DeletePaisesCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<DeletePaisesCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new DeletePaisesCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task DeletePaisesCommand_InputPais_ReturnsUnit()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Paises!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var paisInput = new DeletePaisesCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdPaisesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            var result = await _handler.Handle(paisInput, _cancellationToken);

            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeletePaisesCommand_InputPais_ThrowsNotFoundException()
        {
            var paisInput = new DeletePaisesCommand { Id = 10000 };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(paisInput, _cancellationToken));
        }

        [Fact]
        public async Task DeletePaisesCommand_InputPais_ThrowsDeletedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Paises!.Where(x => x.IsDeleted == true && x.IsSincronized == false).FirstOrDefaultAsync();
            var paisInput = new DeletePaisesCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdPaisesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<DeletedException>(() => _handler.Handle(paisInput, _cancellationToken));
        }

        [Fact]
        public async Task DeletePaisesCommand_InputPais_ThrowsSincronizedException()
        {
            var search = await _unitOfWork.Object.ApplicationReadDbContext.Paises!.Where(x => x.IsDeleted == false && x.IsSincronized == true).FirstOrDefaultAsync();
            var paisInput = new DeletePaisesCommand { Id = search!.Id };
            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdPaisesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(search);

            await Assert.ThrowsAsync<SincronizedException>(() => _handler.Handle(paisInput, _cancellationToken));
        }
    }
}
