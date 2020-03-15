using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CampaignsProductManager.API.controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger.ForContext<HomeController>();
        }

        public IActionResult Index()
        {
            _logger.Information("HomeController.Index called");
            return View();
        }
    }
}