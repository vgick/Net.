using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NBCH_ASP.Infrastructure.WCFClient {
	/// <summary>
	/// Настройки прокси клиента
	/// </summary>
	public class ProxySetting {
		/// <summary>
		/// Binding
		/// </summary>
		public Binding Binding	= default;
		/// <summary>
		/// Address
		/// </summary>
		public EndpointAddress Address	= default;
		/// <summary>
		/// Contract
		/// </summary>
		public String Contract	= default;
	}
}
