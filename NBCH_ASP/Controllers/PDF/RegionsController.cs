using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Storage;

namespace NBCH_ASP.Controllers.PDF {
	[Authorize(Roles = @"role,roleadmin")]
	public class RegionsController : Controller {
		readonly RegionStorage _RegionStorage;
		public RegionsController(IRegion region) => _RegionStorage = new RegionStorage(region);
		[HttpGet]
		public async Task<IActionResult> Index() {
			//RegionModel regionModel	= new RegionModel();
			//regionModel.

			return View(await _RegionStorage.GetRegionsAsync());
		}
	}
}