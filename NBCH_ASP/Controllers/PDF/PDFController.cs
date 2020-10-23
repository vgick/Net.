using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCH_ASP.Models.PDF;
using NBCH_ASP.Models.PDF.PDF;

namespace NBCH_ASP.Controllers.PDF
{
	[Authorize(Roles = @"role,roleadmin")]
	public class PDFController : Controller
	{
		[HttpGet]
		public IActionResult Index() {
			//IndexModel indexModel = new IndexModel() { Regions = new Dictionary<string, bool> { ["Дальний восток"] = false, ["Крым"] = true } };
			//IndexModel indexModel = new IndexModel() { Regions = new bool[] { false, true }};

			IndexModel indexModel = new IndexModel() { Regions = new List<Check> {
					new Check() {Reg = "Владивосток", ch = false},
					new Check() {Reg = "Симферополь", ch = true }
				}
			};


			indexModel.FIO	= "??";


			return View(indexModel);
		}
		public IActionResult Index(IndexModel model) {
			return View(model);
		}
	}
}