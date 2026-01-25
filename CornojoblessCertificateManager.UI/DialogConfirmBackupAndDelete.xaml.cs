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
    /// Interaction logic for DialogConfirmBackupAndDelete.xaml
    /// </summary>
    public partial class DialogConfirmBackupAndDelete : Window
    {
		public SecureString Password => PasswordBox.SecurePassword;

		public DialogConfirmBackupAndDelete() {
			InitializeComponent();
			Loaded += (_, _) => PasswordBox.Focus();
		}

		private void continueBackup(object sender, RoutedEventArgs e) {
			DialogResult = true;
			Close();
		}

		private void cancelBackup(object sender, RoutedEventArgs e) {
			DialogResult = false;
			Close();
		}
	}
}
