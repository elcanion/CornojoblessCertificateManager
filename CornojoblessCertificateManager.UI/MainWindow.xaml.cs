using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace CornojoblessCertificateManager.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public StoreLocation Store;
	public bool? WillReadFromSettings;
    public MainWindow()
    {
        InitializeComponent();
		StoreCombo.SelectedIndex = 0;
		Store = StoreCombo.Text == "LocalMachine" ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
		WillReadFromSettings = ReadFromSettings.IsChecked;

		if (App.Settings != null) {
			var issuers = App.Settings.AllowedIssuers;
			Grid.ItemsSource = CertificateService.GetCertificate(Store, issuers, WillReadFromSettings);
		}
	}

	private void OnLoad(object sender, RoutedEventArgs e) {
		Store = StoreCombo.Text == "LocalMachine" ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
		WillReadFromSettings = ReadFromSettings.IsChecked;

		List<string> issuer = [];
		if (!String.IsNullOrEmpty(IssuerBox.Text)) {
			issuer.Add(IssuerBox.Text);
		}

		Grid.ItemsSource = CertificateService.GetCertificate(Store, issuer, WillReadFromSettings);
	}

	private void OnRemove(object sender, RoutedEventArgs e) {
		if (Grid.SelectedItems is not CertificateInfo info)
			return;

		OnLoad(null, null);
	}
}
