namespace Bank
{
    internal class Program
    {
        /* 
        NÄSTA STEG ÄR ATT ska inte gå att registrera samma användarnamn flera gånger.


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

            string username = "";
            string message = "";

            while (true)
            {
                bool loginSuccessful = false;
                bool locked = false ;

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
                                        message = "Du är inloggad";
                                        loginSuccessful = true;
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
                    Console.WriteLine("\nTryck Enter för att gå vidare.");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }

                while (loginSuccessful)
                {
                    // Välkomstmeny
                    // Se konton och saldo. Öppna nytt konto. Överföra. Sätta in. Ta ut.
                    // Logga ut (tar en till "Välkommen till Banken"...)
                    Console.WriteLine("Välkommen!\n");
                    Console.WriteLine("[1] Se Konton och saldo");
                    Console.WriteLine("[2] Öppna nytt konto");
                    Console.WriteLine("[3] Överföring");
                    Console.WriteLine("[4] Insättning");
                    Console.WriteLine("[5] Uttag");
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
                                    Console.WriteLine(item.accountName);
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
                            break;
                        case "3":
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                        default:
                            Console.WriteLine("Måste jag göra en förändring i koden för att få göra en commit?");
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
            

            // Must make an object to be able to call the customers List
            
            Console.Clear();
        }

        public static void Login(List<string[]> customers, string username)
        {

        }
    }
}


/* TILL README

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