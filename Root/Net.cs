using System;
using SIO = System.IO;
using SNet = System.Net;
using Sockets = System.Net.Sockets;
using Threading = System.Threading;
using Generics = System.Collections.Generic;

namespace Root.Net.Mail
{
	/// <summary>
	/// Allows applications to receive e-mail by using the Post Office Protocol (POP3).
	/// </summary>
	public class Pop3Client
	{
		#region Fields

		private const string success = "+OK";
		private const string error = "-ERR";
		private const int defaultPort = 110;
		private const int defaultTimeout = 300;

		private Sockets.Socket socket;
		private SNet.IPAddress[] serverIps;
		private int port;
		private SIO.StreamReader reader;
		private SIO.StreamWriter writer;
		private Generics.List<string> log;

		private Threading.Thread thStay;
		private Threading.ManualResetEvent stayCompleted;
		private Threading.ManualResetEvent operationCompleted;
		private bool useThreadEvents;

		#endregion

		#region Constructors

		private Pop3Client()
		{
			socket = new Sockets.Socket(Sockets.AddressFamily.InterNetwork, Sockets.SocketType.Stream,
				Sockets.ProtocolType.Tcp);
			log = new Generics.List<string>();
			stayCompleted = new Threading.ManualResetEvent(true);
			operationCompleted = new Threading.ManualResetEvent(true);
			useThreadEvents = false;
		}

		/// <summary>
		/// Initializes a new instance of the Root.Net.Mail.Pop3Client class that receives e-mail
		/// by using the specified POP3 server.
		/// </summary>
		/// <param name="host">A System.String that contains the name or IP address of the host computer
		/// used for POP3 transactions.</param>
		public Pop3Client(string host)
			: this (host, defaultPort)
		{
		}

		/// <summary>
		/// Initializes a new instance of the Root.Net.Mail.Pop3Client class that receives e-mail
		/// by using the specified POP3 server and port.
		/// </summary>
		/// <param name="host">A System.String that contains the name or IP address of the host computer
		/// used for POP3 transactions.</param>
		/// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
		public Pop3Client(string host, int port)
			: this()
		{
			if (host == null)
				throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "host"), (Exception)null);
			if (host.Length == 0)
				throw new ArgumentException(resExceptions.ArgumentEmptyString.Replace("%var", "host"));
			if (port < 0)
				throw new ArgumentException(resExceptions.LessThanZero.Replace("%var", "port"));

			try
			{
				SNet.IPAddress ip = SNet.IPAddress.Parse(host);
				serverIps = new SNet.IPAddress[1];
				serverIps[0] = ip;
			}
			catch
			{
				serverIps = SNet.Dns.GetHostAddresses(host);
			}
			this.port = port;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value that indicates default POP3 server port.
		/// </summary>
		public static int DefaultPort
		{
			get { return defaultPort; }
		}

