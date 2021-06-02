using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum UserStatus
{
    Host,
    Recipient
}
namespace SocialNetworkLibrary
{
    public abstract class Account : IAccount
    {
        public abstract event AccountStateHandler Invited;
        public virtual event AccountStateHandler Added;
        public virtual event AccountStateHandler Deleted;

        protected string _name;
        protected string _password;
        protected string[] _listFriend;
        protected string[] _listInvitations;
        protected string[] _listOfRecommendations;
        public string Name => _name;
        public string Password => _password;
        public string[] ListFriend => _listFriend;
        public string[] ListInvitations => _listInvitations;
        public string[] ListOfRecommendations => _listOfRecommendations;
        public Account(string name, string password)
        {
            if (name != null || password != null)
            {
                if (name.Length >= 3)
                    _name = name;
                else
                    throw new AccountException("The name is less than three characters long");
                if (password.Length >= 1)
                    _password = password;
                else
                    throw new AccountException("Password is less than four characters long");
            }
            else
                throw new NullReferenceException();
        }
        protected abstract void OnInvited(string nameOfFriend);
        public virtual void AddFriend(string name, UserStatus userStatus)
        {
            if(IsInvited(name)){
                switch (userStatus)
                {
                    case UserStatus.Host:
                        CancelInvitations(name);

                        if(!IsInFriends(name))
                            IncreaseFriendsList(name);
                            if (Added != null)
                                Added(this, new AccountEventArgs(name + " was added to the list of friends", Name));
                        break;
                    case UserStatus.Recipient:
                        IncreaseFriendsList(name);
                        break;
                }
            }   
        }
        public void AddToRecommendations(string name)
        {
            if (!IsInFriends(name) && !IsInRecommendations(name))
            {
                if (_listOfRecommendations != null)
                {
                    string[] tempListOfRecommendations = new string[_listOfRecommendations.Length + 1];
                    for (int i = 0; i < _listOfRecommendations.Length; i++)
                        tempListOfRecommendations[i] = _listOfRecommendations[i];
                    tempListOfRecommendations[tempListOfRecommendations.Length - 1] = name;
                    _listOfRecommendations = tempListOfRecommendations;
                }
                else
                {
                    _listOfRecommendations = new string[1];
                    _listOfRecommendations[0] = name;
                }
            }
        }
        public void DeleteFriend(string name)
        {
            if (IsInFriends(name))
            {
                string[] newListFriend = new string[_listFriend.GetLength(0) - 1];
                int newNumberOfFriends = 0;
                for (int i = 0; i < _listFriend.GetLength(0); i++)
                {
                    if (name != _listFriend[i])
                    {
                        newListFriend[newNumberOfFriends] = _listFriend[i];
                        newNumberOfFriends++;
                    }
                }
                _listFriend = newListFriend;
                if (Deleted != null)
                    Deleted(this, new AccountEventArgs(name + " was removed from the friends list of " + this.Name, Name));
            }
            else if (IsInvited(name))
            {
                CancelInvitations(name);
                if (Deleted != null)
                    Deleted(this, new AccountEventArgs(name + " was removed from the list of invitations to " + this.Name, Name));
            }      
        }
        public virtual void SendInvitations(string name, UserStatus userStatus)
        {
            switch (userStatus)
            {
                case UserStatus.Host:
                    if (!IsInFriends(name))
                        IncreaseFriendsList(name);
                        OnInvited(name);
                    break;
                case UserStatus.Recipient:
                    AddToInvitations(name);
                    break;
            }
        }
        protected void AddToInvitations(string name)
        {
            if (!IsInFriends(name) && !IsInvited(name))
            {
                if (_listInvitations != null)
                {
                    string[] tempListInvitations = new string[_listInvitations.Length + 1];
                    for (int i = 0; i < _listInvitations.Length; i++)
                        tempListInvitations[i] = _listInvitations[i];
                    tempListInvitations[tempListInvitations.Length - 1] = name;
                    _listInvitations = tempListInvitations;
                }
                else
                {
                    _listInvitations = new string[1];
                    _listInvitations[0] = name;
                }
            }
        }
        protected void CancelInvitations(string name)
        {
            if (_listInvitations != null)
            {
                string[] newListInvitations = new string[_listInvitations.GetLength(0) - 1];
                int newNumberOfInvitations = 0;
                for (int i = 0; i < _listInvitations.GetLength(0); i++)
                {
                    if (_listInvitations[i] != name)
                    {
                        newListInvitations[newNumberOfInvitations] = _listInvitations[i];
                        newNumberOfInvitations++;
                    }
                }
                _listInvitations = newListInvitations;
            }
        }
        protected void RemoveFromRecommendations(string name)
        {
            if (_listOfRecommendations != null)
            {
                string[] newListOfRecommendations = new string[_listOfRecommendations.GetLength(0) - 1];
                int newNumberOfRecommendations = 0;
                for (int i = 0; i < _listOfRecommendations.GetLength(0); i++)
                {
                    if (_listOfRecommendations[i] != name)
                    {
                        newListOfRecommendations[newNumberOfRecommendations] = _listOfRecommendations[i];
                        newNumberOfRecommendations++;
                    }
                }
                _listOfRecommendations = newListOfRecommendations;
            }
        }
        protected bool IsInFriends(string name)
        {
            bool isInFriends = false;
            if (_listFriend != null)
            {
                for (int i = 0; i < _listFriend.Length; i++)
                {
                    if (_listFriend[i] == name)
                    {
                        isInFriends = true;
                    }
                }
            }
            return isInFriends;
        }
        protected bool IsInvited(string name)
        {
            bool isInvited = false;
            if(_listInvitations != null)
            {
                for (int i = 0; i < _listInvitations.Length; i++)
                {
                    if (_listInvitations[i] == name)
                    {
                        isInvited = true;
                    }
                }
            }
            return isInvited;
        }
        protected bool IsInRecommendations(string name)
        {
            bool isInRecommendations = false;
            if (_listOfRecommendations != null)
            {
                for (int i = 0; i < _listOfRecommendations.Length; i++)
                {
                    if (_listOfRecommendations[i] == name)
                    {
                        isInRecommendations = true;
                    }
                }
            }
            return isInRecommendations;
        }
        protected void IncreaseFriendsList(string name)
        {
            if(_listFriend != null)
            {
                string[] tempListFriend = new string[_listFriend.Length + 1];
                for (int i = 0; i < _listFriend.Length; i++)
                    tempListFriend[i] = _listFriend[i];
                tempListFriend[tempListFriend.Length - 1] = name;
                _listFriend = tempListFriend;
            }
            else
            {
                _listFriend = new string[1];
                _listFriend[0] = name;
            }
            if (IsInRecommendations(name))
                RemoveFromRecommendations(name);

        }
    }
}
