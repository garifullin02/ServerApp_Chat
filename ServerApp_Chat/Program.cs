using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using ServerApp_Chat.Classes;

namespace ServerApp_Chat
{
    class Server
    {
        private IPAddress _address = IPAddress.Parse("127.0.0.1");
        private int _port = 8888;

        private List<ClientForServer> _clients = new List<ClientForServer>();

        public async Task StartServerAsync()
        {
            TcpListener listener = new TcpListener(_address, _port);
            listener.Start();
            Console.WriteLine("The server is running...");

            while (true)
            {
                // Ожидаем подключения клиента
                TcpClient client = await listener.AcceptTcpClientAsync();

                // Открываем отдельный поток клиенту
                Thread threadSeparatedForClient = new Thread(async () => await AuthorizationAndRegistrationClient(new ClientForServer() { tcp = client, stream = client.GetStream() }));
                threadSeparatedForClient.Start();
            }
        }

        private async Task AuthorizationAndRegistrationClient(ClientForServer client)
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await client.stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedJsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Client receivedClientData = JsonConvert.DeserializeObject<Client>(receivedJsonData);

                // Work with db

                if (receivedClientData._authorizationOrRegistrationCheck == true)
                {
                    // Оправляем данные что пользователь есть
                    buffer = Encoding.UTF8.GetBytes("yes");
                    await client.stream.WriteAsync(buffer, 0, buffer.Length);

                    lock (this) { _clients.Add(new ClientForServer() { _uid = receivedClientData._uid }); }

                    Console.WriteLine($"Client {receivedClientData._userName} uid:{receivedClientData._uid} connected");
                    break;
                }
                else
                {
                    // Оправляем данные что пользователь нету
                    buffer = Encoding.UTF8.GetBytes("no");
                    await client.stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }

            Thread threadAction = new Thread(async () => { while (true) { await ReceivingAction(client); } });
            threadAction.Start();
        }

        private async Task ReceivingAction(ClientForServer client)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await client.stream.ReadAsync(buffer, 0, buffer.Length);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if(receivedData == "personals")
            {
                await SendDataPersonal(client);
            }
            else
            {
                buffer = new byte[1024];
                bytesRead = await client.stream.ReadAsync(buffer, 0, buffer.Length);
                string receivedJsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Message receivedClientData = JsonConvert.DeserializeObject<Message>(receivedJsonData);
                Console.WriteLine(receivedClientData._message);
                await HandleClient(client);
            }
            Console.WriteLine("action");
        }

        private async Task SendDataPersonal(ClientForServer client)
        {
            List<Client> clients = new List<Client>();
            foreach (ClientForServer clientForServer in _clients)
            {
                if (clientForServer != client)
                {
                    clients.Add(new Client() {_uid = clientForServer._uid, _userName = "personal" });
                }
            }

            string jsonData = JsonConvert.SerializeObject(clients);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

            await client.stream.WriteAsync(buffer, 0, buffer.Length);
        }

        //private async Task SendDataGroup(ClientForServer client)
        //{
        //    // Work with db

        //    List<Group> groups = new List<Group>()
        //    {
        //        new Group(){ _name = "group1" },
        //        new Group(){ _name = "group2" },
        //        new Group(){ _name = "group2" }
        //    };

        //    string jsonData = JsonConvert.SerializeObject(groups);
        //    byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

        //    await client.stream.WriteAsync(buffer, 0, buffer.Length);

        //    await SendDataPersonal(client);
        //}

        //private async Task SendDataPersonal(ClientForServer client)
        //{

        //    // Work with db

        //    List<PersonalChat> groups = new List<PersonalChat>()
        //    {
        //        new PersonalChat(){ 
        //    };

        //    string jsonData = JsonConvert.SerializeObject(groups);
        //    byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

        //    await client.stream.WriteAsync(buffer, 0, buffer.Length);

        //    Thread newT = new Thread(async () => await HandleClient(client));
        //    newT.Start();
        //}

        private async Task HandleClient(ClientForServer client)
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await client.stream.ReadAsync(buffer, 0, buffer.Length);

                foreach (var clientOther in _clients)
                {
                    if (clientOther != client)
                    {
                        await clientOther.stream.WriteAsync(buffer, 0, bytesRead);
                    }
                }
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
    class Message
    {
        public string _groupOrPersonal { get; set; }
        public string _uid { get; set; }
        public string _uidClient { get; set; }
        public string _message { get; set; }

    }
}
