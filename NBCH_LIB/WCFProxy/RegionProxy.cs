using System.ServiceModel;
using System.ServiceModel.Channels;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

namespace NBCH_LIB.WCFProxy {
	/// <summary>
	/// Прокси для работы с сервисом WCF
	/// </summary>
	public class RegionProxy : ClientBase<IRegion>, IRegion {
		#region Конструкторы
		public RegionProxy() { }
		public RegionProxy(string endpointName) : base(endpointName) { }
		public RegionProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }
		#endregion
		/// <summary>
		/// Добавить регион в базу.
		/// </summary>
		/// <param name="name">Название региона</param>
		/// <returns>Новая запись</returns>
		public Region AddRegion(string name) => Channel.AddRegion(name);

		/// <summary>
		/// Удалить регион по ID.
		/// </summary>
		/// <param name="id">ID региона</param>
		public void DeleteRegion(int id) => Channel.DeleteRegion(id);

		/// <summary>
		/// Получить регион по ID.
		/// </summary>
		/// <param name="id">ID региона</param>
		/// <returns>Регион. Null если регион не найден</returns>
		public Region GetRegionByID(int id) => Channel.GetRegionByID(id);

		/// <summary>
		/// Получить список регионов по имени.
		/// </summary>
		/// <param name="name">Часть имени региона</param>
		/// <returns>Регионы в которых есть часть имени</returns>
		public Region[] GetRegionsByName(string name) => Channel.GetRegionsByName(name);

		/// <summary>
		/// Получить список всех регионов.
		/// </summary>
		/// <returns>Список регионов</returns>
		public Region[] GetRegions() => Channel.GetRegions();
	}
}
