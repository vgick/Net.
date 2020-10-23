using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NBCH_LIB.Models.Posts {
	/// <summary>
	/// Описание поста.
	/// </summary>
	[DataContract]
	public class Post {
		/// <summary>
		/// Код договора 1С.
		/// </summary>
		[DataMember]
		[Required]
		public string Account1CCode { get; set; }

		/// <summary>
		/// Автор сообщения.
		/// </summary>
		[DataMember]
		[Required]
		public string Author { get; set; }

		/// <summary>
		/// Дата сообщения.
		/// </summary>
		[DataMember]
		[Required]
		public DateTime Date { get; set; }

		/// <summary>
		/// Сообщение.
		/// </summary>
		[DataMember]
		[MaxLength(512)]
		public string Message { get; set; }
	}
}
