using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Core.Queries
{
	public class CertificateQuery
	{
		public StoreLocation StoreLocation { get; init; }
		public IReadOnlyCollection<string> Issuers { get; init; } = [];
		public bool OnlyExpired { get; set; }

	}
}
