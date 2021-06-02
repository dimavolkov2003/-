using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    public class StandardAccount : Account
    {
        public override event AccountStateHandler Invited;
        public override event AccountStateHandler Added;

        protected string[] _listWaiting;
        public string[] ListWaiting => _listWaiting;
        public StandardAccount(string name, string password) : base(name, password)
        {
        }
        protected override void OnInvited(string nameOfFriend)
        {
            if (Invited != null)
                Invited(this, new AccountEventArgs(nameOfFriend + " was sent a request and he was added to the waiting list", Name));
        }
        public override void AddFriend(string name, UserStatus userStatus)
        {
            if (IsInvited(name) || IsWaiting(name))
            {
                switch (userStatus)
                {
                    case UserStatus.Host:
                        CancelInvitations(name);
                        IncreaseFriendsList(name);
                        if (Added != null)
                            Added(this, new AccountEventArgs(name + " was added to the list of friends", Name));
                        break;
                    case UserStatus.Recipient:
                        CancelWaiting(name);
                        IncreaseFriendsList(name);
                        break;
                }
            }
        }
        public override void SendInvitations(string name, UserStatus userStatus)
        {
            switch (userStatus)
            {
                case UserStatus.Host:
                    AddToWait(name);
                    OnInvited(name);
                    break;
                case UserStatus.Recipient:
                    AddToInvitations(name);
                    break;
            }
        }
        private void AddToWait(string name)
        {
            if (!IsInFriends(name))
            {
                if (_listWaiting != null)
                {
                    if (!IsWaiting(name))
                    {
                        string[] tempListWaiting = new string[_listWaiting.Length + 1];
                        for (int i = 0; i < _listWaiting.Length; i++)
                            tempListWaiting[i] = _listWaiting[i];
                        tempListWaiting[tempListWaiting.Length - 1] = name;
                        _listWaiting = tempListWaiting;
                    }
                        
                }
                else
                {
                    _listWaiting = new string[1];
                    _listWaiting[0] = name;
                }
            }
        }
        
        private void CancelWaiting(string name)
        {
            if (_listWaiting != null)
            {
                string[] newListWaiting = new string[_listWaiting.GetLength(0) - 1];
                int newNumberOfWaiting = 0;
                for (int i = 0; i < _listWaiting.GetLength(0); i++)
                {
                    if (_listWaiting[i] != name)
                    {
                        newListWaiting[newNumberOfWaiting] = _listWaiting[i];
                        newNumberOfWaiting++;
                    }
                }
                _listWaiting = newListWaiting;
            }

        }
        protected bool IsWaiting(string name)
        {
            bool isWaiting = false;
            for (int i = 0; i < _listWaiting.Length; i++)
            {
                if (_listWaiting[i] == name)
                   isWaiting = true;    
            }
            return isWaiting;
        }

    }
}
