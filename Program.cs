using System.ComponentModel.Design;

namespace Bank
{
    internal class Program
    {
        /* 
        NÄSTA STEG ÄR ATT jobba med insättning


        jobba ner djupet och skapa metoder för att städa upp kod

        utreda hur man sparar mellan körningar
        
        Vore kul att välja bindningstid på sparkontot och lägga till olika räntor

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
                    message = $"Användarnamnet '{username}' fanns inte registrerat";
                    int passwordTries = 0;

                    // SÅNNAHÄR FOREACH-LOOPAR används ju hela tiden på olika ställen
                    // undra om det går att göra en metod av just bara foeach-loopen på nåt sätt
                    foreach (string item in bannedList)
                    {
                        if (item == username)
                        {
                            message = "Denna användare är låst av säkerhetsskäl.";
                            locked = true;
                        }
                    }

                    if(locked == false)
                    {
                        foreach (string[] item in customers)
                        {
                            if (username == item[0])
                            {
                                while (passwordTries < 3)
                                {
                                    Console.WriteLine("Vad är ditt lösenord?\n");
                                    string password = Console.ReadLine();
                                    if (password == item[1])
                                    {
                                        message = "Du är inloggad!";
                                        loginSuccessful = true;
                                        break;
                                    }
                                    else if (password != item[1])
                                    {
                                        passwordTries++;
                                        if (passwordTries == 3)
                                        {
                                            message = "Du har akrivit in fel lösenord för många gånger. Användaren låst.";
                                            bannedList.Add(username);
                                        }
                                        Console.WriteLine("Fel lösenord");
                                        Console.WriteLine($"Återstående försök: {3 - passwordTries}\n");
                                    }
                                }
                            }
                        }
                    }
                    Console.WriteLine(message);
                    Console.WriteLine("\nTryck Enter för att gå vidare."); // Får vi många fler av denna bör det bli egen metod
                    Console.ReadLine();
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
                            Console.WriteLine("Enter för att gå vidare.");
                            Console.ReadLine();
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
                            if(test)
                            {
                                while (true)
                                {
                                    Console.WriteLine("Vilket konto vill du sätta in pengar på? ");
                                    string chooseAccount = Console.ReadLine();

                                    if (int.TryParse(chooseAccount, out int accountNr))
                                    {
                                        while (true)
                                        {
                                            Console.Write("Hur mycket vill du sätta in? ");
                                            string deposit = Console.ReadLine();
                                            if (int.TryParse(deposit, out int depositInt))
                                            {
                                                accountList[accountNr].sum = depositInt;
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
                                    }
                                }
                                
                            }
                            else if(test != true)
                            {
                                Console.WriteLine("Du har inga konton.");
                            }
                            Console.WriteLine("Enter för att gå vidare.");
                            Console.ReadLine();
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            loginSuccessful = false;
                            Console.WriteLine("Du är nu utloggad");
                            Console.WriteLine("Enter för att gå vidare.");
                            Console.ReadLine();
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

        public static void Login(List<string[]> customers, string username)
        {

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