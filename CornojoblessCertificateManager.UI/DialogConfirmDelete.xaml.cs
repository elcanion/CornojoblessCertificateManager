using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CornojoblessCertificateManager.UI
{
	/// <summary>
	/// Interaction logic for DialogConfirmDelete.xaml
	/// </summary>
	public partial class DialogConfirmDelete : Window
    {
		public SecureString Password => PasswordBox.SecurePassword;

		public DialogConfirmDelete() {
			InitializeComponent();
			Loaded += (_, _) => PasswordBox.Focus();
		}

		private void continueDelete(object sender, RoutedEventArgs e) {
			if (Password == null || Password.Length == 0) {
				MessageBox.Show(
					"Password cannot be empty.",
					"Certificate deletion",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);

				DialogResult = false;
				Close();
				return;
			}
			DialogResult = true;
			Close();
		}

		private void cancelDelete(object sender, RoutedEventArgs e) {
			DialogResult = false;
			Close();
		}
	}
}
