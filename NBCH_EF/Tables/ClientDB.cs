using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;
using NBCH_EF.Tables.Interface;
using NBCH_LIB.Models;
using NBCH_LIB.SOAP.SOAP1C;
using NBCH_LIB.SOAP.SOAP1C.GetClientData;
using NBCH_LIB.SOAP.SOAPNBCH;

namespace NBCH_EF.Tables {
	/// <summary>
	/// Клиент.
	/// </summary>
	[Table("ClientDBs")]
	internal class ClientDB : IDBTable {
		/// <summary>
		/// Ключевое поле.
		/// </summary>
		[Key]
		[Column("ID")]
		public int ID {get; set;}

		/// <summary>
		/// Код контрагента в 1С.
		/// </summary>
		[MaxLength(15)]
		public string Code1C {get; set;}

		/// <summary>
		/// Имя.
		/// </summary>
		[MaxLength(70)]
		public string FirstName {get; set;}

		/// <summary>
		/// Отчество.
		/// </summary>
		[MaxLength(70)]
		public string SecondName {get; set;}

		/// <summary>
		/// Фамилия.
		/// </summary>
		[MaxLength(70)]
		public string LastName {get; set;}

		/// <summary>
		/// Рудимент...
		/// ФИО (фамилия + имя + отчество).
		/// </summary>
		[Required]
		[MaxLength(150)]
		public string FIO {get; set;}

		/// <summary>
		/// Дата Рождения.
		/// </summary>
		[Column(TypeName = "datetime")]
		public DateTime BirthDate {get; set;}

		/// <summary>
		/// Является поручителем.
		/// </summary>
		public ICollection<GuarantorDB> GuarantorDBs { get; set; }

		/// <summary>
		/// Рудимент...
		/// Регион пользователя.
		/// </summary>
		[Column("Regions_ID")]
		public int? RegionsId { get; set; }
		public RegionDB Regions { get; set; }

		/// <summary>
		/// Договора.
		/// </summary>
		public virtual ICollection<Account1C> Account1C { get; set; }

		/// <summary>
		/// Концертировать отдельное ФИО в одну строку.
		/// </summary>
		/// <param name="fn">Имя</param>
		/// <param name="sn">Отчество</param>
		/// <param name="ln">Фамилия</param>
		/// <returns>ФИО</returns>
		public static string F_I_OToFIO(string fn, string sn, string ln)
			=> $"{ln?.Trim() ?? ""} {fn?.Trim() ?? ""} {sn?.Trim() ?? ""}".Replace("  ", " ").Replace("  ", " ");

		/// <summary>
		/// Конвертировать ФИО в отдельные фамилию, имя, отчество.
		/// </summary>
		/// <param name="fullFIO">ФИО</param>
		/// <returns>(имя, отчество, фамилия)</returns>
		public static (string FirstName, string SecondName, string LastName) FIOToF_I_O(string fullFIO){
			//(string FirstName, string SecondName, string LastName) result	= new (string FirstName, string SecondName, string LastName);
			if (string.IsNullOrEmpty(fullFIO)) throw new ArgumentNullException();
			string[] fio = fullFIO.Split(' ');
			string lastName, firstName, secondName;

			switch (fio.Length) {
				case 3:
					lastName	= fio[0];
					firstName	= fio[1];
					secondName	= fio[2];
					break;
				case 2:
					lastName	= fio[0];
					firstName	= fio[1];
					secondName	= default;
					break;
				case 1:
					lastName	= fio[0];
					firstName	= default;
					secondName	= default;
					break;
				default:
					secondName	= fio[fio.Length - 1];
					firstName	= fio[fio.Length - 2];
					lastName	= "";
					for (int i = 0; i < fio.Length - 2; i++) {
						lastName	+= $"{fio[i]} ";
					}
					lastName	= lastName.Trim();
					break;
			}

			return (FirstName: firstName, SecondName: secondName, LastName: lastName);
		}

		/// <summary>
		/// Привести описание клиента из 1С, к описанию в базе.
		/// </summary>
		/// <param name="clientProfile">Клиент из 1С</param>
		public static explicit operator ClientDB(ClientProfile clientProfile) {
			if (clientProfile == default) return default;

			ClientDB client;
			try {
				client = new ClientDB() {
					Code1C		= clientProfile.ID1C,
					FirstName	= clientProfile.FirstName,
					SecondName	= clientProfile.SecondName,
					LastName	= clientProfile.LastName,
					FIO			= F_I_OToFIO(clientProfile.FirstName, clientProfile.SecondName, clientProfile.LastName),
					BirthDate	= SOAP1C.StringToDateTime(clientProfile.BirthDate)
				};
			}
			catch (Exception exception) {
				ILogger<ClientDB> logger	= MKKContext.LoggerFactory.CreateLogger<ClientDB>();
				logger.LogError("Не удалось привести ClientProfile {ClientProfile} к ClientDB. Ошибка {exception}", clientProfile, exception);
				return default;
			}

			return client;
		}

		/// <summary>
		/// Приведение ProductRequest к ClientDB (данные из НБКИ).
		/// </summary>
		/// <param name="productRequest">ClientDB</param>
		public static explicit operator ClientDB(ProductRequest productRequest) {
			if (productRequest == default) return default;
			ClientDB client;
			try {
				client	= new ClientDB() {
					FirstName	= productRequest.Prequest.Req.PersonReq.FirstName,
					SecondName	= productRequest.Prequest.Req.PersonReq.SecondName,
					LastName	= productRequest.Prequest.Req.PersonReq.LastName,
					FIO			= F_I_OToFIO(
						productRequest.Prequest.Req.PersonReq.FirstName,
						productRequest.Prequest.Req.PersonReq.SecondName,
						productRequest.Prequest.Req.PersonReq.LastName),
					BirthDate	= productRequest.Prequest.Req.PersonReq.BirthDateTime

				};
			}
			catch (Exception exception) {
				ILogger<ClientDB> logger	= MKKContext.LoggerFactory.CreateLogger<ClientDB>();
				logger.LogError("Не удалось привести ProductRequest {ProductRequest} к ClientDB. Ошибка {exception}", productRequest, exception);
				return default;
			}

			return client;
		}

		/// <summary>
		/// Приведение ClientDB к Client (данные из НБКИ).
		/// </summary>
		/// <param name="val"></param>
		public static explicit operator Client(ClientDB val) {
			if (val == default) return default;

			return new Client() {
				BirthDate	= val.BirthDate,
				Code1C		= val.Code1C,
				LastName	= val.LastName,
				FirstName	= val.FirstName,
				SecondName	= val.SecondName
			};
		}

		/// <summary>
		/// Сравнение с объектом по параметрам из 1С.
		/// </summary>
		/// <param name="obj">Сравниваемый объект</param>
		/// <returns>Результат сравнения</returns>
		public bool Equals1C(object obj) {
			ClientDB client	= obj as ClientDB;
			if (client == default)
				return false;

			if (!client.FIO.Equals(FIO) || !client.Code1C.Equals(Code1C) || !client.FirstName.Equals(FirstName) || !client.BirthDate.Equals(BirthDate) ||
				!client.LastName.Equals(LastName) || !client.SecondName.Equals(SecondName)) return false;

			return true;
		}
	}
}
