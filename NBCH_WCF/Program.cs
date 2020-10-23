using System.ServiceProcess;

namespace NBCH_WCF {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main() {
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] {
				new NBCHServiceWCF()
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
