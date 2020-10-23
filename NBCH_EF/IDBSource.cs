using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NBCH_EF.Tables;

namespace NBCH_EF {
	/// <summary>
	/// Интерфейс базы данных
	/// для интеграционного тестирования.
	/// </summary>
	internal interface IDBSource : IDisposable {
		/// <summary>
		/// Таблица PDF файлов
		/// </summary>
		DbSet<PDFFile> PDFFiles {get; set;}

		/// <summary>
		/// Список пользователей
		/// </summary>
		DbSet<ADUserDB> ADUsers { get; set;}

		/// <summary>
		/// Список регионов
		/// </summary>
		DbSet<RegionDB> Regions {get; set;}

		/// <summary>
		/// Клиенты (владельцы PDF)
		/// </summary>
		DbSet<ClientDB> Clients {get; set;}

		/// <summary>
		/// Связь доступных регионов и пользователей AD
		/// </summary>
		DbSet<ADUserRegionRelation> ADUserRegionRelations {get; set;}

		/// <summary>
		/// Кредитные истории клиентов
		/// </summary>
		DbSet<CreditHistory> CreditHistories {get; set;}

		/// <summary>
		/// Договора 1С
		/// </summary>
		DbSet<Account1C> Account1Cs {get; set;}

		/// <summary>
		/// Данные для расчета ПДН
		/// </summary>
		DbSet<PDNData> PDNDatas {get; set;}

		/// <summary>
		/// Группы документов
		/// </summary>
		DbSet<DocumentGroupDB> DocumentGroupDBs {get; set;}

		/// <summary>
		/// Описания загружаемых файлов
		/// </summary>
		DbSet<FileDescriptionDB> FileDescriptionDBs {get; set;}

		/// <summary>
		/// Роли AD
		/// </summary>
		DbSet<ADRoleDB> ADRoleDBs {get; set;}

		/// <summary>
		/// Описание пользователя из AD
		/// </summary>
		DbSet<ADLoginsDB> ADLoginsDBs {get; set;}

		/// <summary>
		/// Файлы архива
		/// </summary>
		DbSet<RegistrarFileDB> RegistrarFileDBs {get; set;}

		/// <summary>
		/// Организация, заключившая сделку
		/// </summary>
		DbSet<OrganizationDB> OrganizationDBs { get; set; }

		/// <summary>
		/// Точка заключения сделки
		/// </summary>
		DbSet<SellPontDB> SellPointDBs { get; set; }

		/// <summary>
		/// Рассчитанное ПДН
		/// </summary>
		DbSet<PDNResultDB> PDNResultDBs { get; set; }

		/// <summary>
		/// Канал перевода средств
		/// </summary>
		DbSet<TypeOfChargeDB> TypeOfChargeDBs { get; set;}

		/// <summary>
		/// Поручители
		/// </summary>
		DbSet<GuarantorDB> GuarantorDBs { get; set; }

		/// <summary>
		/// Сообщения
		/// </summary>
		DbSet<PostsDB> PostsDBs { get; set; }

		/// <summary>
		/// Таблица проверок.
		/// </summary>
		DbSet<AccountInspectingDB> AccountInspectingDB { get; set; }

		/// <summary>
		/// Сохранить изменения
		/// </summary>
		/// <returns></returns>
		int SaveChanges();

		/// <summary>
		/// Сохранить изменения асинхронно
		/// </summary>
		/// <returns></returns>
		Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default);

		/// <summary>
		/// Изменить состояние объекта в контексте
		/// </summary>
		/// <param name="entity">Объект, состояние которого в контексте необходимо изменить</param>
		/// <returns></returns>
		EntityEntry Entry(object entity);
	}
}
