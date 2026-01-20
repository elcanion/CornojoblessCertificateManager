using System.Windows;
using CornojoblessCertificateManager.Core.Model;
using Microsoft.Extensions.Configuration;

namespace CornojoblessCertificateManager.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public static IConfiguration Configuration { get; private set; }
	public static CertificateSettings? Settings { get; private set; }

	protected override void OnStartup(StartupEventArgs e) {
		base.OnStartup(e);

		Configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.Build();

		Settings = Configuration
			.GetSection("CertificateSettings")
			.Get<CertificateSettings>();
	}
}

