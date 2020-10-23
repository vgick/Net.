using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBCH_ASP.Models;
using NBCH_ASP.Models.PDF;
using NBCH_ASP.Models.PDF.ADUsers;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;
using NBCH_LIB.Storage;

namespace NBCH_ASP.Controllers.PDF
{
	[Authorize(Roles = @"admin")]
	public class ADUsersController : Controller {
		/// <summary>
		/// Хранилище пользователей AD с асинхронными методами.
		/// </summary>
		private readonly ADUserStorage _ADUserStorage;

		/// <summary>
		/// Хранилище регионов с асинхронными методами.
		/// 
		/// </summary>
		private readonly RegionStorage _RegionStorage;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="adUser">Хранилище пользователей AD</param>
		/// <param name="region">Хранилище регионов</param>
		public ADUsersController(IADUser adUser, IRegion region) {
			_ADUserStorage	= new ADUserStorage(adUser);
			_RegionStorage	= new RegionStorage(region);
		}

		/// <summary>
		/// Список всех пользователей базы с возможностью добавлять новых и
		/// выбирать на редактирование выбранного.
		/// </summary>
		/// <returns>Представление</returns>
		public async Task<IActionResult> Index() {
			ADUsersIndexModel adUsersIndexModel	= new ADUsersIndexModel();
			adUsersIndexModel.ADUsers			= (await _ADUserStorage.GetADUsersAsync()).Select(u => (ADUserMVC)u);

			return View(adUsersIndexModel);
		}

		///// <summary>
		///// Список всех пользователей базы с возможностью добавлять новых и
		///// выбирать на редактирование выбранного.
		///// И строка поиска
		///// </summary>
		///// <param name="searchString">Строка поиска</param>
		///// <returns>Представление</returns>
		//[HttpPost]
		//public async Task<IActionResult> Index(string searchString) {
		//	ADUser[] adUsers = await _ADUserStorage.GetADUsersAsync();
		//	return View(adUsers);
		//}

		/// <summary>
		/// Вью с формой добавления нового пользователя системы.
		/// </summary>
		/// <returns>Вью</returns>
		public async Task<IActionResult> AddADUser(){
			RegionWChecked[] allRegions	= (await _RegionStorage.GetRegionsAsync())?.Select(r => new RegionWChecked(r)).ToArray();
			ADUserMVC addADUserModel = new ADUserMVC() {
				ADUser	= new ADUser(),
				Regions = allRegions
			};

			return View(addADUserModel);
		}

		///// <summary>
		///// Добавление новго пользователя
		///// </summary>
		///// <returns></returns>
		//[HttpPost]
		//public async Task<IActionResult> AddADUser(ADUserMVC model) {
		//	if (ModelState.IsValid){
		//		//ADUser adUser	= new ADUser();
		//		//adUser.ADName	= addADUserModel.ADUser.ADName;
		//		//adUser.Regions	= addADUserModel.Regions.Where(i => i.Checked).Select(i => (Region)i).ToArray();

		//		//int? userID	= ADUserProxy.AddADUser(adUser.ADName);



		//	}

		//	//ADUser adUser	= new ADUser() {ADName = "", Regions = Regions.GetRegions() };

		//	return View();
		//}

	}
}