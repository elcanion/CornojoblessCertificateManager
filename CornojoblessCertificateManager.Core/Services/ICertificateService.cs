using CornojoblessCertificateManager.Core.Model;
using CornojoblessCertificateManager.Core.Queries;
using CornojoblessCertificateManager.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Core.Services
{
    public interface ICertificateService
    {
		IReadOnlyList<CertificateInfo> GetCertificatesInfo(CertificateQuery query);
		public void BackupCertificates(CertificateBackupRequest request);
		public void DeleteCertificates(CertificateDeleteRequest request);
	}
}
