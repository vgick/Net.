using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using NBCH_EF.Services;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Services;

namespace NBCH_WCF.Services {
	internal class ServiceMethod {
		private static readonly List<ServiceHost> _ServiceHosts = new List<ServiceHost>();

		/// <summary>
		/// Статический конструктор.
		/// </summary>
		static ServiceMethod() {
			LoggerFactory	= new LoggerFactory().
				AddSeq().
				AddFile(AppDomain.CurrentDomain.BaseDirectory + "Logs/wcf-{Date}.txt");

			Service1C			= new EFService1C();
			Service1СSoap		= new Service1СSoap(Service1C, LoggerFactory);
			ServiceNBCH			= new EFServiceNBCH();
			ServiceNBCHsoap		= new ServiceNBCHsoap(ServiceNBCH, LoggerFactory);
			ServicePDN			= new EFServicePDN();
			ServicePosts		= new EFServicePosts();
			ServiceRegistrar	= new EFServiceRegistrar();
		}

		internal static ILoggerFactory LoggerFactory { get;  }

		/// <summary>
		/// Сервис для работы с данными 1С.
		/// </summary>
		internal static IService1CFUll Service1C { get; }

		/// <summary>
		/// Сервис для работы с данными НБКИ.
		/// </summary>
		internal static IServiceNBCHFull ServiceNBCH { get; }

		/// <summary>
		/// Сервис для работы с ПДН.
		/// </summary>
		internal static IServicePDNWCF ServicePDN { get; }

		/// <summary>
		/// Сервис для работы с сообщениями.
		/// </summary>
		internal static IServicePostsWCF ServicePosts { get; }

		/// <summary>
		/// Сервис для работы с архивом.
		/// </summary>
		internal static IServiceRegistrarWCF ServiceRegistrar { get; }

		/// <summary>
		/// Сервис для работы с СОАП запросами 1С.
		/// </summary>
		internal static IService1СSoapWCF Service1СSoap { get; }

		/// <summary>
		/// Сервис для работы с СОАП запросами к серверу НБКИ.
		/// </summary>
		internal static IServiceNBCHsoapWCF ServiceNBCHsoap { get; }

		/// <summary>
		/// Закрыть все службы.
		/// </summary>
		internal static void Stop(){
			foreach (var item in _ServiceHosts) {
				item.Close();
			}

			LoggerFactory.CreateLogger<ServiceMethod>().LogError("Services is stopped.");
		}

		/// <summary>
		/// Запустить службы.
		/// </summary>
		internal static void Start(){
			StartWCFService(typeof(WCFService1CSoap));
			StartWCFService(typeof(WCFService1C));
			StartWCFService(typeof(WCFServiceNBCH));
			StartWCFService(typeof(WCFServiceNBCHSoap));
			StartWCFService(typeof(WCFServicePDN));
			StartWCFService(typeof(WCFServiceRegistrar));
			StartWCFService(typeof(WCFServicePosts));

			LoggerFactory.CreateLogger<ServiceMethod>().LogInformation("Services is started.");
		}

		/// <summary>
		/// Запустить службу.
		/// </summary>
		/// <param name="serviceHostType">Запускаемая служба</param>
		/// <returns>Результат запуска службы</returns>
		private static bool StartWCFService(Type serviceHostType) {
			try {
				ServiceHost serviceHost = new ServiceHost(serviceHostType);
				serviceHost.Open();
				_ServiceHosts.Add(serviceHost);
				LoggerFactory.CreateLogger<ServiceMethod>().
					LogInformation("Service: {ServiceName} is started. Sate: {ServiceState}.", serviceHostType.Name, serviceHost.State);
			}
			catch (Exception exception) {
				LoggerFactory.CreateLogger<ServiceMethod>().
					LogError("Ошибка при запуске службы {ServiceName}. Ошибка: {Exception}",
						serviceHostType.Name, exception.Message);
				return false;
			}

			return true;
		}


	}
}
