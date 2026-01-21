using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Services;
using CornojoblessCertificateManager.UI.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CornojoblessCertificateManager.UI;
public class StoreLocationOption {
	public string DisplayName { get; init; } = string.Empty;
	public StoreLocation Value { get; init; }
	public bool IsVisible { get; init; }
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public bool? WillReadFromSettings;
	public ObservableCollection<StoreLocation> StoreLocations { get; }
	private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

	public MainWindow()
    {
		InitializeComponent();
		WillReadFromSettings = ReadFromSettings.IsChecked;
		DataContext = new MainWindowViewModel();

		Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e) {
		LoadCertificates();
	}

	private void LoadCertificates() {

		var location = ViewModel.SelectedStoreLocation;
		if (App.Settings != null) {
			var issuers = App.Settings.AllowedIssuers;
			Grid.ItemsSource = CertificateService.GetCertificate(location, issuers, WillReadFromSettings);
		}
		WillReadFromSettings = ReadFromSettings.IsChecked;

		List<string> issuer = [];
		if (!String.IsNullOrEmpty(IssuerBox.Text)) {
			issuer.Add(IssuerBox.Text);
		}

		Grid.ItemsSource = CertificateService.GetCertificate(location, issuer, WillReadFromSettings);
	}

	private void OnLoad(object sender, RoutedEventArgs e) {
		var location = ViewModel.SelectedStoreLocation;
		WillReadFromSettings = ReadFromSettings.IsChecked;

		List<string> issuer = [];
		if (!String.IsNullOrEmpty(IssuerBox.Text)) {
			issuer.Add(IssuerBox.Text);
		}

		Grid.ItemsSource = CertificateService.GetCertificate(location, issuer, WillReadFromSettings);
	}

	private void OnRemove(object sender, RoutedEventArgs e) {
		if (Grid.SelectedItems is not CertificateInfo info)
			return;

		OnLoad(null, null);
	}
}
public class BoolToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		=> (value is true) ? Visibility.Visible : Visibility.Collapsed;

	public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		=> throw new NotImplementedException();
}
