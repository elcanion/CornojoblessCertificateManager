using CornojoblessCertificateManager.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Core.Requests
{
    public class CertificateDeleteRequest
    {
		public StoreLocation Location { get; set; }
		public IReadOnlyCollection<CertificateInfo> Certificates { get; init; } = [];
	}
}
