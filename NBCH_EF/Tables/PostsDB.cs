using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using NBCH_EF.Helpers;
using NBCH_LIB;
using NBCH_LIB.Models.Posts;
using static NBCH_EF.MKKContext;

namespace NBCH_EF.Tables {
	[Table("PostsDBs")]
	internal class PostsDB: Interface.IDBTableID {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID { get; set; }

		/// <summary>
		/// Договор 1С.
		/// </summary>
		[Column("Account_Account1CCode")]
		public string AccountAccount1CCode { get; set; }
		public Account1C Account { get; set; }

		/// <summary>
		/// Автор сообщения.
		/// </summary>
		[Required]
		[Column("Author_ID")]
		public int AuthorID { get; set; }
		public ADLoginsDB Author { get; set; }

		/// <summary>
		/// Дата сообщения.
		/// </summary>
		[Required]
		[Column(TypeName = "datetime")]
		public DateTime Date { get; set; }

		/// <summary>
		/// Сообщение.
		/// </summary>
		[MaxLength(512)]
		public string Message { get; set; }

		/// <summary>
		/// Уровень доступа автора (AuthorPermissionLevel).
		/// </summary>
		public int AuthorPermissionLevel { get; set; }

		/// <summary>
		/// Часовой пояс автора сообщения.
		/// </summary>
		public int ClientTimeZone { get; set; }

		/// <summary>
		/// Приведение PostDB => Post.
		/// </summary>
		/// <param name="postDB">Объект класса Post</param>
		public static explicit operator Post(PostsDB postDB) {
			Post post	= new Post() {
				Account1CCode	= postDB.Account.Account1CCode,
				Author			= postDB.Author.Name,
				Date			= postDB.Date,
				Message			= postDB.Message
			};

			return post;
		}

		/// <summary>
		/// Создать новый объект для добавления в БД на основании Post.
		/// </summary>
		/// <param name="post">Сообщение для публикации</param>
		/// <param name="clientTimeZone">Часовой пояс клиента</param>
		/// <param name="adLogin">Логин пользователя</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public static async Task<PostsDB> CreatePostsDB(Post post, int clientTimeZone, string adLogin,
			CancellationToken cancellationToken) {
			PostsDB postDB	= new PostsDB() {
				Author					= Singleton<ProxyADLoginsDB>.Values[adLogin],
				Date					= post.Date,
				Message					= post.Message ?? "",
				AuthorPermissionLevel	= PermissionLevel.GetInt(adLogin),
				ClientTimeZone			= clientTimeZone,
				Account					= await FindAccountAndLogErrorAsync<PostsDB>(post.Account1CCode, cancellationToken)
			};

			return postDB;
		}
	}
}
