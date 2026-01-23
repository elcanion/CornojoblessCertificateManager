using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Queries;
using System.Security.Cryptography.X509Certificates;

namespace CornojoblessCertificateManager.Core.Services
{
    public class CertificateService : ICertificateService
    {
		public IReadOnlyList<CertificateInfo> GetCertificates(CertificateQuery query) {
			using var store = new X509Store(StoreName.My, query.StoreLocation);
			store.Open(OpenFlags.ReadOnly);

			var certs = store.Certificates.Cast<X509Certificate2>();

			if (query.OnlyExpired) {
				certs = certs.Where(c => c.NotAfter < DateTime.Now);
			}

			if (query.Issuers.Count > 0) {
				certs = certs.Where(c => 
				query.Issuers.Any(i => c.Issuer.Contains(i, StringComparison.OrdinalIgnoreCase)));
			}

			return certs.Select(c => new CertificateInfo {
				Subject = c.Subject,
				Issuer = c.Issuer,
				Thumbprint = c.Thumbprint,
				Expiration = c.NotAfter,
				HasPrivateKey = c.HasPrivateKey,
			}).ToList();
		}
    }
}
