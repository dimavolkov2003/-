using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    public interface IAccount
    {
        void SendInvitations(string name, UserStatus userStatus);
        void AddFriend(string name, UserStatus userStatus);
        void DeleteFriend(string name);
    }
}
