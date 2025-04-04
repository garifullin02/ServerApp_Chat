using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp_Chat.Classes
{
    class ClientForServer
    {
        public string _uid { get; set; }
        public TcpClient tcp { get; set; }
        public NetworkStream stream { get; set; }
    }
}
