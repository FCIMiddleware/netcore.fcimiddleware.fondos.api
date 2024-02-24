using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.V1.Paises.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Paises.Queries.GetById
{
    public class GetByIdPaisesQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByIdPaisesQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByIdPaisesQueryHandler _handler;

        public GetByIdPaisesQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByIdPaisesQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByIdPaisesQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByIdPaisTest_Return_Ok()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Paises!.FirstOrDefaultAsync();

            var request = new GetByIdPaisesQuery
            {
                Id = searchEdit!.Id
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.ShouldBeOfType<Pais>();
            result.Id.ShouldBe(request.Id);
        }
    }
}
