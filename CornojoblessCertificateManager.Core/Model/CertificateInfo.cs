namespace CornojoblessCertificateManager.Core.Model
{
    public class CertificateInfo
    {
		public string Subject { get; init; }
		public string Issuer { get; init; }
		public string Thumbprint { get; init; }
		public bool HasPrivateKey { get; init; }
		public DateTime Expiration { get; init; }

    }
}
