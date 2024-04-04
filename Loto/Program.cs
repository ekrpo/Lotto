using System;
using Loto.modules;
using Loto.Modules;

namespace Loto
{
    class Program
    {
        static string ActiveUsername;
        static bool IsAdmin = false;
        static bool repeat = true;
        static Ticket ticket = new Ticket();

        static void Main(string[] args)
        {
            int AuthOption = ConsoleApp.SelectAuthOption();

            if (AuthOption == 1)  // Register
            {
                RegisterUser();
            }
            else if(AuthOption == 2) // Login
            {
                LoginUser();
            }

            if (IsAdmin)
            {
                CreateDraw();
            }
            else
            {
                CheckForWinAndCreateTicket();
            }
        }

        static void RegisterUser()
        {
            while (true)
            {
                try
                {
                    string username = ConsoleApp.EnterUsernameAtRegister();
                    string password = ConsoleApp.EnterPasswordAtRegister();
                    User NewUser = new User(username, password);
                    NewUser.RegisterUser();
                    ActiveUsername = username;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }

        static void LoginUser()
        {
            while (true)
            {
                try
                {
                    string username = ConsoleApp.EnterUsernameAtLogin();
                    string password = ConsoleApp.EnterPasswordAtLogin();
                    IsAdmin = User.LoginUser(username, password);
                    ActiveUsername = username;
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }

        static void CreateDraw()
        {
            if (ConsoleApp.EnterYesNo("Do you want to start draw? (1 - Yes 2 - No)") == 1)
            {
                Draw draw = new Draw(1);
                draw.GenerateLottoCombination();
                draw.GenerateSupplementaryNumber();
                draw.SetWinningJokerNumber();
                draw.SaveDraw();
                Console.WriteLine("Draw done!");
                Console.WriteLine(draw.FormatForFile());
            }
        }

        static void CheckForWinAndCreateTicket()
        {
            User.CheckForWin(ActiveUsername);
            while (repeat)
            {
                CreateTicket();
            }
        }

        static void CreateTicket()
        {
            int TicketType = ConsoleApp.SelectTicketType();
            int NumberOfCombinations, CombinationSize;

            switch (TicketType)
            {
                case 1:
                    repeat = false;
                    NumberOfCombinations = ConsoleApp.EnterNumberOfCombination();
                    CombinationSize = 6;
                    ticket = new Ticket(ActiveUsername, NumberOfCombinations, CombinationSize);
                    break;
                case 2:
                    repeat = false;
                    NumberOfCombinations = 1;
                    CombinationSize = ConsoleApp.EnterSystemCombinationSize();
                    ticket = new Ticket(ActiveUsername, NumberOfCombinations, CombinationSize);
                    break;
                case 3:
                    repeat = false;
                    NumberOfCombinations = 1;
                    CombinationSize = ConsoleApp.EnterPartialCombinationSize();
                    ticket = new Ticket(ActiveUsername, NumberOfCombinations, CombinationSize);
                    break;
                default:
                    break;
            }

            ticket.Combinations = ConsoleApp.EnterCombinations(ticket.NumberOfCombinations, ticket.CombinationSize);
            ticket.SupplementaryNumber = ConsoleApp.EnterSupplementaryNumber();
            ticket.NumberOfJokerCombinations = ConsoleApp.EnterNumberOfJoker();
            ticket.GenerateJokerCombinations();
            Console.WriteLine(ticket);

            int SaveOption = ConsoleApp.EnterYesNo("Do you want to save this ticket? (1 - Yes  2 - No)");

            if (SaveOption == 1)
            {
                ticket.SaveTicket();
                repeat = true;
                Console.WriteLine("\nTicket saved successfully\n");
            }
            else
            {
                repeat = true;
            }
        }
    }
}







