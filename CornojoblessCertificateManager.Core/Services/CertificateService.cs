using CornojoblessCertificateManager.Core.Model;
using System.Security.Cryptography.X509Certificates;

namespace CornojoblessCertificateManager.Core.Services
{
    public class CertificateService
    {
		public static List<CertificateInfo> GetCertificate(StoreLocation location, List<string>? allowedIssuers, bool? readFromSettings) {
			if (allowedIssuers == null) {
				return new List<CertificateInfo> { new CertificateInfo() };
			}

			using var store = new X509Store(StoreName.My, location);
			store.Open(OpenFlags.ReadOnly);

			if (allowedIssuers?.Count == 0) {
				return store.Certificates
					.Cast<X509Certificate2>()
					.Where(c => c.NotAfter < DateTime.Now)
					.Select(c => new CertificateInfo {
						Subject = c.Subject,
						Issuer = c.Issuer,
						Thumbprint = c.Thumbprint,
						Expiration = c.NotAfter,
						HasPrivateKey = c.HasPrivateKey,
					}).ToList();
			}
			
			var certificates = new List<CertificateInfo>();
			
			foreach(var issuer in allowedIssuers) {
				var query = store.Certificates
				.Cast<X509Certificate2>()
				.Where(c => c.NotAfter < DateTime.Now && c.Issuer.Contains(issuer, StringComparison.OrdinalIgnoreCase))
				.Select(c => new CertificateInfo {
					Subject = c.Subject,
					Issuer = c.Issuer,
					Thumbprint = c.Thumbprint,
					Expiration = c.NotAfter,
					HasPrivateKey = c.HasPrivateKey,
				}).FirstOrDefault();

				if (query != null) {
					certificates.Add(query);
				}
			}

			return certificates;
		}
    }
}
