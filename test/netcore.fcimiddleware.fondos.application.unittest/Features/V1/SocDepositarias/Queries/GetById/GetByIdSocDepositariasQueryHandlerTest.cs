using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Queries.GetById
{
    public class GetByIdSocDepositariasQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByIdSocDepositariasQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByIdSocDepositariasQueryHandler _handler;

        public GetByIdSocDepositariasQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByIdSocDepositariasQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByIdSocDepositariasQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByIdSocDepositariasTest_Return_Ok()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.FirstOrDefaultAsync();

            var request = new GetByIdSocDepositariasQuery
            {
                Id = searchEdit!.Id
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.ShouldBeOfType<SocDepositaria>();
            result.Id.ShouldBe(request.Id);
        }
    }
}
