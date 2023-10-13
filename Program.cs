using System.ComponentModel.Design;
using System.Runtime.Serialization;

namespace Bank
{
    internal class Program
    {
        /* 
        NÄSTA STEG ÄR ATT make a fuckload of methods

        gör sum till double i account-klassen

        utreda hur man sparar mellan körningar
        
        Vore kul att välja bindningstid på sparkontot och lägga till olika räntor

        Möjligt att effektivisera inloggningsmetoden om man kan ta ut användarens indexplats i customers-Listan?

        Översätt allt till engelska

        */

        static void Main(string[] args)
        {
            // Varje array i listan har två element. En för användarnamn, en för lösenord.
            List<string[]> customers = new List<string[]>();

            // I denna lista kommer konton lagras, varje konto-objekt består av user, accountName, och sum.
            List<Account> accountList = new List<Account>();

            // Denna lista laddas med användare som skrivit in fel lösenord för många gånger
            // De som läggs till i listan kommer inte få försöka logga in igen.
            List<string> bannedList = new List<string>();

            // Explain list
            List<int> userAccounts = new List<int>();

            string message = "";

            while (true)
            {
                string username = "";
                bool loginSuccessful = false;
                bool locked = false;
                int chosenAccount = 0;
                int withdrawlInt = 0;

                Console.WriteLine("Välkommen till Banken\n");
                Console.WriteLine("[1] Vill du registrera dig som ny kund hos oss?");
                Console.WriteLine("[2] Logga in.");
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
                    
                    // SÅNNAHÄR FOREACH-LOOPAR används ju hela tiden på olika ställen
                    // undra om det går att göra en metod av just bara foeach-loopen på nåt sätt
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
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }
                while (loginSuccessful)
                {
                    Console.Clear();
                    Console.WriteLine("Välkommen!\n");
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
                            DisplayUserAccounts(accountList, username, userAccounts);
                            if (userAccounts.Count == 0)
                                Console.WriteLine("Du har inga konton.");
                            PressEnter();
                            break;

                        case "2":
                            Account newAccount;
                            Console.Write("Vad ska ditt konto heta? ");
                            string accountName = Console.ReadLine();
                            newAccount = new Account(username, accountName);
                            accountList.Add(newAccount);
                            Console.WriteLine($"Du har nu skapat ett nytt konto med namn {accountName}");
                            PressEnter();
                            break;

                        case "3": // INSÄTTNING  DEPOSIT
                            DisplayUserAccounts(accountList, username, userAccounts);

                            if (userAccounts.Count > 0)
                            {
                                chosenAccount = ChooseAccount(chosenAccount, userAccounts); // Kommer förändringen av chosenAccount ut från metoden?
                                                                            //  Det stod nåt om det i Prog 1 boken

                                while (true) // Göra metod av detta. Liknande finns i insättning.
                                {
                                    Console.WriteLine("Hur mycket pengar vill du sätta in?");
                                    try
                                    {
                                        int amount = Convert.ToInt32(Console.ReadLine());
                                        accountList[chosenAccount].sum += amount;
                                        Console.WriteLine($"Du har satt it {amount} kr på kontot '{accountList[chosenAccount].accountName}'.");
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

                        case "4": // TA UT PENGAR
                            loginSuccessful = Login(customers, username, bannedList);
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

                        case "5": // ÖVERFÖRING
                            Console.WriteLine("Vilket konto vill du överföra pengar till?");
                            Console.WriteLine("[1] Eget konto");
                            Console.WriteLine("[2] Annan användares konto");
                            choice = Console.ReadLine();
                            switch (choice)
                            {
                                case "1":

                                    DisplayUserAccounts(accountList, username, userAccounts);
                                    chosenAccount = ChooseAccount(chosenAccount, userAccounts);
                                    if (userAccounts.Count > 0)
                                    {
                                        withdrawlInt = Withdrawal(accountList, chosenAccount);
                                        Console.Write("Vilket konto vill du sätta in pengarna på?");
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
                                    Console.Write("Vilket konto vill du sätta in pengarna på?");
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
        public static void NewCustomer(List<string[]> customers)
        {
            string[] customer = new string[2];
            bool go = true;
            Console.WriteLine("Vad ska du ha för användarnamn?");
            customer[0] = Console.ReadLine();
            foreach (string[] item in customers)
            {
                if (customer[0] == item[0])
                {
                    go = false;
                }
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
                            Console.WriteLine("Rätt lösenord!");
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
            Console.WriteLine("Enter för att gå vidare.");
            Console.ReadLine();
        }

        public static void DisplayUserAccounts(List<Account> accountList, string username, List<int> userAccounts)
        { // Det är nåt lurt här. Med userAccounts tror jag. Ibland visas inte listan. Nåt med clear eller add alltså.
            userAccounts.Clear();
            for (int i = 0; i < accountList.Count; i++)
            {
                if (accountList[i].userName == username)
                {
                    Console.WriteLine($"Kontonummer: {i}\t Namn: " + accountList[i].accountName + "\t Saldo: " + accountList[i].sum);
                    userAccounts.Add(i);
                }
            }
        }


        // I följande metod tillåts användaren endast mata in någon av de nummer
        // som userAccounts-listan har laddats med.
        public static int ChooseAccount(int chosenAccount, List<int> userAccounts)
        {
            string message = "Felaktigt kontonummer. Försök igen.\n"; // samma rad skrivs två gånger i samma metod. Gillar det inte.
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

        // NOT: OM MAN VÄLJER ETT KONTO SOM INTE HAR NÅGRA PENGAR
        // KAN MAN ALDRIG KOMMA UR LOOPEN
        public static int Withdrawal(List<Account> accountList, int chosenAccount)
        {
            while (true)
            {
                Console.WriteLine("Hur mycket vill du ta ut?");
                string withdrawl = Console.ReadLine();
                if (int.TryParse(withdrawl, out int withdrawlInt))
                {
                    if (accountList[chosenAccount].sum > withdrawlInt)
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


/* TILL README

valt att man måste skapa användare. alla variabler och listor är tomma vid start.
eftersom jag senare hoppas på att få in att det sparas mellan körningar.
Men får se om jag hinner det.

Tankar kring metoder. Repeterar kod? Rensar upp i main?

Vill inte har för många if i if. Djupet.

Funderingar kring hur man kopplar en användare till sitt lösenord och sina konton
Först lista med array bestående av användare + lösen. Men hur binda till konton?
Börjar överväga objekt.
Struktur:

inte riktigt nöjd med messege =
beroende på var i if-sataserna man är
den ska meddela om man lyckats logga in eller fel lösenord eller användare finns ej

känner hela tiden att jag är missnöjd med koden jag skapar
iom jag var bortrest förra veckan och iom jag har ambitiösa mål
inte bara för betyg, men mest för att lära, vill jag uppnå alla VG mål
samt inkludera alla funtkioner i mitt program, även extrauppgifterna.
Så är jag stressad för att jag inte har mer tid. Jag slänger mig in i lösningar
innan de är färdigtänkta. Och känner inte att jag hinner göra om och tänka om.
Pga tidsbrist måste jag välja de lätta lösningarna som jag redan påbörjat.

SÅ arbetssättet jag anammat har varit att bara få nåt att funka... och ev senare slipa det

MITT GENERELLA ARBETSSÄT R ATT GÖRA LÄTTA LÖSNINGAR FÖRST
sedan se vilka delar man kan effektivisera eller göra metoder av

.. men har vart rätt så rörigt att navigera bland 600 rader ometodiserad kod och försöka synkronisera
metoderna så att de ska passa på alla olika ställen.

foreach-loopen i inloggningssystemet fortsätter att loopa även om den redan gått igenom rätt användare (ineffektivt)
Men man kan lägga in en break; där kanske?

*/


/* OLD CODE



            // Display registered users
            if (customers.Count <= 0)
            {
                Console.WriteLine("\nDet finns inga registrerade kunder.");
            }
            else
            {
                foreach (string[] item in customers)
                {
                    // this line was suggested by ChatGPT
                    // Console.WriteLine(string.Join(" ", item)); 
                    // it does exactly the same thing as this next line
                    // but this line is more comprehensable for me
                    Console.WriteLine(item[0] + " " +  item[1]);
                }
            }
*/