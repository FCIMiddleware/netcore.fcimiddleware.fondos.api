using Microsoft.AspNetCore.Mvc;

namespace netcore.fcimiddleware.fondos.api.Controllers.V1
{
    [ApiController]
    [Route("/")]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            _logger.LogInformation($"| Running Api | netcore.fcimiddleware.fondos.api");
            return $"Running | Running Api | netcore.fcimiddleware.fondos.api";
        }
    }
}
