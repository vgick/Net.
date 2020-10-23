using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace NBCH_ASP.Infrastructure.WCFClient {
	public static class WCFProxyConfiguration {
		/// <summary>
		/// Настройки приложения
		/// </summary>
		public static IConfiguration Configuration;
		/// <summary>
		/// Описание сборки для binding
		/// </summary>
		private static readonly Dictionary<string, string> _BindingKeys = new Dictionary<string, string>() {
			["NetTcp"] = "System.ServiceModel.NetTcpBinding, System.ServiceModel.NetTcp",
			["Http"] = "System.ServiceModel.BasicHttpBinding, System.ServiceModel.Http"
		};

		/// <summary>
		/// Получить настройки соединения по контракту
		/// </summary>
		/// <param name="contract">Контракт</param>
		/// <param name="configuration">Конфигурация</param>
		/// <returns>Настройки соединения</returns>
		public static ProxySetting GetWCFProxySetting(Type contract, IConfiguration configuration = default){
			if (configuration != default) Configuration = configuration;

			return GetWCFProxySettingsFromConfig().FirstOrDefault(item => item.Contract == contract.Name);
		}

		/// <summary>
		/// Прочитать в файле конфигурации настройки WCF сервисов
		/// </summary>
		/// <returns>Список настроек прокси клиентов</returns>
		private static ProxySetting[] GetWCFProxySettingsFromConfig(){
			if (Configuration == default) return new ProxySetting[0];

			var servicesConfiguration	= Configuration.GetSection("Services");
			if (servicesConfiguration == default) return new ProxySetting[0];
			List<ProxySetting> proxySettings	= new List<ProxySetting>();

			foreach (IConfigurationSection binding in servicesConfiguration.GetChildren()) {
				foreach (IConfigurationSection contract in binding.GetChildren()) {
					ProxySetting proxySetting	= new ProxySetting();
					proxySetting.Contract		= contract.Key;
					proxySetting.Address		= new EndpointAddress(new Uri(contract.GetChildren().FirstOrDefault()?.Value ?? string.Empty));
					Type type					= Type.GetType(_BindingKeys[binding.Key]);
					proxySetting.Binding		= (NetTcpBinding)Activator.CreateInstance(type);
					//todo: вынести в настройки
					((NetTcpBinding)proxySetting.Binding).MaxReceivedMessageSize	= 2147483647;
					((NetTcpBinding)proxySetting.Binding).MaxBufferSize				= 2147483647;
					((NetTcpBinding)proxySetting.Binding).MaxBufferPoolSize			= 2147483647;
					//((NetTcpBinding)proxySetting.Binding).SendTimeout				= new TimeSpan(0, 5, 0);
					//((NetTcpBinding)proxySetting.Binding).ReceiveTimeout			= new TimeSpan(0, 5, 0);
					proxySettings.Add(proxySetting);
				}
			}

			return proxySettings.ToArray();
		}
	}
}
