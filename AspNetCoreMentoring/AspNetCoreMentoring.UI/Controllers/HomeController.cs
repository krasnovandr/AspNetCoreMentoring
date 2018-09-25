using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreMentoring.UI.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace AspNetCoreMentoring.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
