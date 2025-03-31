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

        private string _firstName;
        private string _userName;
        private string _password;
        private bool _authorizationAndRegistrationCheck;

        public Client(string firstName, string userName, string password, bool authorizationAndRegistrationCheck)
        {
            _firstName = firstName;
            _userName = userName;
            _password = password;
            _authorizationAndRegistrationCheck = authorizationAndRegistrationCheck;
        }
    }
}
