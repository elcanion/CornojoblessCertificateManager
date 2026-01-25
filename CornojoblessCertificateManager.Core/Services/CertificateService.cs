using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Queries;
using CornojoblessCertificateManager.Core.Requests;
using CornojoblessCertificateManager.Core.Utils;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CornojoblessCertificateManager.Core.Services
{
    public class CertificateService : ICertificateService
    {
		public IReadOnlyList<CertificateInfo> GetCertificatesInfo(CertificateQuery query) {
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
			using var store = new X509Store(StoreName.My, request.Location);
			store.Open(OpenFlags.ReadWrite);

			foreach (var info in request.Certificates) {
				var x509cert = store.Certificates
					.Find(X509FindType.FindByThumbprint, info.Thumbprint, validOnly: false)
					.Cast<X509Certificate2>()
					.FirstOrDefault();

				if (x509cert != null) {
					if (x509cert.HasPrivateKey) {
						using var rsa = x509cert.GetRSAPrivateKey();

						if (rsa is RSACng rsaCng) {
							rsaCng.Key.Delete();

						} else if (rsa is RSACryptoServiceProvider rsaCsp && OperatingSystem.IsWindows()) {
							var csp = rsaCsp.CspKeyContainerInfo;

							if (!csp.Removable) {
								var parameters = new CspParameters {
									ProviderType = csp.ProviderType,
									KeyContainerName = csp.KeyContainerName,
									Flags = CspProviderFlags.UseExistingKey | CspProviderFlags.NoPrompt,
								};

								using var provider = new RSACryptoServiceProvider(parameters);
								provider.PersistKeyInCsp = false;
								provider.Clear();
							}
						}
					}

					store.Remove(x509cert);
				}

			}
			store.Close();
		}
	}
}
