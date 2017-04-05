using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Alex.WoWRelogger.Utility
{
	internal class MyWebClient : WebClient
	{
		private System.Net.CookieContainer container = new System.Net.CookieContainer();

		public System.Net.CookieContainer CookieContainer
		{
			get
			{
				return this.container;
			}
			set
			{
				this.container = value;
			}
		}

		public MyWebClient(System.Net.CookieContainer container)
		{
			this.container = container;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			x509Store.Open(OpenFlags.ReadOnly);
			X509Certificate2 x509Certificate2 = x509Store.Certificates.OfType<X509Certificate2>().FirstOrDefault<X509Certificate2>((X509Certificate2 a) => a.Subject.Contains("WM id:"));
			if (x509Certificate2 == null)
			{
				throw new Exception("CERTIFICATE NOT FOUND !!!!!!!!");
			}
			HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(address);
			webRequest.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
			webRequest.ClientCertificates.Add(x509Certificate2);
			webRequest.CookieContainer = this.container;
			return webRequest;
		}
	}
}