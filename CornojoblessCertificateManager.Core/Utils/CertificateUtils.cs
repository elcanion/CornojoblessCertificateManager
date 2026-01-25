using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Core.Utils
{
    public static class CertificateUtils
    {
		public static bool IsExportable(X509Certificate2 cert) {
			if (!cert.HasPrivateKey)
				return false;

			try {
				_ = cert.Export(X509ContentType.Pfx, String.Empty);
				return true;
			} catch (CryptographicException) {
				return false;
			}
		}
	}
}
