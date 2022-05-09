using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventGrid_receiver.Models;
using EventGrid_receiver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventGrid_receiver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventGridService _eventGrid;

        public HomeController(ILogger<HomeController> logger, IEventGridService eventGrid)
        {
            _logger = logger;
            _eventGrid = eventGrid;
        }

        public async Task<IActionResult> IndexAsync()
        {
            await _eventGrid.PublishTopic("test", "sample.eventgridhandler.testappmessage", "this is a test event");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
