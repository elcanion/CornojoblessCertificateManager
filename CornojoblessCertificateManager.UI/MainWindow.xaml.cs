using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Services;
using CornojoblessCertificateManager.UI.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
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
	public ObservableCollection<StoreLocation> StoreLocations { get; } = [];

	public MainWindow(MainWindowViewModel viewModel)
    {
		DataContext = viewModel;
		InitializeComponent();
	}
}

public class BoolToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		=> (value is true) ? Visibility.Visible : Visibility.Collapsed;

	public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		=> throw new NotImplementedException();
}
