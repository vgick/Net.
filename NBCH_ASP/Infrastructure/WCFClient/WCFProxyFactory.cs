using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using static NBCH_ASP.Infrastructure.WCFClient.WCFProxyConfiguration;

namespace NBCH_ASP.Infrastructure.WCFClient {
	/// <summary>
	/// Фабрика WCF клиентов
	/// </summary>
	public static class WCFProxyFactory {
		/// <summary>
		/// Сборка с прокси классами WCF и интерфейсом IWCFInterface ()
		/// </summary>
		private const string _WCFProxyAssemblyName = "NBCH_LIB";

		/// <summary>
		/// NameSpace в котором находятся прокси.
		/// </summary>
		private const string _WCFProxyNameSpace = "NBCH_LIB.WCFProxy";

		/// <summary>
		/// Получить прокси с необходимым контрактом.
		/// </summary>
		/// <param name="contract">Искомый контракт</param>
		/// <param name="configuration">IConfiguration настройки программы. Если не заданы, то подразумевается, что ранее уже были
		/// установлены или через вызов метода, или напрямую ProxyConfiguration.Configuration</param>
		/// <returns>Прокси с необходимым контрактом</returns>
		public static IWCFContract GetWCFProxy(Type contract, IConfiguration configuration = default){
			if (contract == default) return default;
			return GetWCFProxyByContract(contract, GetWCFProxySetting(contract, configuration));
		}

		/// <summary>
		/// Создать WCF прокси с необходимым контрактом.
		/// </summary>
		/// <param name="contract">Необходимый контракт</param>
		/// <param name="proxySetting">Настройки для WCF прокси</param>
		/// <returns>Прокси с необходимым контрактом или default, если прокси с необходимым контрактом не найден</returns>
		private static IWCFContract GetWCFProxyByContract(Type contract, ProxySetting proxySetting){
			if (proxySetting == default || contract == default) return default;

			IWCFContract proxy;
			try {
				Type contractType = FindTypeWithInterface(contract);
				if (contractType == default) return default;
				proxy = (IWCFContract)Activator.CreateInstance(contractType, proxySetting.Binding, proxySetting.Address);
			}
			catch {
				return default;
			}

			return proxy;
		}

		/// <summary>
		/// Найти класс, с реализацией необходимого контракта.
		/// </summary>
		/// <param name="contract">Интерфейс, который необходимо реализовать</param>
		/// <returns>Класс с реализацией интерфейса</returns>
		private static Type FindTypeWithInterface(Type contract) {
			if (contract == default) return default;

			Type type	= Assembly.
				Load(_WCFProxyAssemblyName).
				GetTypes().
				FirstOrDefault(t => t.IsClass && t.GetInterface(contract.Name) != default && t.Namespace == _WCFProxyNameSpace);
			return type;
		}
	}
}
