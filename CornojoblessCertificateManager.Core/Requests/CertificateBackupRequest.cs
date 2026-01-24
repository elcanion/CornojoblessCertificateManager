using CornojoblessCertificateManager.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Core.Requests
{
    public class CertificateBackupRequest
    {
		public StoreLocation Location { get; init; }
		public IReadOnlyCollection<CertificateInfo> Certificates { get; init; } = [];
		public string BackupDirectory { get; init; } = string.Empty;
		public SecureString? PfxPassword { get; init; } = null;
    }
}
