using NBCH_LIB.Models.PDN;

namespace NBCH_ASP.Models.NBCH.PDNComponent {
	/// <summary>
	/// Данные для представления компонента PDNViewComponent.
	/// </summary>
	public class PDNDataView {
		/// <summary>
		/// Данные для расчета ПДН.
		/// </summary>
		public PDNInfoList PDNInfoList {get; set;}

		/// <summary>
		/// Форма редактируемая.
		/// </summary>
		public bool CanEdit {get; set;}

		/// <summary>
		/// Ошибка при запросе данных.
		/// </summary>
		public string Message {get; set;}
	}
}
