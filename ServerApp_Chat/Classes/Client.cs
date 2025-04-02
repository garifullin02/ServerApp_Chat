using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp_Chat.Classes
{
    class Client
    {
        // _RegistrationCheck = 0
        // _AuthorizationCheck = 1

        public string _uid { get; set; }
        public string _firstName {  get; set; }
        public string _userName {  get; set; }
        public string _password { get; set; }
        public bool _authorizationAndRegistrationCheck { get; set; }
    }
}
