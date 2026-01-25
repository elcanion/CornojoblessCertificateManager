using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security;

namespace CornojoblessCertificateManager.Core.Utils
{
    public static class CertificateUtils
    {
		public static bool IsExportable(X509Certificate2 cert) {
			if (!cert.HasPrivateKey)
				return false;

			try {
				_ = cert.Export(X509ContentType.Pfx, String.Empty);
				return true;
			} catch (CryptographicException) {
				return false;
			}
		}

		public static bool ArePasswordsEqual(this SecureString ss1, SecureString ss2) {
			var bstr1 = IntPtr.Zero;
			var bstr2 = IntPtr.Zero;
			try {
				bstr1 = Marshal.SecureStringToBSTR(ss1);
				bstr2 = Marshal.SecureStringToBSTR(ss2);
				var length1 = Marshal.ReadInt32(bstr1, -4);
				var length2 = Marshal.ReadInt32(bstr2, -4);
				if (length1 == length2) {
					for (var x = 0; x < length1; ++x) {
						var b1 = Marshal.ReadByte(bstr1, x);
						var b2 = Marshal.ReadByte(bstr2, x);
						if (b1 != b2)
							return false;
					}
				} else
					return false;
				return true;
			} finally {
				if (bstr2 != IntPtr.Zero)
					Marshal.ZeroFreeBSTR(bstr2);
				if (bstr1 != IntPtr.Zero)
					Marshal.ZeroFreeBSTR(bstr1);
			}
		}
	}
}
