 using System;
using System.Collections.Generic;
using System.Linq;
using Loto.FileManager;
using Loto.modules;
using Loto.Modules;
using Loto.Validations;

namespace Loto.Modules
{
    // Klasa za korisničke metode i svojstva 
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

        // Metoda za registrovanje novog korisnika
        public void RegisterUser()
        {
            if (UserExists(Username)) // Ako korisnik već postoji izbaci grešku
                throw new Exception("Username is already taken");
            // Inače spasi ga u fajl
            UserFileManager.WriteUser(this);
            Console.WriteLine($"Welcome {Username}, you registered to 'LOTO 6/39' successfully. Good luck!");
        }

        // Metoda za prijavu već registriranih korisnika
        public static bool LoginUser(string username, string password)
        {
            // Podaci o korisniku iz fajla sa unesenim usrername-om
            string dbUser = UserFileManager.ReadUser(username);
            if (dbUser == null)
                throw new Exception("User with provided username not exist, please try again");

            string dbPassword = dbUser.Split("|")[1].Split(":")[1].Trim(); // Izvlačenje vrijednosti passworda iz string-a
            bool isAdmin = Convert.ToBoolean(dbUser.Split("|")[2].Split(":")[1].Trim()); // Izvlačenje vrijednosti Admin iz string-a

            if (dbPassword != password) // Da li je uneseni password isti kao u bazi
                throw new Exception("Invalid password, please try again");

            Console.WriteLine($"Hello {username}, good luck!");
            return isAdmin;
        }

        // Metoda koja provjerava dobitke korisnika u zadnjem kolu
        public static void CheckForWin(string username)
        {
            // Load the last draw and user's tickets
            Draw lastDraw = DrawFileManager.LoadDrawFromFile();
            List<Ticket> tickets = TicketFileManager.LoadTickets(username);

            // Prikazivanje informacija o zadnjem izvlačenju
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!LAST DRAW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            Console.WriteLine(lastDraw.FormatForFile());
            Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!LAST DRAW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n");
            // Varijabla koja bilježi da li uopšte ima dobitaka
            bool isWinning = false;

            // Prolazak kroz sve tikete
            foreach (Ticket ticket in tickets)
            {
                // Provjeriti koji tiketi imaju broj kola zadnjeg izvlačenja
                if (ticket.RoundNumber != lastDraw.Round)
                    continue;

                // Provjeriti Loto kombinacije
                int lottoHits = CountHits(ticket.Combinations, lastDraw.WinningLottoCombination);
                int supplementaryHit = ticket.SupplementaryNumber == lastDraw.WinningSupplementaryNumber ? 1 : 0;

                // Provjeriti da li postoje pogotci na lotu bez dodatnog broja
                if (lottoHits > 1 && supplementaryHit == 0)
                {
                    isWinning = true;
                    Console.WriteLine($"Congratulations! You have {lottoHits} hits on Lotto 6/39 on ticket:\n");
                    Console.WriteLine(ticket);
                }
                else if (lottoHits > 1 && supplementaryHit == 1) // Provjeriti da li postoje pogotci na lotu sa dodatnim brojem
                {
                    isWinning = true;
                    Console.WriteLine($"Congratulations! You have {lottoHits} + {supplementaryHit} hits on Lotto 6/39 on ticket:\n");
                    Console.WriteLine(ticket);
                }

                // Provjeri kombinacije na igri Joker
                int jokerHits = CountJokerHits(ticket.JokerCombinations, lastDraw.WinningJokerCombination);

                if (jokerHits > 0)
                {
                    isWinning = true;
                    Console.WriteLine($"Congratulations! You've won Joker with {jokerHits} numbers on ticket:\n");
                    Console.WriteLine(ticket);
                }
            }

            // Ako ne postoji nikakav dobitak
            if (!isWinning)
            {
                Console.WriteLine("No winnings for today. Better luck next time!");
            }
        }

        // Pomoćna metoda za brojanje Loto pogadaka
        private static int CountHits(List<int[]> ticketCombinations, int[] winningCombination)
        {
            int hits = 0;
            foreach (int[] combination in ticketCombinations)
            {
                foreach (int number in combination)
                {
                    if (winningCombination.Contains(number))
                    {
                        hits++;
                    }
                }
            }
            return hits;
        }

        // Pomoćna metoda za brojanje Joker pogodaka
        private static int CountJokerHits(List<int[]> ticketCombinations, int[] winningJokerCombination)
        {
            int hits = 0;
            for(int i = 0; i<ticketCombinations.Count();)
            {              
                for (int j = winningJokerCombination.Count()-1; j>0; j--)
                {
                    if (ticketCombinations[i][j] == winningJokerCombination[j])
                    {
                        hits++;
                    }
                    else
                    {
                        break;
                    }
                    
                }
            }
            return hits;
        }


        // Metoda koja provjerava da li korisnik sa username već postoji
        private static bool UserExists(string username)
        {
            string dbUser = UserFileManager.ReadUser(username);
            return dbUser != null;
        }

        // Metoda koja na poziv objekta vraća formatiran string za fajl .txt
        public override string ToString()
        {
            return $"Username: {Username} | Password: {Password} | isAdmin: {IsAdmin}";
        }
    }
}

