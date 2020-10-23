using System.ServiceProcess;
using NBCH_WCF.Services;

namespace NBCH_WCF {
	public partial class NBCHServiceWCF : ServiceBase {
		public NBCHServiceWCF() {
			InitializeComponent();
		}

		protected override void OnStart(string[] args) {
			ServiceMethod.Start();
		}

		protected override void OnStop() {
			ServiceMethod.Stop();
		}
	}
}
