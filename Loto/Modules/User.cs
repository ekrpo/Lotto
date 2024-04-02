using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loto.CustomErrors;
using Loto.FileManager;
using Loto.Modules;
using Loto.Validations;

namespace Loto.modules
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            IsAdmin = false;
        }

        public void RegisterUser()
        {
            string dbUser = UserFileManager.ReadUser(Username);
            if (dbUser != null) throw new Exception("Username is already taken");
            UserFileManager.WriteUser(this);
            Console.WriteLine($"Wellcome {Username}, you registered to 'LOTO 6/39' successfully. Good luck!");
        }

        public static bool LoginUser(string username, string password)
        {
            string dbUser = UserFileManager.ReadUser(username);
            if (dbUser == null) throw new Exception("User with provided username not exist, please try again");
            string DbPassword = dbUser.Split("|")[1].Split(":")[1].Trim();
            bool IsAdmin = Convert.ToBoolean(dbUser.Split("|")[2].Split(":")[1].Trim()); 
            if (DbPassword != password) throw new Exception("Invalid password, please try again");
            Console.WriteLine($"Hello {username}, good luck!");
            return IsAdmin;
        }

        public static void CheckForWin(string username)
        {
            Draw lastDraw = DrawFileManager.LoadDraw();
            List<Ticket> tickets = TicketFileManager.LoadTickets(username);
            

            // Counters for Lotto, Supplementary, and Joker
            int lottoCounter = 0;
            int supplementaryCounter = 0;
            int jokerCounter = 0;
            bool IsWinning = false;

            Console.WriteLine("This is last draw: ");
            Console.WriteLine(lastDraw.FormatForFile());

            // Iterate over each ticket
            foreach (Ticket ticket in tickets)
            {
                if(ticket.RoundNumber != lastDraw.Round) { continue; };

                if (ticket.SupplementaryNumber == lastDraw.WinningSupplementaryNumber)
                {
                    supplementaryCounter++;
                }
                // Iterate over each combination in the ticket
                foreach (int[] combination in ticket.Combinations)
                {
                    // Check for Lotto and Supplementary numbers
                    foreach (int number in lastDraw.WinningLottoCombination)
                    {
                        if (combination.Contains(number))
                        {
                            lottoCounter++;
                        }
                    }
                    if (lottoCounter > 1 && supplementaryCounter == 0)
                    {
                        IsWinning = true;
                        Console.WriteLine($"Your gain on Lotto6/39 game is {lottoCounter}/6 on ticket: ");
                        Console.WriteLine(ticket);
                    }
                    else if (lottoCounter > 1 && supplementaryCounter == 1)
                    {
                        IsWinning = true;
                        Console.WriteLine($"Your gain on Lotto6/39 game is {lottoCounter} + {supplementaryCounter} on ticket: ");
                        Console.WriteLine(ticket);
                    }
                    lottoCounter = 0;
                    supplementaryCounter = 0;

                }



                foreach (int[] combination in ticket.JokerCombinations)
                {
                    // Check for Joker numbers
                    foreach (int number in lastDraw.WinningJokerCombination.Reverse())
                    {
                        foreach (int ticketNumber in combination.Reverse())
                        {
                            if (ticketNumber == number)
                            {
                                // If there's a match, increment jokerCounter
                                jokerCounter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (jokerCounter > 0)
                        {
                            // Print the gain and the corresponding ticket
                            Console.WriteLine($"Your gain on Joker game is {jokerCounter} on ticket: ");
                            Console.WriteLine(ticket);
                            // Reset the jokerCounter for the next iteration
                            jokerCounter = 0;
                        }
                    }
    
                }

            }
            if(!IsWinning)
            {
                Console.WriteLine("No winnings for today, better luck next time.");
                foreach(Ticket ticket in tickets)
                {
                    if (ticket.RoundNumber != lastDraw.Round) { continue; };
                    Console.WriteLine(ticket.ToString());
                }
            }
        }

        public override string ToString()
        {
            return $"Username: {Username} | Password: {Password} | isAdmin: {IsAdmin}";
        }
    }
}
