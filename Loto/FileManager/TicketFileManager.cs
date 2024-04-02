using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loto.modules;


namespace Loto.FileManager
{
    public class TicketFileManager
    {
        public static string filePath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\Tickets.txt";
        public static async void SaveTicket(string TicketFormat)
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                await writer.WriteLineAsync(TicketFormat);
                await writer.FlushAsync();
                writer.Close();
            }
        }

        public static List<Ticket> LoadTickets(string username)
        {   
            string line;
            List<Ticket> UsersTickets = new List<Ticket>();
            Ticket ticket = new Ticket();
            using (StreamReader reader = new StreamReader(filePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Username"))
                    {
                        string FileUsername = line.Split(":")[1];
                        if (FileUsername.Trim() == username) ticket.Username = FileUsername;
                    }
                    else if (line.Contains("Round")) ticket.RoundNumber = Convert.ToInt32(line.Split(":")[1]);
                    else if (line.Contains("SupplemetaryNumber")) ticket.SupplementaryNumber = Convert.ToInt32(line.Split(":")[1]);
                    else if (line.Contains("*")) ticket.Combinations.Add(Ticket.FormatCombinationsFromFile(line.Split(":")[1]));
                    else if (line.Contains("$")) ticket.JokerCombinations.Add(Ticket.FormatCombinationsFromFile(line.Split(":")[1]));
                    else if (line.Contains("#")) {
                        UsersTickets.Add(ticket);
                        ticket = new Ticket();
                    }

                }
                return UsersTickets;
            }
        }
    }
}
