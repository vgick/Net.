using System;
using System.Collections.Generic;
using System.Linq;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Models;

namespace NBCH_ASP.LocalStorage {
	/// <summary>
	/// Класс для тестирования, реализующий интерфейс IRegion
	/// </summary>
	public class LocalRegions : IRegion {
		/// <summary>
		/// Хранилище списка регионов.
		/// </summary>
		public static List<Region> Regions {get; set;} = new List<Region>();

		/// <summary>
		/// Статический конструктор для заполнения первоначальными данными.
		/// </summary>
		static LocalRegions() {
			Regions.Add(new Region() {ID = 0, Name = "Name0"});
			Regions.Add(new Region() {ID = 1, Name = "Name1"});
			Regions.Add(new Region() {ID = 2, Name = "Name2"});
		}

		/// <summary>
		/// Добавить регион в базу.
		/// </summary>
		/// <param name="name">Название региона</param>
		/// <returns>Новый регион</returns>
		public Region AddRegion(string name) {
			Region region	= new Region() {Name = name};
			region.ID		= Regions.Max(i => i.ID);

			Regions.Add(region);

			return region;
		}

		/// <summary>
		/// Удалить регион по ID.
		/// </summary>
		/// <param name="id"></param>
		public void DeleteRegion(int id) {
			Region region	= GetRegionByID(id);

			if (region == null) throw new ArgumentOutOfRangeException();
			Regions.Remove(region);
		}

		/// <summary>
		/// Получить регион по ID.
		/// </summary>
		/// <param name="id">ID региона</param>
		/// <returns>Регион. Null если регион не найден</returns>
		public Region GetRegionByID(int id) {
			return Regions.FirstOrDefault(i => i.ID == id);
		}

		/// <summary>
		/// Получить список всех регионов.
		/// </summary>
		/// <returns>Список регионов</returns>
		public Region[] GetRegions() {
			return Regions.ToArray();
		}

		/// <summary>
		/// Получить список регионов по имени.
		/// </summary>
		/// <param name="name">Часть имени региона</param>
		/// <returns>Регионы в которых есть часть имени</returns>
		public Region[] GetRegionsByName(string name) {
			return Regions.Where(i => i.Name.Contains(name)).ToArray();
		}
	}
}
