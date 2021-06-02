using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    class AccountException: Exception
    {
        public AccountException() : base() { }
        public AccountException(string str) : base(str) { }
        public AccountException(string str, Exception inner) : base(str, inner) { }
    }
}
