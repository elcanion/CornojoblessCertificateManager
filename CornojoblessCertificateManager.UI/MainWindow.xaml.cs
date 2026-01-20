using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Services;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace CornojoblessCertificateManager.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
		StoreCombo.SelectedIndex = 0;
    }

	private void OnLoad(object sender, RoutedEventArgs e) {
		var store = StoreCombo.Text == "LocalMachine" ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;

		var issuer = IssuerBox.Text;
		if (issuer != null) {
			Grid.ItemsSource = CertificateService.GetCertificate(store, issuer);
		}
	}

	private void OnRemove(object sender, RoutedEventArgs e) {
		if (Grid.SelectedItem is not CertificateInfo info)
			return;

		OnLoad(null, null);
	}
}
