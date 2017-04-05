namespace nManager.Helpful
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;

    public class PostFile
    {
        public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string str = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
            byte[] buffer = Encoding.ASCII.GetBytes("--" + str + "\r\n");
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            NameValueCollection c = new NameValueCollection();
            c.Add("Accepts-Language", "en-us,en;q=0.5");
            request.Headers.Add(c);
            request.ContentType = "multipart/form-data; boundary=" + str;
            Stream requestStream = request.GetRequestStream();
            bool flag = true;
            string format = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string str3 in nvc.Keys)
            {
                if (flag)
                {
                    requestStream.Write(buffer, 0, buffer.Length);
                    flag = false;
                }
                else
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                string str4 = string.Format(format, str3, nvc[str3]);
                byte[] buffer3 = Encoding.UTF8.GetBytes(str4);
                requestStream.Write(buffer3, 0, buffer3.Length);
            }
            requestStream.Write(bytes, 0, bytes.Length);
            string str5 = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string s = string.Format(str5, paramName, new FileInfo(file).Name, contentType);
            byte[] buffer4 = Encoding.UTF8.GetBytes(s);
            requestStream.Write(buffer4, 0, buffer4.Length);
            FileStream stream2 = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer5 = new byte[0x1000];
            int count = 0;
            while ((count = stream2.Read(buffer5, 0, buffer5.Length)) != 0)
            {
                requestStream.Write(buffer5, 0, count);
            }
            stream2.Close();
            byte[] buffer6 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
            requestStream.Write(buffer6, 0, buffer6.Length);
            requestStream.Close();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                request = null;
                return reader.ReadToEnd().Trim();
            }
            catch (Exception)
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                request = null;
                return "";
            }
        }
    }
}

