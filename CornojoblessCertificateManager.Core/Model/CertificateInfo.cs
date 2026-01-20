using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Core.Model
{
    public class CertificateInfo
    {
        public string Subject { get; init; }
        public string Issuer { get; init; }
        public string Thumbprint { get; init; }
        public DateTime Expiration { get; init; }
    }
}
