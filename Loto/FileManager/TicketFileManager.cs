using System.Collections.Generic;
using System.IO;
using Loto.modules;

namespace Loto.FileManager
{
    // Klasa namijenjena za manipulaciju fajlovima za tikete
    public class TicketFileManager
    {
        // Putanja do fajla sa spasenim tiketima korisnika 
        private static string filePath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\Tickets.txt";

        // Metoda za spašavanje tiketa u fajl
        public static async void SaveTicket(string ticketFormat)
        {
            using (StreamWriter writer = File.AppendText(filePath)) // Dodatni tekst se dodaje na postojeći u fajl
            {
                await writer.WriteLineAsync(ticketFormat);
                await writer.FlushAsync();
            }
        }

        // Metoda za čitanja tiketa, smiještanje u listu i vraćanje liste tiketa
        public static List<Ticket> LoadTickets(string username)
        {
            List<Ticket> usersTickets = new List<Ticket>(); // Prazna lista tiketa

            using (StreamReader reader = new StreamReader(filePath))
            {
                Ticket ticket = new Ticket(); // Prazan objekat tiketa
                string line; // Spremnik za učitanu liniju
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Username")) // Uslovi koji ispituju da li tiket pripada prijavljenom korisniku
                    {
                        string fileUsername = line.Split(":")[1].Trim(); // Razdvoji 'key' i 'value' sa ':' i uzmi 'value'
                        if (fileUsername == username)
                        {
                            ticket.Username = fileUsername;  // Ako pripada smjesti username 'value' u prazan objekat
                        }
                    }
                    else if (line.Contains("#")) // '#' označava kraj jednog tiketa, ispitivanje kraja
                    {
                        usersTickets.Add(ticket); // Dodavanje popunjenog tiketa u listu
                        ticket = new Ticket(); // Ponovna inicijalizacija tiketa
                    }
                    else if (ticket.Username == username) // Uslov koji ispituje da li je username popunjen
                    {
                        ParseTicketLine(ticket, line); // Obrada pročitanih linija iz fajla
                    }

                }
            }
            return usersTickets;
        }

        private static void ParseTicketLine(Ticket ticket, string line)
        {
            // Ispitivanje da li linija posjeduje konkretan 'key'
            if (line.Contains("Round"))
            {
                ticket.RoundNumber = ExtractValue(line); // Poziv metode za vađenje vrijednosti iz stringa
            }
            else if (line.Contains("Supplementary Number"))
            {
                ticket.SupplementaryNumber = ExtractValue(line);
            }
            else if (line.Contains("*")) // '*' - prefix koji se pojavljuje ispred loto kombinacije
            {
                // Razdvajanje 'key' i 'value' sa ':' i manipulacija sa stringom kombinacije
                ticket.Combinations.Add(FormatCombinationsFromFile(line.Split(":")[1]));
            }
            else if (line.Contains("$")) // '$' - prefix koji se pojavljuje ispred joker kombinacije
            {
                ticket.JokerCombinations.Add(FormatCombinationsFromFile(line.Split(":")[1]));
            }
        }

        // Metoda za formatiranje kombinacije iz fajla 
        public static int[] FormatCombinationsFromFile(string combinationString)
        {
            // Vade se elementi kombinacije kao stringovi i pretvaraju se u brojeve
            string[] combinationChars = combinationString.Trim().Split();
            int[] combinationNumbers = new int[combinationChars.Length];
            for (int i = 0; i < combinationChars.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(combinationChars[i]))
                {
                    combinationNumbers[i] = int.Parse(combinationChars[i]);
                }
            }
            return combinationNumbers;
        }

        // Metoda za dobijanje vrijednosti koja se nalazi uz 'key' razdvojeni sa ':'
        private static int ExtractValue(string line)
        {
            return Convert.ToInt32(line.Split(":")[1]);
        }
    }
}

