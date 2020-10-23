using System.ComponentModel;

namespace NBCH_WCF {
	partial class ProjectInstaller {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.NBCHProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.NBCHInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// NBCHProcessInstaller
			// 
			this.NBCHProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.NBCHProcessInstaller.Password = null;
			this.NBCHProcessInstaller.Username = null;
			// 
			// NBCHInstaller
			// 
			this.NBCHInstaller.Description = "Сервис по работе с НБКИ";
			this.NBCHInstaller.DisplayName = "National credit history bureau";
			this.NBCHInstaller.ServiceName = "NBCH";
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.NBCHProcessInstaller,
            this.NBCHInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller NBCHProcessInstaller;
		private System.ServiceProcess.ServiceInstaller NBCHInstaller;
	}
}