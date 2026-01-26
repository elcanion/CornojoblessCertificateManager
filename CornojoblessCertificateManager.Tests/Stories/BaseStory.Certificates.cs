using CornojoblessCertificateManager.Core.Services;
using CornojoblessCertificateManager.Tests.Factories;
using NSubstitute;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CornojoblessCertificateManager.Tests.Stories {
	public partial class BaseStory {
		protected X509Certificate2 Certificate;
		protected ICertificateStore CertificateStore;
		protected void TheCertificateExists() {
			Certificate = CertificateFactory.CreateWithPrivateKey();
			CertificateStore = Substitute.For<ICertificateStore>();
			CertificateStore.GetByThumbprint("").Returns(Certificate);
		}

		protected void TheCertificateHasAPrivateKey() {
			throw new NotImplementedException();
		}

		protected void TheUserBacksUpTheCertificate() {
			throw new NotImplementedException();
		}

		protected void TheBackupShouldBeCreated() {
			throw new NotImplementedException();
		}
	}
}
