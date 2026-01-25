using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using CornojoblessCertificateManager.Core.Services;
using CornojoblessCertificateManager.Core.Model;
using System.Windows.Input;
using CornojoblessCertificateManager.Core.Queries;
using CornojoblessCertificateManager.Core.Requests;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.DirectoryServices.ActiveDirectory;

namespace CornojoblessCertificateManager.UI.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged {

		private readonly ICertificateService certificateService;
		private readonly CertificateSettings settings;

		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		public ObservableCollection<CertificateInfo> Certificates { get; } = [];
		public ObservableCollection<StoreLocation> StoreLocations { get; } = [StoreLocation.CurrentUser, StoreLocation.LocalMachine];
		public ObservableCollection<CertificateInfo> SelectedCertificates { get; set; } = [];

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

		private bool onlyExportable = true;
		public bool OnlyExportable {
			get => onlyExportable;
			set {
				onlyExportable = value;
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

		private string issuerInput = string.Empty;
		public string IssuerInput {
			get => issuerInput;
			set {
				issuerInput = value;
				OnPropertyChanged();
			}
		}

		public bool IsAdmin { get; }
		public bool CanRemove => SelectedStoreLocation == StoreLocation.CurrentUser || IsAdmin;

		public ICommand LoadCertificatesCommand { get; }
		public ICommand BackupSelectedCertificatesCommand { get; }

		public MainWindowViewModel(ICertificateService certificateService, CertificateSettings settings) {
			this.certificateService = certificateService;
			this.settings = settings;

			IsAdmin = IsRunningAsAdministrator();

			SelectedStoreLocation = StoreLocation.CurrentUser;

			LoadCertificatesCommand = new RelayCommand(loadCertificates);
			BackupSelectedCertificatesCommand = new RelayCommand(backupCertificates);
		}

		private void backupCertificates() {
			var backupAndRemoveDialog = new DialogConfirmBackupAndRemove {
			};

			bool? result = backupAndRemoveDialog.ShowDialog();
			if (result == true) {
				var request = new CertificateBackupRequest {
					BackupDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
					"CertificateBackups",
					DateTime.Now.ToString("yyyMMdd_HHmmss")),
					Location = SelectedStoreLocation,
					Certificates = SelectedCertificates,
					PfxPassword = backupAndRemoveDialog.Password,
				};

				certificateService.BackupCertificates(request);

				MessageBox.Show("", "Backup finished", MessageBoxButton.OK);
			}
		}

		private void loadCertificates() {
			Certificates.Clear();

			var issuers = new List<string>();
			if (ReadFromSettings) {
				issuers.AddRange(settings.AllowedIssuers);
			}

			if (!string.IsNullOrWhiteSpace(IssuerInput)) {
				issuers.Add(IssuerInput);
			}

			var query = new CertificateQuery {
				StoreLocation = SelectedStoreLocation,
				Issuers = issuers,
				OnlyExpired = OnlyExpired,
				OnlyExportable = OnlyExportable
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
