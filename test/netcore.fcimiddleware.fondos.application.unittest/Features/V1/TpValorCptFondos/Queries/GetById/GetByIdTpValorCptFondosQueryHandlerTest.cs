using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Queries.GetById;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.domain;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.TpValorCptFondos.Queries.GetById
{
    public class GetByIdTpValorCptFondosQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByIdTpValorCptFondosQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByIdTpValorCptFondosQueryHandler _handler;

        public GetByIdTpValorCptFondosQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByIdTpValorCptFondosQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByIdTpValorCptFondosQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByIdFondoTest_Return_Ok()
        {
            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.TpValorCptFondos!.FirstOrDefaultAsync();

            var request = new GetByIdTpValorCptFondosQuery
            {
                Id = searchEdit!.Id
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.ShouldBeOfType<TpValorCptFondo>();
            result.Id.ShouldBe(request.Id);
        }
    }
}
