using Alex.WoWRelogger;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using xNet;

namespace Alex.WoWRelogger.Utility
{
	internal class AccountTools
	{
        /// <summary>
        /// Список куки
        /// </summary>
		private CookieDictionary CC = new CookieDictionary(false);

        /// <summary>
        /// Прокси
        /// </summary>
		private Socks5ProxyClient Proxy;

        /// <summary>
        /// Список прокси
        /// </summary>
		private static string[] ProxyList;

		private X509Certificate2 webmoneyCert;

		private MyWebClient WebmoneyClient = new MyWebClient(new CookieContainer());

        /// <summary>
        /// Инициализация списка прокси
        /// </summary>
		static AccountTools()
		{
			if (!File.Exists("proxy.txt"))
			{
				ProxyList = new string[0];
				return;
			}
			ProxyList = (
                from a in File.ReadAllLines("proxy.txt") select a.Substring(0, a.IndexOf(':'))).ToArray();
		}

		public AccountTools()
		{
		}

        /// <summary>
        /// изменить прокси
        /// </summary>
		public void ChangeProxy()
		{
			if (ProxyList.Length == 0)
			{
				Proxy = null;
				return;
			}
			Proxy = new Socks5ProxyClient(ProxyList[HbRelogManager.r.Next(0, ProxyList.Length - 1)], 1080, "god_912", "legudasy");
		}

        /// <summary>
        /// очистить куки
        /// </summary>
		public void ClearCookies()
		{
			CC.Clear();
		}

        /// <summary>
        /// получить прокси
        /// </summary>
        /// <returns></returns>
		public string GetProxy()
		{
		    return Proxy == null ? "" : string.Concat(Proxy.Host, ":", Proxy.Port.ToString());
		}

        /// <summary>
        /// Зайти на страницу и считать ее в поток
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
	    public Stream GetSiteAsStream(string url)
		{
			MemoryStream memoryStream;
			if (this.Proxy == null)
			{
				this.ChangeProxy();
			}
			using (HttpRequest httpRequest = new HttpRequest())
			{
				httpRequest.UserAgent = Http.FirefoxUserAgent();
				httpRequest.CharacterSet = new UTF8Encoding();
				if (File.Exists("proxy.txt"))
				{
					httpRequest.Proxy = this.Proxy;
				}
				httpRequest.Cookies = this.CC;
				httpRequest.AllowAutoRedirect = true;
				httpRequest.Culture = new CultureInfo("ru-RU");
				httpRequest.MaximumAutomaticRedirections = 1000;
				memoryStream = httpRequest.Get(url, null).ToMemoryStream();
			}
			return memoryStream;
		}

        /// <summary>
        /// Зайти на страницу и считать ее в память в виде строкового значения
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ajax">ajax запрос</param>
        /// <param name="referer"></param>
        /// <param name="autoRedirect"></param>
        /// <returns></returns>
		public string GetSiteAsString(string url, bool ajax = false, string referer = null, bool autoRedirect = true)
		{
			if (this.Proxy == null)
			{
				this.ChangeProxy();
			}
			var str = "";
			using (var httpRequest = new HttpRequest())
			{
				httpRequest.UserAgent = Http.FirefoxUserAgent();
				httpRequest.CharacterSet = new UTF8Encoding();
				httpRequest.Proxy = Proxy;
				httpRequest.AllowAutoRedirect = false;
				httpRequest.Cookies = CC;
				httpRequest.Culture = new CultureInfo("ru-RU");
				httpRequest.MaximumAutomaticRedirections = 1000;
				if (ajax)
				{
					httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
				}
				if (!string.IsNullOrEmpty(referer))
				{
					httpRequest.Referer = referer;
				}
				if (autoRedirect)
				{
					httpRequest.AllowAutoRedirect = true;
				}
				httpRequest.IgnoreProtocolErrors = true;
				str = httpRequest.Get(url, null).ToString();
			}
			return str;
		}

        /// <summary>
        /// Зайти на страницу и считать ее в память в HttpResponse
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ajax"></param>
        /// <param name="referer"></param>
        /// <param name="autoRedirect"></param>
        /// <returns></returns>
		public HttpResponse GetSiteRaw(string url, bool ajax = false, string referer = null, bool autoRedirect = true)
		{
			if (this.Proxy == null)
			{
				this.ChangeProxy();
			}
			HttpResponse httpResponse = null;
			using (HttpRequest httpRequest = new HttpRequest())
			{
				httpRequest.UserAgent = Http.FirefoxUserAgent();
				httpRequest.CharacterSet = new UTF8Encoding();
				httpRequest.Proxy = this.Proxy;
				httpRequest.AllowAutoRedirect = false;
				httpRequest.Cookies = new CookieDictionary(); // this.CC;
				httpRequest.Culture = new CultureInfo("ru-RU");
				httpRequest.MaximumAutomaticRedirections = 1000;
				if (ajax)
				{
					httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
				}
				if (!string.IsNullOrEmpty(referer))
				{
					httpRequest.Referer = referer;
				}
				if (autoRedirect)
				{
					httpRequest.AllowAutoRedirect = true;
				}
				httpRequest.IgnoreProtocolErrors = true;
				httpResponse = httpRequest.Get(url, null);
			}
			return httpResponse;
		}

