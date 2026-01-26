using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Tests.Factories
{
    public static class CertificateFactory
    {
		public static X509Certificate2 CreateWithPrivateKey(string subject = "CN=localhost", string password = "test") {
			using var rsa = RSA.Create(2048);

			var request = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

			//TODO parameterize this better
			request.CertificateExtensions.Add(new X509KeyUsageExtension(
				X509KeyUsageFlags.DigitalSignature,
				critical: true
			));

			request.CertificateExtensions.Add(new X509BasicConstraintsExtension(
				certificateAuthority: false,
				hasPathLengthConstraint: false,
				pathLengthConstraint: 0,
				critical: true
			));

			request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(
				request.PublicKey,
				critical: false
			));

			var x509Certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
			return new X509Certificate2(x509Certificate.Export(X509ContentType.Pfx), password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
		}
    }
}
