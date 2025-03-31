using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp_Chat
{
    class Server
    {
        private static IPAddress _address = IPAddress.Parse("127.0.0.1");
        private static int _port = 8888;

        public async Task StartServerAsync()
        {
            TcpListener listener = new TcpListener(_address, _port);
            listener.Start();
            Console.WriteLine("The server is running...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
            }
        }
    }

    class Program
    {

        static async Task Main(string[] args)
        {
            Server server = new Server();
            await server.StartServerAsync();
        }
    }
}
