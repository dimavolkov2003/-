using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkLibrary
{
    public enum AccountType
    {
        Standard,
        Premium
    }
    public class SocialNetwork
    {
        private Account[] _accounts;
        public string Name { get; private set; }
        public SocialNetwork(string name)
        {
            if (name != null)
                this.Name = name;
            else
                throw new NullReferenceException();
        }
        public Account[] GetAccount()
        {
            return _accounts;
        }
        public void CreateAccount(AccountType accountType, string name, string password, AccountStateHandler SendInvitationsHandler, 
            AccountStateHandler AddFriendHandler, AccountStateHandler DeleteFriendHandler)
        {
            Account newAccount = null;
            if (FindAccount(name) != null)
            {
                throw new SocialNetworkException("The name is already in use by another user");
            }

            switch (accountType)
            {
                case AccountType.Standard:
                    newAccount = new StandardAccount(name, password);
                    break;
                case AccountType.Premium:
                    newAccount = new PremiumAccount(name, password);
                    break;
            }

            if (newAccount == null)
                throw new SocialNetworkException("Account creation error");

            if (_accounts == null)
            {
                _accounts = new Account[] { newAccount };
            }
            else
            {
                Account[] tempAccounts = new Account[_accounts.Length + 1];
                for (int i = 0; i < _accounts.Length; i++)
                    tempAccounts[i] = _accounts[i];
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                _accounts = tempAccounts;
            }

            newAccount.Added += AddFriendHandler;
            newAccount.Deleted += DeleteFriendHandler;
            newAccount.Invited += SendInvitationsHandler;
        }
        public void AddFriend(string nameOfHost, string nameOfSender)
        {
            Account accountOfHost = FindAccount(nameOfHost);
            Account accountOfRecipient = FindAccount(nameOfSender);
            if (accountOfHost == null || accountOfRecipient == null)
            {
                throw new SocialNetworkException("Account not found");
            }
            accountOfHost.AddFriend(accountOfRecipient.Name, UserStatus.Host);
            accountOfRecipient.AddFriend(accountOfHost.Name, UserStatus.Recipient);
        }
        public void SendInvitations(string nameOfHost, string nameOfRecipient)
        {
            if (nameOfHost == nameOfRecipient)
            {
                throw new SocialNetworkException("You cannot perform this operation with the same user");
            }
            Account accountOfHost = FindAccount(nameOfHost);
            Account accountOfRecipient = FindAccount(nameOfRecipient);
            if (accountOfHost == null || accountOfRecipient == null)
            {
                throw new SocialNetworkException("Account not found");
            }
            bool isInvitation = false;
            if (accountOfHost.ListInvitations != null)
            {
                for (int i = 0; i < accountOfHost.ListInvitations.Length; i++)
                {
                    if (accountOfHost.ListInvitations[i] == nameOfRecipient)
                    {
                        isInvitation = true;
                        break;
                    }
                }
            }
            if (!isInvitation)
            {
                accountOfHost.SendInvitations(accountOfRecipient.Name, UserStatus.Host);
                accountOfRecipient.SendInvitations(accountOfHost.Name, UserStatus.Recipient);
            }
            else
            {
                AddFriend(nameOfHost, nameOfRecipient);
            }
        }
        public void RecommendFriends(string nameOfHost, string nameOfFirstFriend, string nameOfSecondFriend)
        {
            Account accountOfHost = FindAccount(nameOfHost);
            if (nameOfFirstFriend == nameOfSecondFriend)
                throw new SocialNetworkException("You cannot perform this operation with the same user");
            Account accountOfFirstFriend = FindAccount(nameOfFirstFriend);
            Account accountOfSecondFriend = FindAccount(nameOfSecondFriend);
            if (accountOfHost == null || accountOfFirstFriend == null || accountOfSecondFriend == null)
                throw new SocialNetworkException("Account not found");
            bool areFriends = false;
            if (accountOfHost.ListFriend != null)
            {
                int numOfFriendsToRecommed = 0;
                for (int i = 0; i < accountOfHost.ListFriend.Length; i++)
                {
                    if (accountOfHost.ListFriend[i] == nameOfFirstFriend || accountOfHost.ListFriend[i] == nameOfSecondFriend)
                        numOfFriendsToRecommed++;
                }
                if (numOfFriendsToRecommed == 2)
                    areFriends = true;
            }
            if (areFriends)
            {
                accountOfFirstFriend.AddToRecommendations(nameOfSecondFriend);
                accountOfSecondFriend.AddToRecommendations(nameOfFirstFriend);
            }
            else
            {
                throw new SocialNetworkException("These are not your friends");
            }
        }
        public void DeleteFriend(string nameOfHost, string nameOfFriend)
        {
            Account accountOfHost = FindAccount(nameOfHost);
            Account accountOfFriend = FindAccount(nameOfFriend);
            if (accountOfHost == null || accountOfFriend == null)
            {
                throw new SocialNetworkException("Account not found");
            }
            accountOfHost.DeleteFriend(nameOfFriend);
            accountOfFriend.DeleteFriend(nameOfHost);
        }
        public Account FindAccount(string name)
        {
            if (_accounts != null)
            {
                for (int i = 0; i < _accounts.Length; i++)
                {
                    if (_accounts[i].Name == name)
                    {
                        return _accounts[i];
                    }
                }
            }
            return null;
        }
    }
}
