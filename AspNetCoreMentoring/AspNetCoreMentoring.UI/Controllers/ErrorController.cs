using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMentoring.UI.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMentoring.UI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Exception error = this.HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            _logger.LogError(error, "An error occurred while processing your request with Id {0}", requestId);

            var errorModel = new ErrorViewModel
            {
                RequestId = requestId
            };

            return View("Error", errorModel);
        }


        [Route("error/404")]
        public IActionResult NotFoundView()
        {
            return View();
        }
    }
}