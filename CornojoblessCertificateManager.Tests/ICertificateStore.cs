using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Tests
{
    public interface ICertificateStore {
		X509Certificate2 GetByThumbprint(string thumbprint);
    }
}
