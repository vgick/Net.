using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Query;
using NBCH_EF.Tables;
using NBCH_LIB.Models.Registrar;
using NBCH_LIB.SOAP.SOAP1C.GetAccountsList;

namespace NBCH_EF
{
    internal partial class MKKContext : DbContext, IDBSource {
	    public static ILoggerFactory LoggerFactory;

	    /// <summary>
	    /// Максимальное кол-во параметров в запросе
	    /// Необходимо, чтобы разбивать большие запросы на маленькие.
	    /// </summary>
	    public const int MaxCountOfParameters = 1000;

	    static MKKContext() {
		    LoggerFactory = new LoggerFactory().
			    AddSeq().
			    AddFile("Logs/base-{Date}.txt");
	    }

		/// <summary>
		/// Конструктор.
		/// </summary>
		public MKKContext() { }

        public MKKContext(DbContextOptions<MKKContext> options) : base(options) { }

		#region Таблицы
        /// <summary>
		/// Таблица PDF файлов.
		/// </summary>
		public DbSet<PDFFile> PDFFiles { get; set; }

		/// <summary>
		/// Список пользователей.
		/// </summary>
		public DbSet<ADUserDB> ADUsers { get; set; }

		/// <summary>
		/// Список регионов.
		/// </summary>
		public DbSet<RegionDB> Regions { get; set; }

		/// <summary>
		/// Клиенты.
		/// </summary>
		public DbSet<ClientDB> Clients { get; set; }

		/// <summary>
		/// Связь доступных регионов и пользователей AD.
		/// </summary>
		public DbSet<ADUserRegionRelation> ADUserRegionRelations { get; set; }

		/// <summary>
		/// Кредитные истории клиентов.
		/// </summary>
		public DbSet<CreditHistory> CreditHistories { get; set; }

		/// <summary>
		/// Договора 1С.
		/// </summary>
		public DbSet<Account1C> Account1Cs { get; set; }

		/// <summary>
		/// Данные для расчета ПДН.
		/// </summary>
		public DbSet<PDNData> PDNDatas { get; set; }

		/// <summary>
		/// Группы документов.
		/// </summary>
		public DbSet<DocumentGroupDB> DocumentGroupDBs { get; set; }

		/// <summary>
		/// Описания загружаемых файлов.
		/// </summary>
		public DbSet<FileDescriptionDB> FileDescriptionDBs { get; set; }

		/// <summary>
		/// Роли AD.
		/// </summary>
		public DbSet<ADRoleDB> ADRoleDBs { get; set; }

		/// <summary>
		/// Описание пользователя из AD.
		/// </summary>
		public DbSet<ADLoginsDB> ADLoginsDBs { get; set; }

		/// <summary>
		/// Файлы архива.
		/// </summary>
		public DbSet<RegistrarFileDB> RegistrarFileDBs { get; set; }

		/// <summary>
		/// Организация, заключившая сделку.
		/// </summary>
		public DbSet<OrganizationDB> OrganizationDBs { get; set; }

		/// <summary>
		/// Точка заключения сделки.
		/// </summary>
		public DbSet<SellPontDB> SellPointDBs { get; set; }

		/// <summary>
		/// Рассчитанное ПДН.
		/// </summary>
		public DbSet<PDNResultDB> PDNResultDBs { get; set; }

		/// <summary>
		/// Поручители.
		/// </summary>
		public DbSet<GuarantorDB> GuarantorDBs { get; set; }

		/// <summary>
		/// Канал перевода средств.
		/// </summary>
		public DbSet<TypeOfChargeDB> TypeOfChargeDBs { get; set; }

		/// <summary>
		/// Сообщения.
		/// </summary>
		public DbSet<PostsDB> PostsDBs { get; set; }

		/// <summary>
		/// Таблица проверок.
		/// </summary>
		public DbSet<AccountInspectingDB> AccountInspectingDB { get; set; }
		#endregion

		#region Запросы к БД
		/// <summary>
		/// Результат запроса.
		/// Документы по договору в разрезе.
		/// </summary>
		public DbSet<FileDescriptionQuery> FileDescriptionQuery { get; set;}

		/// <summary>
		/// Результат запроса.
		/// Доступы к документам.
		/// </summary>
		public DbSet<RoleListQuery> RoleListQuery { get; set; }

		/// <summary>
		/// Результат запроса.
		/// Доступы к документам.
		/// </summary>
		public DbSet<AccountsForCheck> AccountsForCheckQuery { get; set; }
		#endregion

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
#if DEBUG
			string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionTestBase"].ConnectionString;
#else
			string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionProductBase"].ConnectionString;
#endif
			if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
	        modelBuilder.Entity<ADRoleDB>().HasOne(d => d.WriteADRoles).WithMany(p => p.WriteADRoles).HasForeignKey(d => d.WriteADRolesID);
	        modelBuilder.Entity<ADRoleDB>().HasOne(d => d.ReadADRoles).WithMany(p => p.ReadADRoles).HasForeignKey(d => d.ReadADRolesID);

			modelBuilder.Entity<ADLoginsDB>().HasIndex(i => i.Login).IsUnique(); 
	        modelBuilder.Entity<ClientDB>().HasIndex(i => i.Code1C).IsUnique();
			modelBuilder.Entity<OrganizationDB>().HasIndex(i => i.Name).IsUnique();
	        modelBuilder.Entity<PDNResultDB>().HasIndex(i => i.Account1CID).IsUnique();
	        modelBuilder.Entity<TypeOfChargeDB>().HasIndex(i => i.Name).IsUnique();
			modelBuilder.Entity<SellPontDB>().HasIndex(i => i.Name).IsUnique();

	        modelBuilder.Entity<ADUserRegionRelation>().HasKey(k => new { k.ADUserID, k.RegionID });
	        modelBuilder.Entity<ADUserRegionRelation>().HasOne(r => r.ADUser).WithMany(a => a.ADUserRegionRelations).HasForeignKey(k => k.ADUserID).IsRequired();
	        modelBuilder.Entity<ADUserRegionRelation>().HasOne(r => r.Region).WithMany(r => r.ADUserRegionRelations).HasForeignKey(k => k.RegionID).IsRequired();

	        modelBuilder.Entity<GuarantorDB>().HasKey(k => new { k.Account1CID, k.ClientDBID });
	        modelBuilder.Entity<GuarantorDB>().HasOne(r => r.Account).WithMany(i => i.GuarantorDBs).HasForeignKey(k => k.Account1CID).IsRequired();
	        modelBuilder.Entity<GuarantorDB>().HasOne(r => r.Client).WithMany(i => i.GuarantorDBs).HasForeignKey(k => k.ClientDBID).IsRequired();

	        modelBuilder.Entity<AccountInspectingDB>().HasOne(i => i.Account1C).WithMany(a => a.AccountInspectingDbs).HasForeignKey(k => k.Account1CID).IsRequired();
	        modelBuilder.Entity<AccountInspectingDB>().HasOne(i => i.ADLoginsDB).WithMany(u => u.AccountInspectingDbs).HasForeignKey(k => k.ADLoginsDBID).IsRequired();
	        
			// Запросы, хранимые процедуры T-SQL.
			modelBuilder.Entity<FileDescriptionQuery>().HasNoKey();
			modelBuilder.Entity<RoleListQuery>().HasNoKey();
			modelBuilder.Entity<AccountsForCheck>().HasNoKey();

			base.OnModelCreating(modelBuilder);
        }

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
