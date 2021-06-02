using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    public class PremiumAccount: Account
    {
        public override event AccountStateHandler Invited;
        public PremiumAccount(string name, string password): base(name, password)
        {
        }
        protected override void OnInvited(string nameOfFriend)
        {
            if (Invited != null)
                Invited(this, new AccountEventArgs(nameOfFriend +" was sent a request and he is added to friends" , Name));
        }
    }
}
