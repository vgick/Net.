using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Models;

namespace NBCH_ASP.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger) {
			_logger = logger;
		}

		//[AuthAttribute(AuthAttribute.AuthRole.User, AuthAttribute.AuthRole.Admin)]
		public IActionResult Index() {
			return View();
		}

		[ActionName("New")]
		//[AuthAttribute(AuthAttribute.AuthRole.User)]
		public IActionResult Privacy() {
			return View("Privacy");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
