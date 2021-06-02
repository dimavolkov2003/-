using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);

    public class AccountEventArgs
    {
        public string Message { get; private set; }
        public string Name { get; private set; }
        public AccountEventArgs(string message, string name)
        {
            Message = message;
            Name = name;
        }
    }
}
