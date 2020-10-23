using System.Collections.Generic;
using System.Linq;
using NBCH_LIB.Models;

namespace NBCH_ASP.Models.PDF {
	/// <summary>
	/// ADUser со списком регионов, в которых есть чекбокс + метод для загрузки всех регионов, которые есть.
	/// </summary>
	public class ADUserMVC{
		/// <summary>
		/// ADUser. В самом объекте Region не заполнен.
		/// </summary>
		public ADUser ADUser {get; set;}

		/// <summary>
		/// Список регионов.
		/// </summary>
		private readonly List<RegionWChecked> _Regions = new List<RegionWChecked>();

		/// <summary>
		/// Свойство для заполнения и получения регионов с чекбоксом.
		/// </summary>
		public RegionWChecked[] Regions {
			get => _Regions.ToArray();
			set {
				_Regions.Clear();
				_Regions.AddRange(value);
			}
		}

		/// <summary>
		/// Загрузить регионы, которые ещё не включены в список регионов и присвоить имь статус checked = false.
		/// </summary>
		/// <param name="regions"></param>
		public void LoadAllRegions(Region[] regions){
			IEnumerable<Region> newRegions				= regions.Except(_Regions);
			IEnumerable<RegionWChecked> newRegionsWCh	= newRegions.Select(r =>
				new RegionWChecked() {ADUsers = r.ADUsers, ID = r.ID, Name = r.Name, Checked = false}
			);
			_Regions.AddRange(newRegionsWCh);
		}

		/// <summary>
		/// Неявное приведение ADUser к ADUserMVC? с заполнением списка регионов класса ADRegionMVC из ADRegion.Regions.
		/// </summary>
		/// <param name="adUser"></param>
		public static implicit operator ADUserMVC(ADUser adUser) {
			ADUserMVC result	= new ADUserMVC() {ADUser = adUser};
			IEnumerable<RegionWChecked> regions	= adUser?.Regions?.Select(i => new RegionWChecked() {ID = i.ID, Name = i.Name, ADUsers = i.ADUsers, Checked = true});
			if (regions != null) result._Regions.AddRange(regions);

			return result;
		}
	}
}
