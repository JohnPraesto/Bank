using System.ComponentModel.Design;
using System.Runtime.Serialization;

namespace Bank
{
    internal class Program
    {
        /* 
        NÄSTA STEG ÄR ATT användaren endast ska tillåtas mata in displayade kontonummer vid insättning, 
        även senare överföring!

        gör sum till double i account-klassen

        jobba ner djupet och skapa metoder för att städa upp kod

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

            
            string message = "";

            while (true)
            {
                string username = "";
                bool loginSuccessful = false;
                bool locked = false;

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
                            // Gå igenom lista med account-objekt och sök efter Account.user == (det aktiva usernamet)
                            // Skriv ut alla konton
                            // Om det inte finns några konton så skriv "Du har inga konton".
                            message = "Du har inga konton.\n";
                            foreach (Account item in accountList)
                            {
                                if(item.userName == username)
                                {
                                    Console.WriteLine(item.accountName + " " + item.sum);
                                    message = "";
                                }
                            }
                            Console.WriteLine(message);
                            Console.ReadLine();

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
                        case "3":
                            // Insättning
                            // TIll metod, det är exakt samma kod som i case 1
                            int accNr = 0;
                            //foreach (Account item in accountList)
                            //{
                            //    if (item.userName == username)
                            //    {
                            //        accNr++;
                            //        Console.WriteLine($"[{accNr}] " + item.accountName);
                            //    }
                            //}
                            bool test = false;
                            for (int i = 0; i < accountList.Count; i++)
                            {
                                if (accountList[i].userName == username)
                                {
                                    Console.WriteLine($"[{i}] " + accountList[i].accountName);
                                    test = true;
                                }
                            }
                            //(Man kan kalla indexplats för varje konto dess kontonummer.
                            if(test)
                            {
                                while (true)
                                {
                                    Console.WriteLine("Vilket konto vill du sätta in pengar på? ");
                                    string chooseAccount = Console.ReadLine(); // Vad händer om användaren
                                    // lyckas välja ett index som finns, men som tillhör en annan användare?
                                    // OCH användaren ska inte kunna mata in ett indexnummer som inte finns
                                    // Hmm, undvika båda problemen om användaren endast tillåts mata in indexnr
                                    // som displayas

                                    if (int.TryParse(chooseAccount, out int accountNr2))
                                    {
                                        while (true)
                                        {
                                            Console.Write("Hur mycket vill du sätta in? ");
                                            string deposit = Console.ReadLine();
                                            if (int.TryParse(deposit, out int depositInt))
                                            {
                                                accountList[accountNr2].sum = depositInt;
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Din insättning innehöll ogiltiga tecken. Försök igen.");
                                            }
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Input not acceptable");
                                        Console.WriteLine("Enter för att gå vidare.");
                                        Console.ReadLine();
                                    }
                                }
                                
                            }
                            else if(test != true)
                            {
                                Console.WriteLine("Du har inga konton.");
                            }
                            PressEnter();
                            break;
                        case "4": // TA UT PENGAR
                            loginSuccessful = Login(customers, username, bannedList);

                            if (loginSuccessful)
                            {
                                // Lista skapas som laddas med de indexplatser
                                // användarens konton ligger på i customers-listan
                                List<int> userAccounts = new List<int>(); 
                                for (int i = 0; i < accountList.Count; i++)
                                {
                                    if (accountList[i].userName == username)
                                    {
                                        Console.WriteLine($"[{i}] " + accountList[i].accountName + " " + accountList[i].sum);
                                        userAccounts.Add(i);
                                    }
                                }


                                string accountWithdrawl = "";
                                int accountWithdrawlToInt = 0;
                                // I följande While tillåts användaren endast mata in någon av de nummer
                                // som userAccounts-listan har laddats med.
                                bool go = true;
                                while (go)
                                {
                                    Console.Write("Vilket konto vill du dra pengar från? Ange kontonummer: ");
                                    accountWithdrawl = Console.ReadLine();
                                    try
                                    {
                                        accountWithdrawlToInt = Convert.ToInt32(accountWithdrawl);
                                        foreach (int item in userAccounts)
                                        {
                                            if (accountWithdrawlToInt == item)
                                            {
                                                go = false;
                                                message = "Hur mycket vill du ta ut?";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        message = "Kontonummret finns inte. Försök igen.\n";
                                    }
                                    Console.WriteLine(message);
                                }


                                    

                                while (true)
                                {
                                    
                                    string withdrawl = Console.ReadLine();
                                    if (int.TryParse(withdrawl, out int withdrawlInt))
                                    {
                                        if (accountList[accountWithdrawlToInt].sum > withdrawlInt)
                                        {
                                            accountList[accountWithdrawlToInt].sum -= withdrawlInt;
                                            Console.WriteLine($"Du har tagit ut {withdrawlInt} kr från kontot.");

                                            break;
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
                                break;

                            }

                            PressEnter();

                            break;
                        case "5":
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
                            //message = "Rätt lösenord!";
                            return true;
                        }
                        else if (password != item[1])
                        {
                            passwordTries++;
                            if (passwordTries == 3)
                            {
                                Console.WriteLine("Du har skrivit in fel lösenord för många gånger. Användaren låst.");
                                //message = "Du har skrivit in fel lösenord för många gånger. Användaren låst.";
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