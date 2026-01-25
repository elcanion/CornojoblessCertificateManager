using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Queries;
using CornojoblessCertificateManager.Core.Requests;
using CornojoblessCertificateManager.Core.Utils;
using System.Security;
using System.Security.Cryptography;
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

			if (query.OnlyExportable) {
				certs = certs.Where(c => CertificateUtils.IsExportable(c));
			}

			if (query.Issuers.Count > 0) {
				certs = certs.Where(c => 
					query.Issuers.Any(i => c.Issuer.Contains(i, StringComparison.OrdinalIgnoreCase)));
			}

			return certs.Select(c => new CertificateInfo(c)).ToList();
		}

		public void BackupCertificates(CertificateBackupRequest request) {
			Directory.CreateDirectory(request.BackupDirectory);

			using var store = new X509Store(StoreName.My, request.Location);
			store.Open(OpenFlags.ReadOnly);

			foreach (var info in request.Certificates) {
				var cert = store.Certificates
					.Find(X509FindType.FindByThumbprint, info.Thumbprint, validOnly: false)
					.Cast<X509Certificate2>()
					.FirstOrDefault();

				var fileName = $"{cert.Subject}_{cert.Thumbprint}.pfx";
				var fullPath = Path.Combine(request.BackupDirectory, fileName);

				byte[] bytes;

				try {
					bytes = cert.HasPrivateKey
						? cert.Export(X509ContentType.Pfx, request.PfxPassword)
						: cert.Export(X509ContentType.Cert);
				} catch (CryptographicException) {
					bytes = cert.Export(X509ContentType.Cert);
				}
				

				File.WriteAllBytes(fullPath, bytes);
			}
		}

		public void DeleteCertificates(CertificateDeleteRequest request) {
			throw new NotImplementedException();
		}
	}
}
