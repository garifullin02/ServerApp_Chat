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

        public string _firstName {  get; set; }
        public string _userName {  get; set; }
        public string _password { get; set; }
        public bool _authorizationAndRegistrationCheck { get; set; }

        public Client(string firstName, string userName, string password, bool authorizationAndRegistrationCheck)
        {
            _firstName = firstName;
            _userName = userName;
            _password = password;
            _authorizationAndRegistrationCheck = authorizationAndRegistrationCheck;
        }
    }
}
