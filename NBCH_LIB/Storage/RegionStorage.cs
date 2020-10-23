using System.Threading.Tasks;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

namespace NBCH_LIB.Storage {
	public class RegionStorage : IRegion {
		/// <summary>
		/// Хранилище регионов.
		/// </summary>
		private IRegion Region {get; set;} = default;
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="region">Хранилище регионов</param>
		public RegionStorage(IRegion region) => Region = region;

		/// <summary>
		/// Добавить регион в базу асинхронно.
		/// </summary>
		/// <param name="name">Название региона</param>
		/// <returns>Новая запись</returns>
		public async Task<Region> AddRegionAsync(string name) {return await Task.Run(() => Region.AddRegion(name));}

		/// <summary>
		/// Удалить регион по ID асинхронно.
		/// </summary>
		/// <param name="id">ID региона</param>
		public async Task DeleteRegionAsync(int id) => await Task.Run(() => Region.DeleteRegion(id));

		/// <summary>
		/// Получить регион по ID асинхронно.
		/// </summary>
		/// <param name="id">ID региона</param>
		/// <returns>Регион. Null если регион не найден</returns>
		public async Task<Region> GetRegionByIDAsync(int id) => await Task.Run(() => Region.GetRegionByID(id));

		/// <summary>
		/// Получить список регионов по имени.
		/// </summary>
		/// <param name="name">Часть имени региона</param>
		/// <returns>Регионы в которых есть часть имени</returns>
		public async Task<Region[]> GetRegionsByNameAsync(string name) => await Task.Run(() => Region.GetRegionsByName(name));

		/// <summary>
		/// Получить список всех регионов асинхронно.
		/// </summary>
		/// <returns>Список регионов</returns>
		public async Task<Region[]> GetRegionsAsync() => await Task.Run(() => Region.GetRegions());

		/// <summary>
		/// Добавить регион в базу.
		/// </summary>
		/// <param name="name">Название региона</param>
		/// <returns>Новая запись</returns>
		public Region AddRegion(string name) => Region.AddRegion(name);

		/// <summary>
		/// Получить регион по ID.
		/// </summary>
		/// <param name="id">ID региона</param>
		/// <returns>Регион. Null если регион не найден</returns>
		public Region GetRegionByID(int id) => Region.GetRegionByID(id);

		/// <summary>
		/// Получить список регионов по имени.
		/// </summary>
		/// <param name="name">Часть имени региона</param>
		/// <returns>Регионы в которых есть часть имени</returns>
		public Region[] GetRegionsByName(string name) => Region.GetRegionsByName(name);

		/// <summary>
		/// Получить список всех регионов.
		/// </summary>
		/// <returns>Список регионов</returns>
		public Region[] GetRegions() => Region.GetRegions();

		/// <summary>
		/// Удалить регион по ID.
		/// </summary>
		/// <param name="id">ID региона</param>
		public void DeleteRegion(int id) => Region.DeleteRegion(id);
	}
}
