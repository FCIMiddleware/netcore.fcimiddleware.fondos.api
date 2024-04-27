using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.GetByDescripcion;
using netcore.fcimiddleware.fondos.application.Features.V1.Monedas.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.Monedas.Queries.GetByDescripcion
{
    public class GetByDescripcionMonedasQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByDescripcionMonedasQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByDescripcionMonedasQueryHandler _handler;

        public GetByDescripcionMonedasQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByDescripcionMonedasQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByDescripcionMonedasQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByDescripcionMonedaTest_Return_Ok()
        {
            var request = new GetByDescripcionMonedasQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = "",
                Sort = ""
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.ShouldBeOfType<PaginationVm<MonedaListVm>>();
        }

        [Fact]
        public async Task GetAllMonedasTest_Return_PageSize_Ok()
        {

            var request = new GetByDescripcionMonedasQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = "",
                Sort = ""
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.PageSize.ShouldBe(10);
        }

        [Fact]
        public async Task GetAllMonedasTest_Return_Register_Ok()
        {

            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.Monedas!.FirstOrDefaultAsync();

            var request = new GetByDescripcionMonedasQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = searchEdit!.Descripcion,
                Sort = ""
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Count.ShouldBe(1);
        }
    }
}
