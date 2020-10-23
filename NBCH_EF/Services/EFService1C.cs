using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBCH_EF.Helpers;
using NBCH_EF.Tables;
using NBCH_EF.Tables.Interface;
using NBCH_LIB;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Logger;
using NBCH_LIB.SOAP.SOAP1C;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using static NBCH_EF.MKKContext;
using static NBCH_LIB.Logger.ExceptionLog;

namespace NBCH_EF.Services {
	public class EFService1C : IService1CFUll {
		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static EFService1C() {
			_Logger = MKKContext.LoggerFactory.CreateLogger<EFService1C>();
		}

		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger<EFService1C> _Logger;

		/// <summary>
		/// Обновить информацию по договору 1С.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		public void UpdateAccountAndClientInfo(CreditDocumentNResult document1C) =>
			UpdateAccountAndClientInfoAsync(document1C, CancellationToken.None).WaitAndThrowException();

		/// <summary>
		/// Обновить информацию по договору 1С асинхронно.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		public async Task UpdateAccountAndClientInfoAsync(CreditDocumentNResult document1C) =>
			await UpdateAccountAndClientInfoAsync(document1C, CancellationToken.None);

		/// <summary>
		/// Обновить информацию по договору 1С асинхронно.
		/// </summary>
		/// <param name="document1C">Договора 1С</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task UpdateAccountAndClientInfoAsync(CreditDocumentNResult document1C,
			CancellationToken cancellationToken) {
			
			UpdateAccountAndClientInfoCheckParams(document1C);

			Account1C account1C		= (Account1C)document1C.CreditDocument;
			ClientDB[] clientDBs	= ClientsFromAccount(document1C, account1C);

			ClientTask[] presentClientsTask	= clientDBs.AsParallel().
				Select(i => new ClientTask() {
					ClientDB = i,
					Task = FindClientAndLogErrorAsync<EFService1C>(i.Code1C, cancellationToken)
				}).
				WithCancellation(cancellationToken).
				ToArray();
			
			UpdateClientsInfo(presentClientsTask, cancellationToken);

			Account1C presentAccount1CTask	= await FindAccountAndLogErrorAsync<EFService1C>(
				account1C.Account1CCode,
				cancellationToken, 
				c => c.Client, o => o.Organization, sp => sp.SellPont, cht => cht.TypeOfCharge);

			await UpdateAccount1CInfoAsync(account1C, presentAccount1CTask, cancellationToken);

			if (clientDBs.Length > 1) await UpdateGuarantorsAsync(clientDBs, cancellationToken);
		}

		/// <summary>
		/// Проверить входные параметры UpdateAccountAndClientInfoCheck.
		/// </summary>
		/// <param name="document1C">Договор 1С</param>
		private void UpdateAccountAndClientInfoCheckParams(CreditDocumentNResult document1C) {
			if (document1C?.CreditDocument == default)
				LogAndThrowException<ArgumentNullException, EFService1C>(_Logger,
					nameof(document1C),
					"Не задан договор./* Метод {methodName} document1C {document1C}.*/",
					"UpdateAccountAndClientInfoCheckParams", document1C);
		}


		/// <summary>
		/// Обновить список поручителей асинхронно.
		/// </summary>
		/// <param name="clientDB">Клиент</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private static async Task UpdateGuarantorsAsync(ClientDB[] clientDB, CancellationToken cancellationToken) {
			string account1CCode	= clientDB.Where(acc => acc.GuarantorDBs != default).
				Select(acc => acc.GuarantorDBs).FirstOrDefault().FirstOrDefault()?.Account.Account1CCode;
			Account1C account1C		= await FindAccountAndLogErrorAsync<EFService1C>(account1CCode, cancellationToken);

