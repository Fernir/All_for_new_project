using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace HighVoltz.HBRelog.Source.AntiCaptcha
{
	public class HttpHelper
	{
		public HttpHelper()
		{
		}

		public static dynamic Post(Uri url, string post)
		{
			object obj;
			object obj1 = null;
			byte[] bytes = Encoding.UTF8.GetBytes(post);
			HttpWebRequest length = (HttpWebRequest)WebRequest.Create(url);
			length.Method = "POST";
			length.ContentType = "application/json";
			length.ContentLength = (long)((int)bytes.Length);
			try
			{
				using (Stream requestStream = length.GetRequestStream())
				{
					requestStream.Write(bytes, 0, (int)bytes.Length);
					requestStream.Close();
				}
				using (HttpWebResponse response = (HttpWebResponse)length.GetResponse())
				{
					obj1 = JsonConvert.DeserializeObject((new StreamReader(response.GetResponseStream(), Encoding.UTF8)).ReadToEnd());
					response.Close();
				}
				return obj1;
			}
			catch
			{
				obj = false;
			}
			return obj;
		}
	}
}