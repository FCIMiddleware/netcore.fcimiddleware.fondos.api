using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Fondos.Queries.GetById
{
    public class GetByIdFondosQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByIdFondosQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByIdFondosQueryHandler _handler;

        public GetByIdFondosQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByIdFondosQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByIdFondosQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByIdFondoTest_Return_Ok()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Fondos!.FirstOrDefaultAsync();

            var request = new GetByIdFondosQuery
            {
                Id = searchEdit!.Id
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.ShouldBeOfType<Fondo>();
            result.Id.ShouldBe(request.Id);
        }
    }
}
