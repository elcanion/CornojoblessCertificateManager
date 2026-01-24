using System.Windows;
using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CornojoblessCertificateManager.UI.ViewModel;

namespace CornojoblessCertificateManager.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public static IHost AppHost { get; private set; }

	public App() {
		AppHost = Host.CreateDefaultBuilder()
			.ConfigureServices((ctx, services) => {

			services.AddSingleton<ICertificateService, CertificateService>();
			services.AddSingleton(provider =>
			{
				var config = ctx.Configuration;
				return config.GetSection("CertificateSettings").Get<CertificateSettings>()!;
			});
			services.AddSingleton<MainWindowViewModel>();
			services.AddSingleton<MainWindow>();
		}).Build();
	}

	protected override void OnStartup(StartupEventArgs e) {
		base.OnStartup(e);

		var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
		mainWindow.Show();
	}
}

