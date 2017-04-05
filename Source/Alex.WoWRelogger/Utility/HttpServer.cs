using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alex.WoWRelogger.Utility
{
	internal class HttpServer
	{
		public const int SERVER_PORT = 55556;

		private TcpListener Listener;

		public static Func<string, string> Handler;

		private static HttpServer _instance;

		public HttpServer(int Port)
		{
			HttpServer tcpListener = this;
			(new Task(() => {
				tcpListener.Listener = new TcpListener(IPAddress.Loopback, Port);
				tcpListener.Listener.Start();
				while (true)
				{
					TcpClient tcpClient = tcpListener.Listener.AcceptTcpClient();
					(new Thread(new ParameterizedThreadStart(HttpServer.ClientThread))
					{
						IsBackground = true
					}).Start(tcpClient);
				}
			})).Start();
		}

		private static void ClientThread(object StateInfo)
		{
			Client client = new Client((TcpClient)StateInfo);
		}

		~HttpServer()
		{
			if (this.Listener != null)
			{
				this.Listener.Stop();
			}
		}

		public static HttpServer Get()
		{
			return HttpServer._instance;
		}

		public void SetHandler(Func<string, string> f)
		{
			HttpServer.Handler = f;
		}

		public static void Start()
		{
			HttpServer._instance = new HttpServer(55556);
		}
	}
}