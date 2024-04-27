using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using netcore.fcimiddleware.fondos.application.Features.Shared.Queries;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.GetByDescripcion;
using netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Queries.Vms;
using netcore.fcimiddleware.fondos.application.Mappings;
using netcore.fcimiddleware.fondos.application.unittest.Mocks;
using netcore.fcimiddleware.fondos.infrastructure.Repositories;
using Shouldly;

namespace netcore.fcimiddleware.fondos.application.unittest.Features.V1.SocGerentes.Queries.GetByDescripcion
{
    public class GetByDescripcionSocGerentesQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<GetByDescripcionSocGerentesQueryHandler>> _logger;
        private CancellationToken _cancellationToken;
        private GetByDescripcionSocGerentesQueryHandler _handler;

        public GetByDescripcionSocGerentesQueryHandlerTest()
        {
            _unitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _logger = new Mock<ILogger<GetByDescripcionSocGerentesQueryHandler>>();
            MockDataRepository.AddDataRepository(_unitOfWork.Object);
            _handler = new GetByDescripcionSocGerentesQueryHandler(_logger.Object, _mapper, _unitOfWork.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetByDescripcionSocGerenteTest_Return_Ok()
        {
            var request = new GetByDescripcionSocGerentesQuery
            {
                PageIndex = 1,
                PageSize = 10,
                Search = "",
                Sort = ""
            };

            var result = await _handler.Handle(request, _cancellationToken);

            result.ShouldBeOfType<PaginationVm<SocGerenteListVm>>();
        }

        [Fact]
        public async Task GetAllSocGerentesTest_Return_PageSize_Ok()
        {

            var request = new GetByDescripcionSocGerentesQuery
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
        public async Task GetAllSocGerentesTest_Return_Register_Ok()
        {

            var searchEdit = await _unitOfWork.Object.ApplicationReadDbContext.SocGerentes!.FirstOrDefaultAsync();

            var request = new GetByDescripcionSocGerentesQuery
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
