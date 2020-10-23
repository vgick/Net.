using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NBCH_EF.Tables;
using NBCH_LIB.ADServiceProxy;

namespace NBCH_EF.Helpers {
	/// <summary>
	/// Прокси данных пользователей из AD / базы данных.
	/// </summary>
	internal class ProxyADLoginsDB : ProxyADLogins {
		/// <summary>
		/// Получить данные из кэша.
		/// </summary>
		/// <param name="login">Поиск по логину</param>
		/// <returns>Данные из кэша</returns>
		internal new ADLoginsDB this[string login]{
			get{
				if (login.IndexOf('\\') == -1) throw new ArgumentException($"Неверный формат логина AD. Логин должен быть вида: \"domain\\login\". Передан логин \"{login}\"");

				login = login.Substring(login.IndexOf('\\') + 1);

				lock (Locker) {
					if (_ADLogins.Keys.Contains(login)) UpdateDataInDB(login);
					else {
						using (IDBSource dbSource = new MKKContext()) {
							ADLoginsDB adLoginsIDB	= dbSource.ADLoginsDBs.AsNoTracking(). FirstOrDefault(i => i.Login.Equals(login));
							if (adLoginsIDB == default) 
								AddDataToDB(dbSource, login);
							else
								UpdateCacheFromDB(dbSource, adLoginsIDB, login);
						}
					}

					return (ADLoginsDB)_ADLogins[login];
				}
			}
		}

		/// <summary>
		/// Обновить кэш.
		/// </summary>
		/// <param name="dbSource">БД</param>
		/// <param name="adLoginsIDB">Логин в БД</param>
		/// <param name="login">Логин пользователя</param>
		private void UpdateCacheFromDB(IDBSource dbSource, ADLoginsDB adLoginsIDB, string login) {
			_ADLogins.TryAdd(login, adLoginsIDB);

			_ADLogins[login].Name		= GetNameByLogin(login);
			_ADLogins[login].UpdateDate	= DateTime.Now;

			dbSource.SaveChanges();
		}

		/// <summary>
		/// Добавить в БД новую запись пользователя.
		/// </summary>
		/// <param name="dBSource">БД</param>
		/// <param name="login">Логин пользователя</param>
		private void AddDataToDB(IDBSource dBSource, string login) {
			ADLoginsDB adLoginsIDB	= new ADLoginsDB() {
				Login		= login,
				Name		= GetNameByLogin(login),
				UpdateDate	= DateTime.Now
			};

			dBSource.ADLoginsDBs.Add(adLoginsIDB);
			dBSource.SaveChanges();

			_ADLogins.TryAdd(login, adLoginsIDB);
		}

		/// <summary>
		/// Обновить данные в БД.
		/// </summary>
		/// <param name="login">логин пользователя</param>
		private void UpdateDataInDB(string login){
			if ( _ADLogins[login].UpdateDate.AddDays(UpdateIntervalInDay) < DateTime.Now) {
				using (IDBSource dBSource = new MKKContext()) {
					_ADLogins[login].Name		= GetNameByLogin(login);
					_ADLogins[login].UpdateDate	= DateTime.Now;

					dBSource.SaveChanges();
				}
			}
		}
	}
}
