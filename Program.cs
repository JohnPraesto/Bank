namespace Bank
{
    internal class Program
    {
        /* NÄSTA STEG ÄR ATT lösa konton

        Först lista med array bestående av användare + lösen. Men hur binda till konton?
        Börjar överväga en kund-klass-objekt. Och göra en lista av alla objekt.
        Struktur:
        string namn
        string lösen
        int inloggningsförsök
        
        eller hmm. kanske behålla användare och lösen som det är nu.
        och bara göra objekt för konton och dess saldon?
        string kontoNamn;
        int summa;
        och sen göra en lista med konto-objekt. Men hur koppla det till rätt användare?

        Man kan göra en och samma lista för alla startade konton. Men man lägger till
        string user;
        i klassen
        så när inloggar användare vill se sina konton så
        foreach konto in kontolista
            if konto.user == inloggad användare
                write konto.kontoNamn
                write konto.summa
         * 
        utreda hur man sparar mellan körningar

        ska inte gå att registrera samma användarnamn flera gånger.
        
        Vore kul att välja bindningstid på sparkontot och lägga till olika räntor





        */


        static void Main(string[] args)
        {
            List<string[]> customers = new List<string[]>();
            

            while (true)
            {
                bool welcomeMenu = false;
                Console.Clear();
                Console.WriteLine("Välkommen till Banken\n");
                Console.WriteLine("[1] Vill du registrera dig som ny kund hos oss?");
                Console.WriteLine("[2] Logga in.");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    NewCustomer(customers);
                }
                else if (choice == "2")
                {
                    Login(customers);
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }

                while (welcomeMenu)
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
                    switch (choice)
                    {
                        case "1":
                            break;
                        case "2":
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
                            break;
                    }
                }
            }

        }
        public static void NewCustomer(List<string[]> customers)
        {
            string[] customer = new string[2];

            Console.Clear();
            Console.WriteLine("Vad ska du ha för användarnamn?");
            customer[0] = Console.ReadLine();
            Console.WriteLine("Vad ska du ha för lösenord?"); // Would be cool to convert inserted characters to *
            customer[1] = Console.ReadLine();

            // Must make an object to be able to call the customers List
            customers.Add(customer);
        }

        public static bool Login(List<string[]> customers)
        {
            Console.Clear();
            Console.Write("Ange användarnamn: ");
            // TIMEWARP... här! Check banned list.
            string username = Console.ReadLine();
            int loginCount = 0;
            bool loginSuccessful = false;

            // Kontrollera att användarnamnet finns i customer List
            foreach (string[] item in customers)
            {
                if (username == item[0])
                {
                    Console.Write("Ange ditt lösenord: ");
                    string password = Console.ReadLine();
                    if (password == item[1])
                    {
                        Console.WriteLine("Du är inloggad!");
                        loginSuccessful = true;
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Fel lösenord!");
                        Console.ReadLine();
                        // Efter tre försök så läggs aktuellt användarnamn till
                        // banned list, som är en array
                        // denna array använs vid inlog sedan. Typ ... TIMEWARP
                    }
                }
            }
            return loginSuccessful;
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