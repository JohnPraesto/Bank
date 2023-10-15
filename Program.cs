using System.ComponentModel.Design;
using System.Runtime.Serialization;

namespace Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Every array in this list has two elements. One for username, one for password.
            // They are connected by the index of this list.
            List<string[]> customers = new List<string[]>();

            // User accounts will be stored in this list. Each object is made up by username, accountName and sum.
            List<Account> accountList = new List<Account>();

            // Users that failed to insert correct password three times will be added to this list
            // This list is iterated through to find those to be stopped before logging in
            List<string> bannedList = new List<string>();

            // This list will contain the index number from accountList of one user
            // For display of the users account and that accounts index number (will be called account number)
            List<int> userAccounts = new List<int>();

            CreateAccounts(accountList, customers);

            while (true)
            {
                string username = "";
                bool loginSuccessful = false;
                bool locked = false;
                int chosenAccount = 0;
                double withdrawlInt = 0;

                Console.WriteLine("Välkommen till Banken\n");
                Console.WriteLine("[1] Registrera ny användare");
                Console.WriteLine("[2] Logga in");
                string choice = Console.ReadLine();
                Console.Clear();
                if (choice == "1")
                {
                    NewCustomer(customers);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Vad är ditt användarnamn?");
                    username = Console.ReadLine();
                    
                    // This foreach-loop looks for the username in bannedList
                    // If username is in bannedList, the user cannot login
                    foreach (string item in bannedList)
                    {
                        if (item == username)
                        {
                            Console.WriteLine("Denna användare är låst av säkerhetsskäl.");
                            locked = true;
                        }
                    }

                    if(locked == false)
                    {
                        loginSuccessful = Login(customers, username, bannedList);
                    }
                    PressEnter();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }
                while (loginSuccessful)
                {
                    Console.WriteLine("===== MENY =====\n");
                    Console.WriteLine("[1] Se Konton och saldo");
                    Console.WriteLine("[2] Öppna nytt konto");
                    Console.WriteLine("[3] Insättning");
                    Console.WriteLine("[4] Uttag");
                    Console.WriteLine("[5] Överföring");
                    Console.WriteLine("[6] Logga ut");
                    choice = Console.ReadLine();
                    Console.Clear();
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("===== KONTON OCH SALDO =====\n");
                            DisplayUserAccounts(accountList, username, userAccounts);
                            if (userAccounts.Count == 0)
                                Console.WriteLine("Du har inga konton.");
                            PressEnter();
                            break;

                        case "2":
                            Console.WriteLine("===== SKAPA NYTT KONTO =====\n");
                            Account newAccount;
                            Console.Write("Vad ska ditt konto heta? ");
                            string accountName = Console.ReadLine();
                            newAccount = new Account(username, accountName);
                            accountList.Add(newAccount);
                            Console.Write("\nDu har nu skapat ett nytt konto: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(accountName);
                            Console.ResetColor();
                            PressEnter();
                            break;

                        case "3":
                            Console.WriteLine("===== INSÄTTNING =====\n");
                            DisplayUserAccounts(accountList, username, userAccounts);
                            
                            if (userAccounts.Count > 0)
                            {
                                chosenAccount = ChooseAccount(chosenAccount, userAccounts); 

                                while (true)
                                {
                                    Console.Write("\nHur mycket pengar vill du sätta in? ");
                                    try
                                    {
                                        double amount = Convert.ToDouble(Console.ReadLine());
                                        accountList[chosenAccount].sum += amount;
                                        Console.WriteLine($"\nDu har satt it {amount} kr på kontot '{accountList[chosenAccount].accountName}'.");
                                        break;
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Du har matat in ogiltiga tecken. Försök igen.");
                                    }
                                }
                            }
                            else
                                Console.WriteLine("Du har inga konton.");

                            PressEnter();

                            break;

                        case "4":
                            Console.WriteLine("För att kunna ta ut pengar måste du ange ditt lösenord.\n");
                            loginSuccessful = Login(customers, username, bannedList);
                            PressEnter();
                            Console.WriteLine("===== UTTAG =====\n");
                            if (loginSuccessful && userAccounts.Count > 0)
                            {
                                DisplayUserAccounts(accountList, username, userAccounts);
                                chosenAccount = ChooseAccount(chosenAccount, userAccounts);
                                Withdrawal(accountList, chosenAccount);
                                PressEnter();
                                break;
                            }
                            else
                                Console.WriteLine("Du har inga konton.");
                            PressEnter();
                            break;

                        case "5":
                            Console.WriteLine("===== ÖVERFÖRING =====\n");
                            Console.WriteLine("Vilket konto vill du överföra pengar till?\n");
                            Console.WriteLine("[1] Eget konto");
                            Console.WriteLine("[2] Annan användares konto");
                            choice = Console.ReadLine();
                            Console.Clear();
                            switch (choice)
                            {
                                case "1":

                                    DisplayUserAccounts(accountList, username, userAccounts);
                                    Console.WriteLine("Konto att ta pengar ifrån. ");
                                    chosenAccount = ChooseAccount(chosenAccount, userAccounts);
                                    if (userAccounts.Count > 0)
                                    {
                                        withdrawlInt = Withdrawal(accountList, chosenAccount);
                                        Console.WriteLine("Vilket konto vill du sätta in pengarna på?");
                                        chosenAccount = ChooseAccount(chosenAccount, userAccounts);
                                        accountList[chosenAccount].sum += withdrawlInt;
                                        Console.WriteLine($"Du har satt in {withdrawlInt} kr på konto '{accountList[chosenAccount].accountName}'");
                                        PressEnter();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Du har inga konton");
                                    }

                                    break;

                                case "2":

                                    DisplayUserAccounts(accountList, username, userAccounts);
                                    Console.WriteLine("Konto att ta pengar ifrån.\n");
                                    chosenAccount = ChooseAccount(chosenAccount, userAccounts);

                                    if (userAccounts.Count > 0)
                                    {
                                        withdrawlInt = Withdrawal(accountList, chosenAccount);
                                    }
                                    else
                                        Console.WriteLine("Du har inga konton.");
                                        

                                    userAccounts.Clear();
                                    for (int i = 0; i < accountList.Count; i++)
                                    {
                                        if (accountList[i].userName != username)
                                        {
                                            Console.WriteLine($"[{i}] " + accountList[i].accountName + " " + accountList[i].sum);
                                            userAccounts.Add(i);
                                        }
                                    }
                                    Console.Write("\nVilket konto vill du sätta in pengarna på?");
                                    chosenAccount = ChooseAccount(chosenAccount, userAccounts);
                                    accountList[chosenAccount].sum += withdrawlInt;
                                    Console.WriteLine($"Du har satt in {withdrawlInt} kr på konto '{accountList[chosenAccount].accountName}'");
                                    PressEnter();

                                    break;

                                default:
                                    Console.WriteLine("Ogiltigt val");
                                    break;
                            }
                            break;

                        case "6":
                            loginSuccessful = false;
                            Console.WriteLine("Du är nu utloggad");
                            PressEnter();
                            break;

                        default:
                            Console.WriteLine("Ogiltigt val.");
                            break;
                    }
                }
            }
        }

        // This method creates default users and their accounts
        public static void CreateAccounts(List<Account> accountList, List<string[]> customers)
        {
            string[] usernames = new string[5] { "Anna", "Berit", "Celine", "Dakota", "Ewald" };
            for (int i = 0; i < usernames.Length; i++)
            {
                Account newAccount;
                newAccount = new Account(usernames[i], (usernames[i] + "s Lönekonto"));
                accountList.Add(newAccount);
            }
            for (int i = 0; i < usernames.Length; i++)
            {
                Account newAccount;
                newAccount = new Account(usernames[i], (usernames[i] + "s Sparkonto"));
                accountList.Add(newAccount);
            }
            for (int i = 0; i < accountList.Count; i++)
            {
                accountList[i].sum = i * 100;
            }
            for (int i = 0; i < usernames.Length; i++)
            {
                string[] customer = new string[2];
                customer[0] = usernames[i]; // The default users will have same password as username
                customer[1] = usernames[i];
                customers.Add(customer);
            }
        }

        // Lets the user create a new customer
        public static void NewCustomer(List<string[]> customers)
        {
            string[] customer = new string[2];
            bool go = true;
            Console.WriteLine("Vad ska du ha för användarnamn?");
            customer[0] = Console.ReadLine();
            foreach (string[] item in customers)
            {
                if (customer[0] == item[0]) // If username is already registered
                    go = false;
            }

            if (go)
            {
                Console.WriteLine("Vad ska du ha för lösenord?"); // Would be cool to convert inserted characters to *
                customer[1] = Console.ReadLine();
                customers.Add(customer);
            }
            else if (go == false)
            {
                Console.WriteLine("Användarnamnet är uppgaget.\nTryck Enter.");
                Console.ReadLine();
            }
            
            Console.Clear();
        }

        // Method for Loging in to the Bank. It's also used to verify user before withdrawing money.
        // Asks the user for password. If wrong password is given three times. The user is added
        // to the bannedList and will not be able to log in anymore.
        public static bool Login(List<string[]> customers, string username, List<string> bannedList)
        {
            int passwordTries = 0;
            foreach (string[] item in customers)
            {
                if (username == item[0])
                {
                    while (passwordTries < 3)
                    {
                        Console.Write("Ange ditt lösenord: ");
                        string password = Console.ReadLine();
                        if (password == item[1])
                        {
                            Console.WriteLine($"\nVälkommen, {username}! Du loggas nu in.");
                            return true;
                        }
                        else if (password != item[1])
                        {
                            passwordTries++;
                            if (passwordTries == 3)
                            {
                                Console.WriteLine("Du har skrivit in fel lösenord för många gånger. Användaren låst.");
                                bannedList.Add(username);
                                return false;
                            }
                            Console.WriteLine("Fel lösenord");
                            Console.WriteLine($"Återstående försök: {3 - passwordTries}\n");
                        }
                    }
                }
            }
            Console.WriteLine("Andändaren finns ej registrerad.");
            return false;
        }

        public static void PressEnter()
        {
            Console.WriteLine("\nEnter för att gå vidare.");
            Console.ReadLine();
            Console.Clear();
        }

        // In this method, accountList is iterated through to find all Account-objects with matching username
        // And the accounts will be displayed to console
        public static void DisplayUserAccounts(List<Account> accountList, string username, List<int> userAccounts)
        {
            userAccounts.Clear();
            for (int i = 0; i < accountList.Count; i++)
            {
                if (accountList[i].userName == username)
                {
                    Console.WriteLine($"Kontonummer: {i}");
                    Console.WriteLine($"Namn:        {accountList[i].accountName}");
                    Console.WriteLine($"Saldo:       {accountList[i].sum}");
                    Console.WriteLine("---------------------------------");
                    userAccounts.Add(i);
                }
            }
        }

        // Whenever the user needs to choose an account, this method is called.
        // This method only allows the user to choose an account number of those on display
        public static int ChooseAccount(int chosenAccount, List<int> userAccounts)
        {
            string message = "Felaktigt kontonummer. Försök igen.\n";
            while (true)
            {
                Console.Write("Välj konto. Ange kontonummer: ");
                try
                {
                    chosenAccount = Convert.ToInt32(Console.ReadLine());
                    foreach (int item in userAccounts)
                    {
                        if (chosenAccount == item)
                        {
                            message = "";
                            return chosenAccount;
                        }
                    }
                }
                catch
                {
                    message = "Felaktigt kontonummer. Försök igen.\n";
                }
                Console.WriteLine(message);
            }
        }

        // This method is called whenever the user wants to take money out of an account
        public static double Withdrawal(List<Account> accountList, int chosenAccount)
        {
            while (true)
            {
                Console.Write("\nHur mycket vill du ta ut? ");
                string withdrawl = Console.ReadLine();
                if (double.TryParse(withdrawl, out double withdrawlInt))
                {
                    if (accountList[chosenAccount].sum >= withdrawlInt)
                    {
                        accountList[chosenAccount].sum -= withdrawlInt;
                        Console.WriteLine($"Du har tagit ut {withdrawlInt} kr från kontot '{accountList[chosenAccount].accountName}'.");
                        return withdrawlInt;
                    }
                    else
                    {
                        Console.WriteLine("Du har inte så mycket pengar på kontot!");
                    }
                }
                else
                {
                    Console.WriteLine("Du har matat in ogiltiga tecken. Försök igen.");
                }
            }
        }
    }
}