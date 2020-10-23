using System;
using System.Collections.Generic;
using System.Linq;

namespace NBCH_LIB.SOAP.SOAPProxy {
	public class SOAPProxy {
		/// <summary>
		/// Время устаревания данных в секундах
		/// </summary>
		private static int _UpdateIntervalInSeconds	= 10;

		private readonly object _LockCopy	= new object();

		/// <summary>
		/// Кэш
		/// </summary>
		private readonly Dictionary<UpdateInfo, ISOAPData> _ProxyData	= new Dictionary<UpdateInfo, ISOAPData>();

		/// <summary>
		/// Обновить данные кэша
		/// </summary>
		/// <param name="soapMethod">Соап метод</param>
		/// <param name="server">Сервер</param>
		/// <param name="updateInfo">Обновляемый кэш</param>
		private void UpdateCache<T>(SOAP1C.SOAPMethod<T> soapMethod, string server, UpdateInfo updateInfo) where T : ISOAPData {
			T data					= soapMethod.GetData(server);
			updateInfo.Time			= DateTime.Now;

			lock (_LockCopy) {
				_ProxyData[updateInfo] = data;
			}
		}


		/// <summary>
		/// Добавить в кэш новую запись
		/// </summary>
		/// <param name="soapMethod">Соап метод</param>
		/// <param name="server">Сервер</param>
		private void AddNewCache<T>(SOAP1C.SOAPMethod<T> soapMethod, string server) where T : ISOAPData {
			T data	= soapMethod.GetData(server);

			UpdateInfo updateInfo	= new UpdateInfo() {
				Server		= server,
				AdditionKey	= soapMethod.AdditionKey,
				Time		= DateTime.Now,
				Type		= typeof(T).ToString()
			};
			lock (_LockCopy) {
				_ProxyData.Add(updateInfo, data);
			}
		}

		/// <summary>
		/// Найти обновление в кэше
		/// </summary>
		/// <param name="server">Закэшированный сервер</param>
		/// <param name="additionKey">Дополнительный ключ кэша</param>
		/// <param name="type">Тип кэшированного значения (соап метод)</param>
		/// <returns>Ключ кэша</returns>
		private UpdateInfo UpdateInfoByServer(string server, string additionKey, string type) {
			UpdateInfo record = default;

			lock (_LockCopy) {
				foreach (UpdateInfo item in _ProxyData.Keys) {
					if (item.Server == server && item.AdditionKey == additionKey && item.Type == type) {
						record = item;
						break;
					}
				}

				UpdateInfo[] updateInfo	= _ProxyData.Keys.Where(i => (DateTime.Now - i.Time).TotalMinutes > 10 && record != i).ToArray();
				foreach (var remove in updateInfo) {
					_ProxyData.Remove(remove);
				}
			}

			return record;
		}

		/// <summary>
		/// Получить данные из кэша.
		/// </summary>
		/// <typeparam name="T">Типа данных</typeparam>
		/// <param name="soapMethod">Сервис для получения новых данных</param>
		/// <param name="server">Сервер для подключения</param>
		/// <returns>Данные</returns>
		public T GetData<T>(SOAP1C.SOAPMethod<T> soapMethod, string server) where T : ISOAPData {
			string itemType = typeof(T).ToString();

			UpdateInfo updateInfo = UpdateInfoByServer(server, soapMethod.AdditionKey, itemType);
			if (updateInfo == default)
				AddNewCache(soapMethod, server);

			updateInfo = UpdateInfoByServer(server, soapMethod.AdditionKey, itemType);
			if (updateInfo.Time.AddSeconds(_UpdateIntervalInSeconds) < DateTime.Now && !updateInfo.Updating) {
				lock (_ProxyData[updateInfo]) {
					updateInfo.Updating = true;
					UpdateCache(soapMethod, server, updateInfo);
					updateInfo.Updating = false;
				}
			}

			T result;
			lock (_LockCopy) {
				result = (T)_ProxyData[updateInfo].Clone();
			}

			return result;
		}

		/// <summary>
		/// Информация о обновлении
		/// </summary>
		private class UpdateInfo {
			/// <summary>
			/// Время обновления
			/// </summary>
			public DateTime Time { get; set; }

			/// <summary>
			/// Сервер
			/// </summary>
			public string Server { get; set; }

			/// <summary>
			/// Дополнительный ключ
			/// </summary>
			public string AdditionKey { get; set; }

			/// <summary>
			/// Тип кэшированного значения (соап метод)
			/// </summary>
			public string Type { get; set; }

			/// <summary>
			/// Объект в стадии обновления.
			/// </summary>
			public bool Updating { get; set; }
		}
	}
}
