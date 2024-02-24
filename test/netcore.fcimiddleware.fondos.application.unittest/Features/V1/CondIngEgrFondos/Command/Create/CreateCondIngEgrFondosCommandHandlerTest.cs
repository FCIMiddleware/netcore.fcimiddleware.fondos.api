using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Exceptions;
using netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.CondIngEgrFondos.Command.Create
{
    public class CreateCondIngEgrFondosCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateCondIngEgrFondosCommandHandler>> _logger;
        private readonly Mock<IMediator> _mediator;
        private CancellationToken _cancellationToken;
        private CreateCondIngEgrFondosCommandHandler _handler;

        public CreateCondIngEgrFondosCommandHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<CreateCondIngEgrFondosCommandHandler>>();
            _mediator = new Mock<IMediator>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new CreateCondIngEgrFondosCommandHandler(_logger.Object, _mapper, _unitOfWork.Object, _mediator.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateCondIngEgrFondosCommand_InputFondo_ReturnsFondo_Without_IdCAFCI()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            
            var condIngEgrFondoInput = new CreateCondIngEgrFondosCommand { Descripcion = "Nuevo Fondo", FondoId = fondo!.Id, TpValorCptFondoId = tpValorCptFondo!.Id };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tpValorCptFondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            var result = await _handler.Handle(condIngEgrFondoInput, _cancellationToken);

            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task CreateCondIngEgrFondosCommand_InputFondo_ThrowsAlreadyExistsException_Descripcion()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.CondIngEgrFondos!.FirstOrDefaultAsync();
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new CreateCondIngEgrFondosCommand { Descripcion = searchEdit!.Descripcion, FondoId = fondo!.Id, TpValorCptFondoId = tpValorCptFondo!.Id };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tpValorCptFondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fondo);

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));

        }

        [Fact]
        public async Task CreateCondIngEgrFondosCommand_InputFondo_ThrowsAlreadyExistsException_FondoId()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new CreateCondIngEgrFondosCommand { Descripcion = "Nuevo Fondo", FondoId = 1000, TpValorCptFondoId = tpValorCptFondo!.Id };

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdTpValorCptFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tpValorCptFondo);

            _mediator
                .Setup(m => m.Send(It.IsAny<GetByIdFondosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(condIngEgrFondoInput, _cancellationToken));
        }

        [Fact]
        public async Task CreateCondIngEgrFondosCommand_InputFondo_ThrowsAlreadyExistsException_TpValorCptFondoId()
        {
            var fondo = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();
            var tpValorCptFondo = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.Where(x => x.IsDeleted == false && x.IsSincronized == false).FirstOrDefaultAsync();

            var condIngEgrFondoInput = new CreateCondIngEgrFondosCommand { Descripcion = "Nuevo Fondo", FondoId = fondo.Id, TpValorCptFondoId = 1000 };
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
