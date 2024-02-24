using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Monedas.Queries.GetById
{
    public class GetByIdMonedasQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByIdMonedasQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByIdMonedasQueryHandler _handler;

        public GetByIdMonedasQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByIdMonedasQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByIdMonedasQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByIdMonedaTest_Return_Ok()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.FirstOrDefaultAsync();

            var request = new GetByIdMonedasQuery
            {
                Id = searchEdit!.Id
            };

            var result = await _handler.Handle(request, CancellationToken.None);
            
            result.ShouldBeOfType<Moneda>();
            result.Id.ShouldBe(request.Id);
        }
    }
}
