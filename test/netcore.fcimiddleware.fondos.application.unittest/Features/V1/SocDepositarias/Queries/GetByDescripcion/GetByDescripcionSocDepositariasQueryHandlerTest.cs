using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.GetByDescripcion;
using netcore.fcimiddleware.fondos.application.Features.V1.SocDepositarias.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocDepositarias.Queries.GetByDescripcion
{
    public class GetByDescripcionSocDepositariasQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByDescripcionSocDepositariasQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByDescripcionSocDepositariasQueryHandler _handler;

        public GetByDescripcionSocDepositariasQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByDescripcionSocDepositariasQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByDescripcionSocDepositariasQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByDescripcionSocDepositariaTest_Return_Ok()
        {
            var request = new GetByDescripcionSocDepositariasQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = "",
                Sort = ""
            };

            var result = await _handler.Handle(request, _cancellationToken);

            result.ShouldBeOfType<PaginationVm<SocDepositariaListVm>>();
        }

        [Fact]
        public async Task GetAllSocDepositariasTest_Return_PageSize_Ok()
        {

            var request = new GetByDescripcionSocDepositariasQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = "",
                Sort = ""
            };

            var result = await _handler.Handle(request, _cancellationToken);

            result.PageSize.ShouldBe(10);
        }

        [Fact]
        public async Task GetAllSocDepositariasTest_Return_Register_Ok()
        {

            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocDepositarias!.FirstOrDefaultAsync();

            var request = new GetByDescripcionSocDepositariasQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = searchEdit!.Descripcion,
                Sort = ""
            };

            var result = await _handler.Handle(request, _cancellationToken);

            result.Count.ShouldBe(1);
        }
    }
}