			await DeleteGuarantorsAsync(account1C, cancellationToken);
			await AddGuarantorsAsync(clientDB, account1C, cancellationToken);
		}

		/// <summary>
		/// Добавить в договор поручителей асинхронно.
		/// </summary>
		/// <param name="clientDB">Список клиентов, в том числе поручителей</param>
		/// <param name="account1C">Договор</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private static async Task AddGuarantorsAsync(ClientDB[] clientDB, Account1C account1C,
			CancellationToken cancellationToken) {
			
			using (IDBSource dbSource = new MKKContext()) {
				string[] guarantorsCode	= clientDB.
					Where(j => j.GuarantorDBs != default).
					Select(j => j.Code1C).
					ToArray();
				
				ClientDB[] guarantors	= dbSource.Clients.
					Where(i => guarantorsCode.Contains(i.Code1C)).
					ToArray();
				
				AttachAndLogError<Account1C, EFService1C>((MKKContext)dbSource, account1C);

				foreach (ClientDB guarantor in guarantors) {
					GuarantorDB guarantorDB = new GuarantorDB() { Account = account1C, Client = guarantor };
					await dbSource.GuarantorDBs.AddAsync(guarantorDB, cancellationToken);
				}

				await dbSource.SaveChangesAndLogErrorAsync<EFService1C>(new LogShortMessage(
					message: "/*Метод {methodName}, clientDB {clientDB}, account1C {account1C}*/",
					"AddGuarantorsAsync", clientDB, account1C),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Удалить из договора всех поручителей асинхронно.
		/// </summary>
		/// <param name="account1C">Договор</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private static async Task DeleteGuarantorsAsync(Account1C account1C, CancellationToken cancellationToken) {
			using (IDBSource dbSource = new MKKContext()) {
				IQueryable<GuarantorDB> guarantorDBs	=
					dbSource.GuarantorDBs.Where(gr => gr.Account1CID.Equals(account1C.Account1CCode));
				
				dbSource.GuarantorDBs.RemoveRange(guarantorDBs);
				
				await dbSource.SaveChangesAndLogErrorAsync<EFService1C>(new LogShortMessage(
					message: "/*Метод {methodName}, guarantorDBs {guarantorDBs}, account1C {account1C}*/",
					"DeleteGuarantorsAsync", guarantorDBs, account1C),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Обновить/добавить по договору 1С запись в БД асинхронно.
		/// </summary>
		/// <param name="account1CDB">Договор из 1С</param>
		/// <param name="presentAccount1C">Договор из 1С в БД</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private async Task UpdateAccount1CInfoAsync(Account1C account1CDB, Account1C presentAccount1C,
			CancellationToken cancellationToken) {
			
			if (presentAccount1C == default) await AddNewAccountRecordAsync(account1CDB, cancellationToken);
			else await UpdateAccountRecordAsync(account1CDB, presentAccount1C, cancellationToken);
		}

		/// <summary>
		/// Обновить информацию по договору асинхронно.
		/// </summary>
		/// <param name="account1CDB">новый договор</param>
		/// <param name="presentAccount1C">Договор в базе</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		private static async Task UpdateAccountRecordAsync(Account1C account1CDB, Account1C presentAccount1C,
			CancellationToken cancellationToken) {
			
			using (IDBSource dbSource = new MKKContext()) {
				if (!presentAccount1C.Equals1CParams(account1CDB)) {
					AttachAndLogError<Account1C, EFService1C>((MKKContext)dbSource, presentAccount1C);

					if (presentAccount1C.Client != default)
						dbSource.Entry(presentAccount1C.Client).State = EntityState.Detached;

					presentAccount1C.DateTime	= account1CDB.DateTime;

					ClientDB clientDB	=
						await FindClientAndLogErrorAsync<EFService1C>(account1CDB.Client.Code1C, cancellationToken);
					dbSource.Entry(clientDB).State	= EntityState.Unchanged;

					presentAccount1C.Client					= clientDB;
					dbSource.Entry(presentAccount1C).State	= EntityState.Modified;
				}

				if (presentAccount1C.Organization == default)
					await UpdateAccountAsync(dbSource, presentAccount1C, account1CDB.Organization, cancellationToken);

				if (presentAccount1C.SellPont == default)
					await UpdateAccountAsync(dbSource, presentAccount1C, account1CDB.SellPont, cancellationToken);

				if (presentAccount1C.TypeOfCharge == default && presentAccount1C.TypeOfCharge?.Name != account1CDB.TypeOfCharge.Name)
					await UpdateAccountAsync(dbSource, presentAccount1C, account1CDB.TypeOfCharge, cancellationToken);

				if (presentAccount1C.AdditionAgrement != account1CDB.AdditionAgrement) {
					presentAccount1C.AdditionAgrement = account1CDB.AdditionAgrement;
					dbSource.Entry(presentAccount1C).State	= EntityState.Modified;
				}

				await dbSource.SaveChangesAndLogErrorAsync<EFService1C>(new LogShortMessage(
					message: "/*Метод {methodName}, document1C {document1C}, pdnValue {pdnValue}*/",
					"AddNewAccountRecordAsync", account1CDB, presentAccount1C, cancellationToken),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Добавить новую запись в БД асинхронно.
		/// </summary>
		/// <param name="account1CDB">Новая запись</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private async Task AddNewAccountRecordAsync(Account1C account1CDB, CancellationToken cancellationToken) {
			using (MKKContext dbSource = new MKKContext()) {
				ClientDB accountClientDB		= await FindClientAndLogErrorAsync<EFService1C>(account1CDB.Client.Code1C, cancellationToken);
				OrganizationDB organizationDB	= await FindRequiredDBRecordByNameAndLogErrorAsync<OrganizationDB, EFService1C>(account1CDB.Organization.Name, cancellationToken);
				SellPontDB sellPointDB			= await FindRequiredDBRecordByNameAndLogErrorAsync<SellPontDB, EFService1C>(account1CDB.SellPont.Name, account1CDB.SellPont.Code1C, cancellationToken);
				TypeOfChargeDB typeOfCharge		= await FindRequiredDBRecordByNameAndLogErrorAsync<TypeOfChargeDB, EFService1C>(account1CDB.TypeOfCharge.Name, cancellationToken);

				AttachAndLogError<OrganizationDB, EFService1C>(dbSource, organizationDB);
				AttachAndLogError<SellPontDB, EFService1C>(dbSource, sellPointDB);
				AttachAndLogError<ClientDB, EFService1C>(dbSource, accountClientDB);
				AttachAndLogError<TypeOfChargeDB, EFService1C>(dbSource, typeOfCharge);

				account1CDB.Organization	= organizationDB;
				account1CDB.SellPont		= sellPointDB;
				account1CDB.Client			= accountClientDB;
				account1CDB.TypeOfCharge	= typeOfCharge;

				await dbSource.Account1Cs.AddAsync(account1CDB, cancellationToken);

				await dbSource.SaveChangesAndLogErrorAsync<EFService1C>(new LogShortMessage(
					message: "/*Метод {methodName}, account1C {account1C}*/",
					"AddNewAccountRecordAsync", account1CDB),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Проверить - есть ли в БД клиенты, если нет, добавить/обновить запись в БД.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="presentClientsTask"></param>
		private static void UpdateClientsInfo(ClientTask[] presentClientsTask, CancellationToken cancellationToken) {
			try { presentClientsTask.AsParallel().WithCancellation(cancellationToken).ForAll(UpdateClientFunction); }
			catch (AggregateException exception) {
				if (exception.InnerException != default) {
					throw exception.InnerException;
				}
			}
		}

		/// <summary>
		/// функция обновления данных клиента.
		/// </summary>
		/// <param name="clientTask">Задача с данными клиента</param>
		private static void UpdateClientFunction(ClientTask clientTask) {
			using (IDBSource dbSource = new MKKContext()) {
				if (clientTask.ClientDB.BirthDate < new DateTime(1920, 1, 1))
					LogAndThrowException<Exception, EFService1C>(_Logger,
						"",
						"Проверьте год рождения клиента {clientLastName} {clientFirstName} {clientSecondName}" +
						" (код клиента {clientID1C}). Год рождения: {clientBirthDate}./* Метод {methodName}.*/",
						clientTask.ClientDB.LastName, clientTask.ClientDB.FirstName,
						clientTask.ClientDB.SecondName, clientTask.ClientDB.Code1C, clientTask.ClientDB.BirthDate,
						"UpdateClientsInfo");

				ClientDB presentClient					= clientTask.Task.ResultAndThrowException();
				ICollection<GuarantorDB> guarantorDB	= clientTask.ClientDB.GuarantorDBs;
				clientTask.ClientDB.GuarantorDBs		= default;
				
				if (presentClient == default) {
					dbSource.Clients.Add(clientTask.ClientDB);
					presentClient	= clientTask.ClientDB;
				}
				else dbSource.Entry(presentClient).State	= EntityState.Modified;

				presentClient.BirthDate		= clientTask.ClientDB.BirthDate;
				presentClient.FIO			= clientTask.ClientDB.FIO;
				presentClient.FirstName		= clientTask.ClientDB.FirstName;
				presentClient.LastName		= clientTask.ClientDB.LastName;
				presentClient.SecondName	= clientTask.ClientDB.SecondName;

				try { dbSource.SaveChanges(); }
				catch (Exception exception) {
					LogAndThrowException<Exception, EFService1C>(_Logger,
						"",
						"Не удалось обновить данные клиента по договору. Клиент {ClientDB}. Ошибка работы" +
						" с БД./* Метод {methodName}, исключение: {ContextException}*/",
						new ExLogValue() {
							LogValue = clientTask.ClientDB,
							ExValue = clientTask.ClientDB.Code1C
						},
						"UpdateClientsInfo", exception);
				}

				clientTask.ClientDB.GuarantorDBs = guarantorDB;
			}
		}

		/// <summary>
		/// Получить список клиентов из договора.
		/// </summary>
		/// <param name="document">Договор 1С из вебсервиса</param>
		/// <param name="account1C">Договор 1С</param>
		/// <returns>Список клиентов</returns>
		private static ClientDB[] ClientsFromAccount(CreditDocumentNResult document, Account1C account1C) {
			List<ClientDB> clientDBs	= new List<ClientDB> { (ClientDB)document.CreditDocument.Client };

			foreach (ClientProfile client in document.CreditDocument?.Guarantors ?? new ClientProfile[0]) {
				if (SOAP1C.StringToDateTime(client.BirthDate) < new DateTime(1920, 1, 1))
					LogAndThrowException<Exception, EFService1C>(_Logger,
						"",
						"Проверьте год рождения клиента {clientLastName} {clientFirstName} {clientSecondName}" +
						" (код клиента {clientID1C}). Год рождения:{clientBirthDate)}./* Метод {methodName}.*/",
						client.LastName, client.FirstName, client.SecondName, client.ID1C, client.BirthDate,
						"CreditDocumentNResult");

				ClientDB clientDB				= (ClientDB)client;
				List<GuarantorDB> guarantors	= new List<GuarantorDB> { new GuarantorDB() { Account = account1C } };
				clientDB.GuarantorDBs			= guarantors;

				clientDBs.Add(clientDB);
			}

			return clientDBs.ToArray();
		}

		/// <summary>
		/// Обновить значение эккаунта асинхронно.
		/// </summary>
		/// <typeparam name="TEntity">Обновляемый тип данных</typeparam>
		/// <param name="dbSource">Источник данных</param>
		/// <param name="account1C">Счет</param>
		/// <param name="value">Новое значение</param>
		/// <param name="cancellationToken">Токен отмены</param>
		private static async Task UpdateAccountAsync<TEntity>(IDBSource dbSource, Account1C account1C, TEntity value,
			CancellationToken cancellationToken) where TEntity : class, IDBTableName, new() {
			
			AttachAndLogError<Account1C, EFService1C>((MKKContext)dbSource, account1C);

			TEntity valueDB		=
				await FindRequiredDBRecordByNameAndLogErrorAsync<TEntity, EFService1C>(value.Name, cancellationToken);
			string fieldName	= valueDB.GetType().Name.Substring(0, (valueDB.GetType().Name.Length - "DB".Length));

			dbSource.Entry(valueDB).State	= EntityState.Unchanged;
			account1C.GetType().GetProperty(fieldName)?.SetValue(account1C, valueDB);
			dbSource.Entry(account1C).State	= EntityState.Modified;
		}

		/// <summary>
		/// Загрузить ПДН по договору.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		public void LoadPDNFromFile(CreditDocumentNResult document1C, double pdnValue) => Task.Run(() =>
			LoadPDNFromFileAsync(document1C, pdnValue, CancellationToken.None)).WaitAndThrowException();

		/// <summary>
		/// Загрузить ПДН по договору асинхронно.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		public async Task LoadPDNFromFileAsync(CreditDocumentNResult document1C, double pdnValue) =>
			await LoadPDNFromFileAsync(document1C, pdnValue, CancellationToken.None);

		/// <summary>
		/// Загрузить ПДН по договору асинхронно.
		/// </summary>
		/// <param name="document1C">Договор</param>
		/// <param name="pdnValue">Значение процента ПДН</param>
		/// <param name="cancellationToken">Токен отмены</param>
		public async Task LoadPDNFromFileAsync(CreditDocumentNResult document1C, double pdnValue,
			CancellationToken cancellationToken) {
			
			LoadPDNFromFileAsyncCheckParams(document1C);

			await UpdateAccountAndClientInfoAsync(document1C, cancellationToken);

			string account1CCode = document1C.CreditDocument.Code1C;

			using (IDBSource dbSource = new MKKContext()) {
				PDNResultDB pdn = await dbSource.PDNResultDBs.
					AsNoTracking().
					Where(i => i.Account1C.Account1CCode.Equals(account1CCode)).
					SingleOrDefaultAndErrorAsync<PDNResultDB, EFService1C>(cancellationToken);
				
				if (pdn == default) {
					pdn = new PDNResultDB();
					dbSource.Entry(pdn).State = EntityState.Added;
				}

				Account1C account1CInDB	= await FindAccountAndLogErrorAsync<EFService1C>(account1CCode, cancellationToken);

				pdn.Account1C			= account1CInDB;
				pdn.Percent				= pdnValue;
				dbSource.Entry(account1CInDB).State = EntityState.Unchanged;

				await dbSource.SaveChangesAndLogErrorAsync<EFService1C>(new LogShortMessage(
					message: "/*Метод {methodName}, document1C {document1C}, pdnValue {pdnValue}*/",
					"AddNewAccountRecordAsync", document1C, pdnValue),
					cancellationToken
				);
			}
		}

		/// <summary>
		/// Проверить входные параметры метода LoadPDNFromFile.
		/// </summary>
		/// <param name="document1C"></param>
		private void LoadPDNFromFileAsyncCheckParams(CreditDocumentNResult document1C) {
			if (string.IsNullOrEmpty(document1C?.CreditDocument?.Code1C ?? ""))
				LogAndThrowException<ArgumentNullException, EFService1C>(_Logger,
					nameof(document1C),
					"Не задан документ 1С./* Метод {methodName}.*/",
					"LoadPDNFromFileAsyncCheckParams");
		}

		/// <summary>
		/// Структура для хранения промежуточных данных для асинхронного вызова.
		/// </summary>
		private struct ClientTask {
			/// <summary>
			/// Данные клиента из БД.
			/// </summary>
			public ClientDB ClientDB { get; set; }

			/// <summary>
			/// Задача.
			/// </summary>
			public Task<ClientDB> Task { get; set; }
		}
	}
}
