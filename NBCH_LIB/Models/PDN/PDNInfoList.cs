using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models.PDN {
	/// <summary>
	/// Полный набор данных для расчета ПДН на договор 1С
	/// </summary>
	[DataContract]
	public class PDNInfoList {
		/// <summary>
		/// Конструктор без параметров.
		/// </summary>
		public PDNInfoList() { }

		/// <summary>
		/// Конструктор с параметром для заполнения.
		/// </summary>
		/// <param name="pdnList"></param>
		public PDNInfoList(PDN[] pdnList) {
			if (pdnList == default) return;

			List<PDNCard> cards			= new List<PDNCard>();
			List<PDNNonCard> nonCards	= new List<PDNNonCard>();

			foreach (PDN pdn in pdnList) {
				if (pdn is PDNCard pdnCard) {
					cards.Add(pdnCard);
					continue;
				}

				if (pdn is PDNNonCard pdnNonCard) {
					nonCards.Add(pdnNonCard);
					continue;
				}
			}

			PDNCards	= cards.ToArray();
			PDNNonCards	= nonCards.ToArray();
		}

		/// <summary>
		/// Данные для расчета ПДН карты.
		/// </summary>
		[DataMember]
		public PDNCard[] PDNCards {get; set;} = new PDNCard[0];

		/// <summary>
		/// Данные для расчета ПДН не карты.
		/// </summary>
		[DataMember]
		public PDNNonCard[] PDNNonCards {get; set;} = new PDNNonCard[0];

		/// <summary>
		/// Дата формирования отчета (дата анкеты, используемой для расчета ПДН).
		/// </summary>
		[DataMember]
		public DateTime ReportDate {get; set;}

		/// <summary>
		/// ID анкеты НБКИ, используемой для расчета ПДН.
		/// </summary>
		[DataMember]
		public int CreditHistoryID {get; set;}

		/// <summary>
		/// Номер договора 1С.
		/// </summary>
		[DataMember]
		public string Account1CID { get; set; }

		/// <summary>
		/// Дата заведения договора 1С.
		/// Требуется если при расчете ПДН не запрашивалась новая анкета, а
		/// использовалась анкета из архива.
		/// </summary>
		[DataMember]
		public DateTime Account1CDate { get; set; }

		/// <summary>
		/// Была ручная корректировка.
		/// </summary>
		[DataMember]
		public bool Manual {get; set;}

		/// <summary>
		/// Принять текущий расчет ПДН как действительный (принять возможные ошибки в расчете, как допустимые).
		/// </summary>
		[DataMember]
		public bool PDNAccept { get; set; }
	}
}





