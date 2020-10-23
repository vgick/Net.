using NBCH_EF.Services;
using NBCH_LIB;
using NBCH_LIB.Interfaces;

namespace NBCH_EF {
	public static class MKKEFBase {
		/// <summary>
		/// Конструктор.
		/// </summary>
		static MKKEFBase() {
			ServicePosts		= Singleton<EFServicePosts>.Values;
			ServiceRegistrar	= new EFServiceRegistrar();
		}

		/// <summary>
		/// Сервис для работы с почтой.
		/// </summary>
		public static readonly IServicePosts ServicePosts;

		/// <summary>
		/// Сервис для работы с архивом.
		/// </summary>
		public static readonly IServiceRegistrar ServiceRegistrar;
	}
}
