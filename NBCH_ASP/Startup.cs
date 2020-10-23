using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Logging;
using NBCH_ASP.Infrastructure;
using NBCH_ASP.Infrastructure.DataFromConfigurationFile.ISecrets;
using NBCH_ASP.Infrastructure.WCFClient;
using NBCH_EF.Services;
using NBCH_LIB.Interfaces;
using NBCH_LIB.Interfaces.WCF;
using NBCH_LIB.Services;

namespace NBCH_ASP {
	public class Startup {
		/// <summary>
		/// Имя ключа в кукисах с информацией о регионе
		/// </summary>
		public static readonly string WebService1CRegion	= "WebService1CRegion";
		/// <summary>
		/// Настройки для подключения к сервису 1С из файла конфигурации
		/// </summary>
		public static readonly string WebService1С			= "WebService1C";
		/// <summary>
		/// Настройки для подключения к сервису НБКИ из файла конфигурации
		/// </summary>
		public static readonly string WebServiceNBCH		= "WebServiceNBCH";

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		private IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddCors();

			services.AddAuthentication(IISDefaults.AuthenticationScheme);
			services.AddControllersWithViews();
			services.AddSingleton<IADUser>(p => (IADUser)WCFProxyFactory.GetWCFProxy(typeof(IADUser), Configuration));
			services.AddSingleton<IRegion>(p => (IRegion)WCFProxyFactory.GetWCFProxy(typeof(IRegion), Configuration));
			services.AddTransient<IPDFSaver>(p => (IPDFSaver)WCFProxyFactory.GetWCFProxy(typeof(IPDFSaver), Configuration));

			//services.AddTransient<IService1CWCF>(p => (IService1CWCF)WCFProxyFactory.GetWCFProxy(typeof(IService1CWCF), Configuration));
			//services.AddTransient<IService1СSoapWCF>(p => (IService1СSoapWCF)WCFProxyFactory.GetWCFProxy(typeof(IService1СSoapWCF), Configuration));
			services.AddTransient<IService1C, EFService1C>();
			services.AddTransient<IService1СSoap, Service1СSoap>();

			//services.AddTransient<IServiceNBCHWCF>(p => (IServiceNBCHWCF)WCFProxyFactory.GetWCFProxy(typeof(IServiceNBCHWCF), Configuration));
			services.AddTransient<IServiceNBCH, EFServiceNBCH>();
			services.AddTransient<IServiceNBCHsoap, ServiceNBCHsoap>();

			//services.AddTransient<IServiceRegistrarWCF>(p => (IServiceRegistrarWCF)WCFProxyFactory.GetWCFProxy(typeof(IServiceRegistrarWCF), Configuration));
			services.AddTransient<IServiceRegistrar, EFServiceRegistrar>();

			//services.AddTransient<IServicePDNWCF>(p => (IServicePDNWCF)WCFProxyFactory.GetWCFProxy(typeof(IServicePDNWCF), Configuration));
			services.AddTransient<IServicePDN, EFServicePDN>();

			services.AddTransient<ISecret1C>(p => Secret1C.GetSecret(Configuration));
			services.AddTransient<ISecretNBCH[]>(p => SecretNBCH.GetSecretNBCHs(Configuration));
			
			//services.AddTransient<IServicePostsWCF>(p => (IServicePostsWCF)WCFProxyFactory.GetWCFProxy(typeof(IServicePostsWCF), Configuration));
			services.AddTransient<IServicePosts, EFServicePosts>();
			
			services.AddTransient<IServiceInspecting, EFServiceInspecting>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			loggerFactory.AddFile("Logs/nbch-{Date}.txt");

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();


			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod());

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: null,
					pattern: "RegistrarAdm/{action}/{SelectedDocument?}",
					defaults: new { controller = "RegistrarRolesAdministration", action = "FileDescription" });
				endpoints.MapControllerRoute(
					name: null,
					pattern: "PDNEdit/{action}/{Account1CCode}/{ReportDate}/{CreditHistoryID}",
					defaults: new { controller = "PDNEdit", action = "Index" });
				endpoints.MapControllerRoute(
					name: null,
					pattern: "NBCHRequest/{action}",
					defaults: new { controller = "NBCHRequest", action = "Index" });
				endpoints.MapControllerRoute(
					name: null,
					pattern: "Archive/{action}/{CreditHistoryID?}",
					defaults: new { controller = "ArchiveCH", action = "GetCh" });
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{CreditHistoryID?}");
			});
		}
	}
}
