using NBCH_LIB.Models.PDN;

namespace NBCH_ASP.Models.NBCH.PDNEditViewComponent {
	/// <summary>
	/// Данные для отрисовки формы с данными ПДН.
	/// </summary>
	public class PDNEditViewComponentView {
		/// <summary>
		/// ПДН карт.
		/// </summary>
		public PDNCard[] PDNCards { get; set; } = new PDNCard[0];

		/// <summary>
		/// ПДН не карты.
		/// </summary>
		public PDNNonCard[] PDNNonCards { get; set; } = new PDNNonCard[0];

		/// <summary>
		/// Форма редактируемая.
		/// </summary>
		public bool Disabled {get; set;}

		/// <summary>
		/// Ошибка при запросе данных.
		/// </summary>
		public string Message {get; set;}
	}
}