		public byte[] GetWebmoneyPicture(string url)
		{
			CookieContainer cookieContainer = new CookieContainer();
			return this.WebmoneyClient.DownloadData(url);
		}

        /// <summary>
        /// Отправить Post запрос с данными
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">данные запроса</param>
        /// <param name="ajax"></param>
        /// <param name="referer"></param>
        /// <param name="autoRedirect"></param>
        /// <returns></returns>
		public string PostSite(string url, string data, bool ajax = false, string referer = null, bool autoRedirect = true)
		{
			List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
			string[] strArrays = data.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string[] strArrays1 = strArrays[i].Split(new char[] { '=' });
				string str = strArrays1[0];
				keyValuePairs.Add(new KeyValuePair<string, string>(str, strArrays1[1]));
			}
			return this.PostSite(url, keyValuePairs, ajax, referer, autoRedirect);
		}

        public HttpResponse PostSiteRaw(string url, string data, bool ajax = false, string referer = null, bool autoRedirect = true)
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            string[] strArrays = data.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string[] strArrays1 = strArrays[i].Split(new char[] { '=' });
                string str = strArrays1[0];
                keyValuePairs.Add(new KeyValuePair<string, string>(str, strArrays1[1]));
            }
            return this.PostSiteRaw(url, keyValuePairs, ajax, referer, autoRedirect);
        }

        /// <summary>
        /// Отправить Post запрос с данными
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">данные</param>
        /// <param name="ajax"></param>
        /// <param name="referer"></param>
        /// <param name="autoRedirect"></param>
        /// <returns></returns>
		public string PostSite(string url, List<KeyValuePair<string, string>> data, bool ajax = false, string referer = null, bool autoRedirect = true)
		{
			string str = "";
			if (this.Proxy == null)
			{
				this.ChangeProxy();
			}
			using (HttpRequest httpRequest = new HttpRequest())
			{
				httpRequest.UserAgent = Http.FirefoxUserAgent();
				httpRequest.CharacterSet = new UTF8Encoding();
				if (File.Exists("proxy.txt"))
				{
					httpRequest.Proxy = this.Proxy;
				}
				httpRequest.Cookies = this.CC;
				httpRequest.Culture = new CultureInfo("ru-RU");
				httpRequest.MaximumAutomaticRedirections = 1000;
				httpRequest.AllowAutoRedirect = false;
                if (ajax)
				{
					httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");

				}
				if (!string.IsNullOrEmpty(referer))
				{
					httpRequest.Referer = referer;
				}
				if (autoRedirect)
				{
					httpRequest.AllowAutoRedirect = true;
				}
				foreach (KeyValuePair<string, string> datum in data)
				{
					httpRequest.AddParam(datum.Key, datum.Value);
				}
				var t = httpRequest.ToString();
                str = httpRequest.Post(url).ToString();
			}
			return str;
		}

        public HttpResponse PostSiteRaw(string url, List<KeyValuePair<string, string>> data, bool ajax = false, string referer = null, bool autoRedirect = true)
        {
            if (this.Proxy == null)
            {
                this.ChangeProxy();
            }
            using (HttpRequest httpRequest = new HttpRequest())
            {
                httpRequest.UserAgent = Http.FirefoxUserAgent();
                httpRequest.CharacterSet = new UTF8Encoding();
                if (File.Exists("proxy.txt"))
                {
                    httpRequest.Proxy = this.Proxy;
                }
                httpRequest.Cookies = this.CC;
                httpRequest.Culture = new CultureInfo("ru-RU");
                httpRequest.MaximumAutomaticRedirections = 1000;
                httpRequest.AllowAutoRedirect = false;
                if (ajax)
                {
                    httpRequest.AddHeader("X-Requested-With", "XMLHttpRequest");

                }
                if (!string.IsNullOrEmpty(referer))
                {
                    httpRequest.Referer = referer;
                }
                if (autoRedirect)
                {
                    httpRequest.AllowAutoRedirect = true;
                }
                foreach (KeyValuePair<string, string> datum in data)
                {
                    httpRequest.AddParam(datum.Key, datum.Value);
                }
                return httpRequest.Post(url);
            }
        }

		public string PostWebmoney(string url, List<KeyValuePair<string, string>> data)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			data.ForEach((KeyValuePair<string, string> a) => nameValueCollection.Add(a.Key, a.Value));
			return Encoding.GetEncoding("WINDOWS-1251").GetString(this.WebmoneyClient.UploadValues(url, nameValueCollection));
		}
	}
}