using CornojoblessCertificateManager.Core.Utils;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CornojoblessCertificateManager.Core.Model
{
	public class CertificateInfo
	{
		public string Subject { get; init; }
		public string Issuer { get; init; }
		public string Thumbprint { get; init; }
		public bool HasPrivateKey { get; init; }
		public DateTime Expiration { get; init; }
		public bool IsExpired => Expiration < DateTime.UtcNow;
		public bool IsExportable { get; }

		public CertificateInfo(X509Certificate2 certificate) {
			Subject = certificate.Subject;
			Issuer = certificate.Issuer;
			Thumbprint = certificate.Thumbprint;
			HasPrivateKey = certificate.HasPrivateKey;
			Expiration = certificate.NotAfter;
			HasPrivateKey = certificate.HasPrivateKey;
			IsExportable = CertificateUtils.IsExportable(certificate);
		}
	}
}
