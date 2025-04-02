using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using ServerApp_Chat.Classes;

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

                Thread threadSeparatedForClient = new Thread(async () => await AuthorizationAndRegistrationClient(client));
                threadSeparatedForClient.Start();
            }
        }

        private async Task AuthorizationAndRegistrationClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedJsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Client receivedClientData = JsonConvert.DeserializeObject<Client>(receivedJsonData);

                // Work with db
                
                Console.WriteLine($"Client {receivedClientData._userName} {receivedClientData._password} connected");
                break;
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