		/// <summary>
		/// Gets a value that indicates whether current Pop3Client is connected
		/// to a remote host.
		/// </summary>
		public bool IsConnected
		{
			get
			{
				if (socket == null)
					return false;
				else
					return socket.Connected;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Establishes a connection to a remote host. The host is specified by a host name and a port number.
		/// </summary>
		/// <param name="user">User name to connect to POP3 host.</param>
		/// <returns>True whether current client succefully connect to host; otherwise, false.</returns>
		public bool Connect(string user)
		{
			return this.Connect(user, null);
		}

		/// <summary>
		/// Establishes a connection to a remote host. The host is specified by a host name and a port number.
		/// </summary>
		/// <param name="user">User name to connect to POP3 host.</param>
		/// <param name="password">User password to connect to POP3 host.</param>
		/// <returns>True whether current client succefully connect to host; otherwise, false.</returns>
		public bool Connect(string user, string password)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			if (socket.Connected)
				throw new Exception("Already connected");

			if (socket == null)	// occurs in systems older than Windows 2000.
				socket = new Sockets.Socket(Sockets.AddressFamily.InterNetwork, Sockets.SocketType.Stream,
				Sockets.ProtocolType.Tcp);

			socket.Connect(serverIps, port);
			Sockets.NetworkStream ns = new Sockets.NetworkStream(socket);
			reader = new SIO.StreamReader(ns, System.Text.Encoding.ASCII);
			writer = new SIO.StreamWriter(ns, System.Text.Encoding.ASCII);
			writer.AutoFlush = true;

			if (!VerifyResponse())
			{
				this.Disconnect();
				return false;
			}

			writer.WriteLine(Pop3Command.UserLogin + user);
			if (!VerifyResponse())
			{
				this.Disconnect();
				return false;
			}

			if (password != null)
			{
				writer.WriteLine(Pop3Command.Password + password);
				if (!VerifyResponse())
				{
					this.Disconnect();
					return false;
				}
			}

			writer.WriteLine(Pop3Command.Noop);
			if (!VerifyResponse())
			{
				this.Disconnect();
				return false;
			}

			useThreadEvents = false;
			thStay = new Threading.Thread(new Threading.ThreadStart(StayConnected));
			thStay.Priority = System.Threading.ThreadPriority.BelowNormal;
			thStay.IsBackground = true;
			thStay.Start();

			return true;
		}

		public int CountMessages()
		{
			if (!socket.Connected)
				throw new Exception("Not connected");

			InitializeOperation();

			writer.WriteLine(Pop3Command.MessagesStatus);
			if (!VerifyResponse())
				throw new Exception(log[log.Count - 1]);

			string ret = log[log.Count - 1];
			int spacesCount = Strings.CountOf(ret, ' ');
			int count;

			if (spacesCount > 0)
				count = int.Parse(ret.Substring(0, ret.IndexOf(' ')));
			else
				throw new Exception("#0007");

			FinalizeOperation();

			return count;
		}

		/// <summary>
		/// Closes current client connection and allows reuse of current instance.
		/// </summary>
		public void Disconnect()
		{
			if (socket == null || !socket.Connected)
				throw new Exception("Already disconnected");

			this.InitializeOperation();

			this.InternalDisconnect();
			thStay.Abort();

			this.FinalizeOperation();
		}

		public DataSize GetMessageSize(int msgNumber)
		{
			if (!socket.Connected)
				throw new Exception("Not connected");

			InitializeOperation();

			writer.WriteLine(Pop3Command.MessagesList + msgNumber.ToString());
			if (!VerifyResponse())
				throw new Exception(log[log.Count - 1]);

			string ret = log[log.Count - 1];
			int spacesCount = Strings.CountOf(ret, ' ');
			DataSize ds;

			if (spacesCount == 1)
				ds = new DataSize(long.Parse(ret.Substring(ret.IndexOf(' ') + 1)));
			else if (spacesCount == 0)
				throw new Exception("#0008");
			else
			{
				int[] idxs = Strings.AllIndexOf(ret, ' ');
				ds = new DataSize(long.Parse(ret.Substring(idxs[0] + 1, idxs[1] - (idxs[0] + 1))));
			}

			FinalizeOperation();

			return ds;
		}

		public DataSize GetAllMessagesSize()
		{
			if (!socket.Connected)
				throw new Exception("Not connected");

			InitializeOperation();

			writer.WriteLine(Pop3Command.MessagesStatus);
			if (!VerifyResponse())
				throw new Exception(log[log.Count - 1]);

			string ret = log[log.Count - 1];
			int spacesCount = Strings.CountOf(ret, ' ');
			DataSize ds;

			if (spacesCount == 1)
				ds = new DataSize(long.Parse(ret.Substring(ret.IndexOf(' ') + 1)));
			else if (spacesCount == 0)
				throw new Exception("#0009");
			else
			{
				int[] idxs = Strings.AllIndexOf(ret, ' ');
				ds = new DataSize(long.Parse(ret.Substring(idxs[0] + 1, idxs[1] - (idxs[0] + 1))));
			}

			FinalizeOperation();

			return ds;
		}

		private void InitializeOperation()
		{
			if (!stayCompleted.Reset())
				throw new Exception("#0001");
			if (!operationCompleted.Reset())
				throw new Exception("#0002");
			useThreadEvents = true;
			if (!stayCompleted.WaitOne())
				throw new Exception("#0003");
		}

		private void InternalDisconnect()
		{

			try
			{
				writer.WriteLine(Pop3Command.Quit);
				string temp = reader.ReadLine();
				log.Add(temp.Substring(temp.IndexOf(' ') + 1));
			}
			catch { }

			try { reader.Close(); }
			catch { }
			try { writer.Close(); }
			catch { }

			try
			{
				socket.Close();
				socket = null;
			}
			catch { }
		}

		private void FinalizeOperation()
		{
			useThreadEvents = false;
			if (!operationCompleted.Set())
				throw new Exception("#0004");

			if (thStay.IsAlive)
				thStay.Join(100);
		}

		private void StayConnected()
		{
			try
			{
				while (socket.Connected)
				{
					writer.WriteLine(Pop3Command.Noop);
					reader.ReadLine();
					if (useThreadEvents)
					{
						if (!stayCompleted.Set())
							throw new Exception("#0005");
						if (!operationCompleted.WaitOne())
							throw new Exception("#0006");

						if (socket == null)
							return;
					}
					Threading.Thread.Sleep(defaultTimeout);
				}
			}
			catch (Threading.ThreadAbortException)
			{
			}
			catch
			{
				InternalDisconnect();
			}
		}

		private bool VerifyResponse()
		{
			string temp = reader.ReadLine();
			log.Add(temp.Substring(temp.IndexOf(' ') + 1));
			return temp.StartsWith(success);
		}

		#endregion

		private static class Pop3Command
		{
			private const string user = "USER ";
			private const string pass = "PASS ";
			private const string stat = "STAT";
			private const string list = "LIST ";
			private const string quit = "QUIT";
			private const string noop = "NOOP";

			public static string MessagesList
			{
				get { return list; }
			}

			public static string MessagesStatus
			{
				get { return stat; }
			}

			public static string Noop
			{
				get { return noop; }
			}

			public static string Password
			{
				get { return pass; }
			}

			public static string UserLogin
			{
				get { return user; }
			}

			public static string Quit
			{
				get { return quit; }
			}
		}
	}
}
