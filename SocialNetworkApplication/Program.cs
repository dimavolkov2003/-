using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkLibrary;

namespace SocialNetworkApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            SocialNetwork socialNetwork = new SocialNetwork("Dream");
            socialNetwork.CreateAccount(AccountType.Standard, "Dima", "1", SendInvitationsHandler, AddFriendHandler, DeleteFriendHandler);
            socialNetwork.CreateAccount(AccountType.Standard, "Alex", "2", SendInvitationsHandler, AddFriendHandler, DeleteFriendHandler);
            socialNetwork.CreateAccount(AccountType.Premium, "Ilon", "3", SendInvitationsHandler, AddFriendHandler, DeleteFriendHandler);
            socialNetwork.CreateAccount(AccountType.Premium, "Igor", "4", SendInvitationsHandler, AddFriendHandler, DeleteFriendHandler);
            bool alive = true;
            Account account = null;
            bool AccountFound = false;
            while (alive)
            {
                if (account != null)
                {
                    AccountFound = true;
                }
                while (AccountFound)
                {
                    ConsoleColor color2 = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen; // выводим список команд зеленым цветом
                    Console.WriteLine("1. Personal data \t 2. Send a request for friendship\t 3. All people");
                    Console.WriteLine("4. Add friend    \t 5. Delete friend                \t 6. Recommend friends");
                    Console.WriteLine("7. Sign out");
                    Console.WriteLine("Select the item number:");
                    Console.ForegroundColor = color2;
                    try
                    {
                        int command = Convert.ToInt32(Console.ReadLine());

                        switch (command)
                        {
                            case 1:
                                AccountInformation(account);
                                break;
                            case 2:
                                InviteToFriends(socialNetwork, account);
                                break;
                            case 3:
                                ShowAllPeople(socialNetwork, account.Name);
                                break;
                            case 4:
                                AddFriend(socialNetwork, account);
                                break;
                            case 5:
                                DeleteFriend(socialNetwork, account);
                                break;
                            case 6:
                                RecommendFriends(socialNetwork, account);
                                break;
                            case 7:
                                AccountFound = false;
                                continue;
                        }

                    }
                    catch (Exception ex)
                    {
                        // выводим сообщение об ошибке красным цветом
                        color2 = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ForegroundColor = color2;
                    }
                }
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen; // выводим список команд зеленым цветом
                Console.WriteLine("1. Log in \t 2. Sign up \t 3. Exit");
                Console.WriteLine("Select the item number:");
                Console.ForegroundColor = color;
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            account = LogIn(socialNetwork);
                            break;
                        case 2:
                            account = SignUp(socialNetwork);
                            break;
                        case 3:
                            alive = false;
                            continue;

                    }

                }
                catch (Exception ex)
                {
                    // выводим сообщение об ошибке красным цветом
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }
        private static Account LogIn(SocialNetwork socialNetwork)
        {
            Console.WriteLine("Enter login:");

            string name = Convert.ToString(Console.ReadLine());
            Account account = socialNetwork.FindAccount(name);
            if (account == null)
            {
                throw new Exception("Account not found");
            }
            Console.WriteLine("Enter password:");
            string password = Convert.ToString(Console.ReadLine());
            if (account.Password != password)
            {
                throw new Exception("Incorrect password");
            }
            return account;
        }
        private static Account SignUp(SocialNetwork socialNetwork)
        {
            Console.WriteLine("Select an account type: 1. Standard 2. Premium");
            AccountType accountType;

            int type = Convert.ToInt32(Console.ReadLine());

            if (type == 2)
                accountType = AccountType.Premium;
            else
                accountType = AccountType.Standard;

            Console.WriteLine("Enter login:");

            string name = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter password:");
            string password = Convert.ToString(Console.ReadLine());
            socialNetwork.CreateAccount(accountType, name, password, SendInvitationsHandler, AddFriendHandler, DeleteFriendHandler);
            Account[] allAccounts = socialNetwork.GetAccount();
            Account account = allAccounts[allAccounts.Length - 1];
            return account;
        }
        private static void AccountInformation(Account account)
        {

            Console.WriteLine("Name: {0}", account.Name);
            Console.WriteLine("Password: {0}", account.Password);
            string accountType = "Standard";
            if (account is PremiumAccount)
                accountType = "Premium";
            Console.WriteLine("AccountType: {0}", accountType);
            if (account.ListFriend != null)
            {
                Console.Write("ListFriend: ");
                for (int i = 0; i < account.ListFriend.Length; i++)
                {
                    Console.Write("{0}", account.ListFriend[i]);
                    if (i != account.ListFriend.Length - 1)
                        Console.Write(", ");
                }
                Console.WriteLine();
            }
            if (account.ListInvitations != null)
            {
                Console.Write("ListInvitations: ");
                for (int i = 0; i < account.ListInvitations.Length; i++)
                {
                    Console.Write("{0}", account.ListInvitations[i]);
                    if (i != account.ListInvitations.Length - 1)
                        Console.Write(", ");
                }
                Console.WriteLine();
            }
            if (account is StandardAccount)
            {
                StandardAccount standardAccount = account as StandardAccount;
                if (standardAccount.ListWaiting != null)
                {
                    Console.Write("ListWaiting: ");
                    for (int i = 0; i < standardAccount.ListWaiting.Length; i++)
                    {
                        Console.Write("{0}", standardAccount.ListWaiting[i]);
                        if (i != standardAccount.ListWaiting.Length - 1)
                            Console.Write(", ");
                    }
                    Console.WriteLine();
                }
            }
        }
        private static void InviteToFriends(SocialNetwork socialNetwork, Account account)
        {
            if(account.ListOfRecommendations != null)
            {
                Console.Write("ListOfRecommendations: ");
                for (int i = 0; i < account.ListOfRecommendations.Length; i++)
                {
                    Console.Write("{0}", account.ListOfRecommendations[i]);
                    if (i != account.ListOfRecommendations.Length - 1)
                        Console.Write(", ");
                }
                Console.WriteLine();
                }
            else
            {
                Console.WriteLine("ListOfRecommendations: No recommendations");
            }
            Console.WriteLine("Select the item number:");
            Console.WriteLine("1. Send a request for friendship \t 2. Exit");
            int command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    Console.WriteLine("To Whom to send a request for friendship?");
                    string name = Convert.ToString(Console.ReadLine());
                    Account accountOfSender = socialNetwork.FindAccount(name);
                    if (account == null)
                        throw new Exception("Account not found");
                    string nameOfSender = name;
                    socialNetwork.SendInvitations(account.Name, nameOfSender);
                    break;
                case 2:
                    return;
            }
        }
        private static void ShowAllPeople(SocialNetwork socialNetwork, string nameOfHost)
        {
            Account[] allAccounts = socialNetwork.GetAccount();
            int numberOfPeople = 1;
            if (allAccounts != null)
            {
                for (int i = 0; i < allAccounts.Length; i++)
                {
                    if (allAccounts[i].Name != nameOfHost)
                    {
                        Console.WriteLine("{0}. {1}", numberOfPeople, allAccounts[i].Name);
                        numberOfPeople++;
                    }
                }
            }
        }
        private static void AddFriend(SocialNetwork socialNetwork, Account account)
        {
            Console.WriteLine("All requests for friendship");
            string[] listInvitations = account.ListInvitations;
            if (listInvitations != null)
            {
                for (int i = 0; i < listInvitations.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, listInvitations[i]);
                }
            }
            Console.WriteLine("Select the item number:");
            Console.WriteLine("1. Add friend \t 2. Exit");
            int command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    Console.WriteLine("Who to add?:");
                    string nameOfFriend = Convert.ToString(Console.ReadLine());
                    Account[] allAccounts = socialNetwork.GetAccount();
                    Account accountOfFriend = socialNetwork.FindAccount(nameOfFriend);
                    if (accountOfFriend == null)
                        throw new Exception("Account not found");
                    bool isInvited = false;
                    if (listInvitations != null)
                    {
                        for (int i = 0; i < listInvitations.Length; i++)
                        {
                            if (listInvitations[i] == nameOfFriend)
                            {
                                isInvited = true;
                            }
                        }
                    }
                    if (isInvited)
                        socialNetwork.AddFriend(account.Name, nameOfFriend);
                    else
                        throw new Exception("Account not found");
                    break;
                case 2:
                    return;
            }       
        }
        private static void DeleteFriend(SocialNetwork socialNetwork, Account account)
        {
            if (account.ListFriend != null)
            {
                if (account.ListFriend.Length != 0)
                {
                    Console.Write("ListFriend: ");
                    for (int i = 0; i < account.ListFriend.Length; i++)
                    {
                        Console.Write("{0} ", account.ListFriend[i]);
                        if(i != account.ListFriend.Length - 1)
                            Console.Write(", ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Select the item number: ");
                    Console.WriteLine("1. Delete friend \t 2. Exit");
                    int command = Convert.ToInt32(Console.ReadLine());
                    switch (command)
                    {
                        case 1:
                            Console.WriteLine("Who to delete?");
                            string nameOfFriend = Convert.ToString(Console.ReadLine());
                            Account accountOfFriend = socialNetwork.FindAccount(nameOfFriend);
                            if (accountOfFriend == null)
                                throw new Exception("Account not found");
                            socialNetwork.DeleteFriend(account.Name, nameOfFriend);
                            break;
                        case 2:
                            return;
                    }      
                }
            }
            else
            {
                Console.WriteLine("ListFriend: No friends");
                Console.WriteLine("Select the item number:");
                Console.WriteLine("1. Exit");
                int command = Convert.ToInt32(Console.ReadLine());
                switch (command)
                {
                    case 1:
                        return;
                }
            }         
        }
        private static void RecommendFriends(SocialNetwork socialNetwork, Account account)
        {
            if (account.ListFriend != null)
            {
                Console.Write("ListFriend: ");
                for (int i = 0; i < account.ListFriend.Length; i++)
                {
                    Console.Write("{0} ", account.ListFriend[i]);
                    if (i != account.ListFriend.Length - 1)
                        Console.Write(",");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("ListFriend: No friends");
            }
            Console.WriteLine("Select the item number:");
            Console.WriteLine("1. Recommend friends \t 2. Exit");
            int command = Convert.ToInt32(Console.ReadLine());
            switch (command)
            {
                case 1:
                    Console.WriteLine("Enter the name of the first friend:");
                    string nameOfFirstFriend = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("Enter the name of the second friend:");
                    string nameOfSecondFriend = Convert.ToString(Console.ReadLine());
                    socialNetwork.RecommendFriends(account.Name, nameOfFirstFriend, nameOfSecondFriend);
                    break;
                case 2:
                    return;
            }
        }
        private static void SendInvitationsHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void AddFriendHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void DeleteFriendHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
