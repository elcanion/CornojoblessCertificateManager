using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CornojoblessCertificateManager.Tests.Stories;

namespace CornojoblessCertificateManager.Tests.Stories.Certificates
{
	[TestClass]
	[Story(Title = "User backs up a certificate")]
	[TestCategory("User")]
    public partial class CertificateBackup : BaseStory
    {

		[TestMethod]
		public void TheUserBacksUpACertificate() {
			this.Given(x => x.TheCertificateExists())
				.And(x => x.TheCertificateHasAPrivateKey())
				.When(x => x.TheUserBacksUpTheCertificate())
				.Then(x => x.TheBackupShouldBeCreated())
				.BDDfy();
		}

	}
}
