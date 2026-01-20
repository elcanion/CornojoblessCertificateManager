using CornojoblessCertificateManager.Core.Model;
using System.Security.Cryptography.X509Certificates;

namespace CornojoblessCertificateManager.Core.Services
{
    public class CertificateService
    {
		public static List<CertificateInfo> GetCertificate(StoreLocation location, string allowedIssuer) {
			using var store = new X509Store(StoreName.My, location);
			store.Open(OpenFlags.ReadOnly);

			return store.Certificates
				.Cast<X509Certificate2>()
				.Where(c => c.NotAfter < DateTime.Now && c.Issuer.Contains(allowedIssuer, StringComparison.OrdinalIgnoreCase))
				.Select(c => new CertificateInfo {
					Subject = c.Subject,
					Issuer = c.Issuer,
					Thumbprint = c.Thumbprint,
					Expiration = c.NotAfter,
					HasPrivateKey = c.HasPrivateKey,
				}).ToList();
		}
    }
}
