using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace CornojoblessCertificateManager.UI.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		private StoreLocation selectedStoreLocation;
		public StoreLocation SelectedStoreLocation {
			get => selectedStoreLocation;
			set {
				if (selectedStoreLocation != value) {
					selectedStoreLocation = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(CanRemove));
				}
			}
		}
		public bool IsAdmin { get; }
		public bool CanRemove => SelectedStoreLocation == StoreLocation.CurrentUser || IsAdmin;
		public ObservableCollection<StoreLocation> StoreLocations { get; }

		public MainWindowViewModel() {
			IsAdmin = IsRunningAsAdministrator();

			StoreLocations = new ObservableCollection<StoreLocation>
			{
				StoreLocation.CurrentUser,
				StoreLocation.LocalMachine
			};

			SelectedStoreLocation = StoreLocation.CurrentUser;
		}

		public static bool IsRunningAsAdministrator() {
			using var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);

			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
