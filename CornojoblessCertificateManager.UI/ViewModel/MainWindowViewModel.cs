using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using CornojoblessCertificateManager.Core.Services;
using CornojoblessCertificateManager.Core.Model;
using System.Windows.Input;
using CornojoblessCertificateManager.Core.Queries;

namespace CornojoblessCertificateManager.UI.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged {

		private readonly ICertificateService certificateService;
		private readonly CertificateSettings settings;

		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		public ObservableCollection<CertificateInfo> Certificates { get; } = [];
		public ObservableCollection<StoreLocation> StoreLocations { get; } = [StoreLocation.CurrentUser, StoreLocation.LocalMachine];

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

		private bool onlyExpired = true;
		public bool OnlyExpired {
			get => onlyExpired;
			set {
				onlyExpired = value;
				OnPropertyChanged();
			}
		}

		private bool readFromSettings;
		public bool ReadFromSettings {
			get => readFromSettings;
			set {
				readFromSettings = value;
				OnPropertyChanged();
			}
		}

		public bool IsAdmin { get; }
		public bool CanRemove => SelectedStoreLocation == StoreLocation.CurrentUser || IsAdmin;

		public ICommand LoadCertificatesCommand { get; }

		public MainWindowViewModel(ICertificateService certificateService, CertificateSettings settings) {
			this.certificateService = certificateService;
			this.settings = settings;

			IsAdmin = IsRunningAsAdministrator();

			SelectedStoreLocation = StoreLocation.CurrentUser;

			LoadCertificatesCommand = new RelayCommand(LoadCertificates);
		}

		private void LoadCertificates() {
			Certificates.Clear();

			var issuers = new List<string>();
			if (ReadFromSettings) {
				issuers.AddRange(settings.AllowedIssuers);
			}

			var query = new CertificateQuery {
				StoreLocation = SelectedStoreLocation,
				Issuers = issuers,
				OnlyExpired = OnlyExpired
			};

			var result = certificateService.GetCertificates(query);

			foreach(var cert in result) {
				Certificates.Add(cert);
			}
		}

		public static bool IsRunningAsAdministrator() {
			using var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);

			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
