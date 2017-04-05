using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Alex.WoWRelogger.Utility
{
	internal class Client
	{
		public Client(TcpClient Client)
		{
			string str = "";
			byte[] numArray = new byte[1024];
			do
			{
				int num = Client.GetStream().Read(numArray, 0, (int)numArray.Length);
				int num1 = num;
				if (num <= 0)
				{
					break;
				}
				str = string.Concat(str, Encoding.ASCII.GetString(numArray, 0, num1));
			}
			while (str.IndexOf("\r\n\r\n") < 0 && str.Length <= 4096);
			Match match = Regex.Match(str, "^\\w+\\s+([^\\s\\?]+)[^\\s]*\\s+HTTP/.*|");
			if (match == Match.Empty)
			{
				this.SendError(Client, 400);
				return;
			}
			string value = match.Groups[1].Value;
			value = Uri.UnescapeDataString(value);
			string str1 = "text/html";
			string handler = "";
			if (HttpServer.Handler != null)
			{
				handler = HttpServer.Handler(value);
			}
			string str2 = string.Concat(new object[] { "HTTP/1.1 200 OK\nContent-Type: ", str1, "\nContent-Length: ", handler.Length, "\n\n" });
			byte[] bytes = Encoding.ASCII.GetBytes(str2);
			byte[] bytes1 = Encoding.ASCII.GetBytes(handler);
			try
			{
				try
				{
					Client.GetStream().Write(bytes, 0, (int)bytes.Length);
					Client.GetStream().Write(bytes1, 0, (int)bytes1.Length);
				}
				catch (Exception exception)
				{
				}
			}
			finally
			{
				Client.Close();
			}
			Client.Close();
		}

		private void SendError(TcpClient Client, int Code)
		{
			HttpStatusCode code = (HttpStatusCode)Code;
			string str = string.Concat(Code.ToString(), " ", code.ToString());
			string str1 = string.Concat("<html><body><h1>", str, "</h1></body></html>");
			string[] strArrays = new string[] { "HTTP/1.1 ", str, "\nContent-type: text/html\nContent-Length:", null, null, null };
			strArrays[3] = str1.Length.ToString();
			strArrays[4] = "\n\n";
			strArrays[5] = str1;
			string str2 = string.Concat(strArrays);
			byte[] bytes = Encoding.ASCII.GetBytes(str2);
			Client.GetStream().Write(bytes, 0, (int)bytes.Length);
			Client.Close();
		}
	}
}