using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Loto.modules;
using Loto.Validations;

namespace Loto
{
    /*
        KLASA ZA INTERAKCIJU KORISNIKA KROZ KONZOLU
        Pattern svih metoda u klasi:
        Obično sadrži metode koje daku upute korisniku za unos
        Potom omogućavaju korisniku unos i pretvaraju u neki tip podataka
        Potom validiraju korisnički unos
        A zatim obavještavaju o rezultatu unosa
        Metoda se ponavlaj sve dok korisnik ne unese ispravno 
        Gresške se hvataju i ispisuju bez prekida programa
    */
    public class ConsoleApp
    {
        public static int EnterNumberOfCombination()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("How many combinations of LOTO 6/39 you want to play? (1 - 10)");
                    int NumberOfCombinations = Convert.ToInt32(Console.ReadLine());
                    TicketValidation.ValidateNumberOfCombination(NumberOfCombinations);
                    return NumberOfCombinations;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

        }

        public static int EnterSystemCombinationSize()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("How many numbers in combination you want? (7 - 16)");
                    int CombinationSize = Convert.ToInt32(Console.ReadLine());
                    TicketValidation.ValidateSystemCombinationSize(CombinationSize);
                    return CombinationSize;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

        }

        public static int EnterPartialCombinationSize()
        {
            while(true)
            {
                try
                {
                    Console.WriteLine("How many numbers in combination you want? (9 - 23)");
                    int CombinationSize = Convert.ToInt32(Console.ReadLine());
                    TicketValidation.ValidatePartialCombinationSize(CombinationSize);
                    return CombinationSize;
                }
                catch(Exception ex) { Console.WriteLine(ex.Message); }
            }

        }

        public static int EnterYesNo(string message)
        {
            while (true)
            {
                try
                {

                    Console.WriteLine(message);
                    int SaveOption = Convert.ToInt32(Console.ReadLine());
                    TicketValidation.ValidateSaveOption(SaveOption);
                    return SaveOption;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue; 
                }
            }
           
        }

        private static int[] EnterCombination(int size)
        {
            while (true)
            {
                try
                {
                    int[] Combination = new int[size];
                    if (EnterYesNo("Do you want to random generate this combination? (1 - Yes  2 - No)") == 1)
                    {
                        Combination = Ticket.GenerateLottoCombination(size);
                    }
                    else
                    {
                        int i = 0;
                        Console.WriteLine("Enter numbers in your combination");
                        while (i < size)
                        {
                            Console.Write($"Enter {i + 1} number: ");
                            try
                            {
                                int EnteredNumber = Convert.ToInt32(Console.ReadLine());
                                TicketValidation.ValidateCombinationMember(EnteredNumber, Combination);
                                Combination[i] = EnteredNumber;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                continue;
                            }
                            i++;

                        }
                    }
                    return Combination;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            
            

        }

        public static List<int[]> EnterCombinations(int numberOfCombinations, int combinationSize)
        {
            List<int[]> combinations = new List<int[]>();   
            for(int i = 0; i<numberOfCombinations; i++)
            {
                Console.WriteLine($"Enter your {i+1} combination");
                int[] Combination = EnterCombination(combinationSize);
                combinations.Add(Combination);
            }
            return combinations;
        }

        public static int EnterNumberOfJoker()
        {
            while (true)
            {
                Console.Write("How many combinations you want to play in Joker game?\n(1 - 3, 0 - if you will not play): ");
                try { 
                    int NumberOfJoker = Convert.ToInt32(Console.ReadLine()); 
                    TicketValidation.ValidateNumberOfJoker(NumberOfJoker); 
                    return NumberOfJoker;
                }
                catch (Exception e) { 
                    Console.WriteLine(e.Message); 
                    continue; 
                }
            }
        }

        public static int EnterSupplementaryNumber()
        {
            if (EnterYesNo("Do you want to automatic generate supplementary number? (1 - Yes  2 - No)") == 1)
            {
                return Ticket.GenerateSupplementaryNumber();
            }
            while (true)
            {
                Console.Write("Enter Supplementary Number (1 - 39, 0 - to avoid pick): ");
                try { 
                    int SupplementaryNumber = Convert.ToInt32(Console.ReadLine());
                    TicketValidation.ValidateSupplementaryNumber(SupplementaryNumber);  
                    return SupplementaryNumber;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        }

        public static string EnterUsernameAtRegister()
        {
            while (true)
            {
                Console.Write("Enter your username: ");
                try { 
                    string username = Console.ReadLine(); 
                    UserValidation.ValidateUsernameAtRegister(username);    
                    return username;    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        }

        public static string EnterUsernameAtLogin()
        {
            while (true)
            {
                Console.Write("Enter your username: ");
                try { 
                    string username = Console.ReadLine(); 
                    UserValidation.ValidateUsernameAtLogin(username);
                    return username;    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        }

        public static string EnterPasswordAtRegister()
        {
            while (true)
            {  
                try {
                    Console.Write("Enter your password: ");
                    string EnteredPassword = Console.ReadLine();
                    Console.Write("Confirm entered password: ");
                    string ConfirmedPassword = Console.ReadLine();
                    UserValidation.ValidatePasswordAtRegister(EnteredPassword, ConfirmedPassword);
                    return EnteredPassword;
                }
                catch (Exception e) { Console.WriteLine(e.Message); continue; }

            }
        }

        public static string EnterPasswordAtLogin()
        {
            while (true)
            {
                Console.Write("Confirm your password: ");
                try { 
                    string password = Console.ReadLine(); 
                    UserValidation.ValidatePasswordAtLogin(password);
                    return password;    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        }

        public static int SelectTicketType()
        {
            while (true)
            {
                Console.WriteLine("Enter number next to ticket type you want.");
                Console.WriteLine("1 - Classic Ticket\n2 - System Ticket\n3 - Partial Ticket");
                try { 
                    int TicketTypeNumber = Convert.ToInt32(Console.ReadLine()); 
                    TicketValidation.ValidateTicketSelection(TicketTypeNumber); 
                    return TicketTypeNumber;    
                }
                catch (Exception e) { Console.WriteLine(e.Message); continue; }
            }

        }

        public static int SelectAuthOption()
        {
            while (true)
            {
                Console.WriteLine("Select Register or Login option: 1 - Register 2 - Login: ");
                try
                {
                    int OptionNumber = int.Parse(Console.ReadLine());
                    if(OptionNumber != 1 && OptionNumber != 2)
                    {
                        Console.WriteLine("Invalid input, enter 1 or 2");
                        continue;
                    }
                    return OptionNumber;
                }
                catch (Exception e) { Console.WriteLine(e.Message); continue; }
            }

        }

    }
}
