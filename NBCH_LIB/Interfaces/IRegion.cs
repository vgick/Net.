using System.ServiceModel;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Models;

namespace NBCH_LIB.Interfaces {
	/// <summary>
	/// Контракт для работы с WCF сервисом списка регионов
	/// </summary>
	[ServiceContract]
	public interface IRegion : IWCFContract {
		/// <summary>
		/// Добавить регион в базу
		/// </summary>
		/// <param name="name">Название региона</param>
		/// <returns>Новый регион</returns>
		[OperationContract]
		Region AddRegion(string name);
		/// <summary>
		/// Получить регион по ID
		/// </summary>
		/// <param name="id">ID региона</param>
		/// <returns>Регион. Null если регион не найден</returns>
		[OperationContract]
		Region GetRegionByID(int id);
		/// <summary>
		/// Получить список регионов по имени
		/// </summary>
		/// <param name="name">Часть имени региона</param>
		/// <returns>Регионы в которых есть часть имени</returns>
		[OperationContract]
		Region[] GetRegionsByName(string name);
		/// <summary>
		/// Получить список всех регионов.
		/// </summary>
		/// <returns>Список регионов</returns>
		[OperationContract]
		Region[] GetRegions();
		/// <summary>
		/// Удалить регион по ID
		/// </summary>
		/// <param name="id"></param>
		[OperationContract]
		void DeleteRegion(int id);
	}
}
