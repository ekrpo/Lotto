using System.IO;
using Loto.modules;
using Loto.Modules;

namespace Loto.FileManager
{
    // Klasa za manipulaciju fajlovima za korisnike
    public class UserFileManager
    {
        // Putanja do fajla za smiještanje podataka o korisnicima
        private static string filePath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\UsersData.txt";

        // Metoda za upis podataka o korisniku u fajl (prethodno formatiran)
        public static async Task WriteUser(User user)
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                await writer.WriteLineAsync(user.ToString());
                await writer.FlushAsync();
            }
        }

        // Metoda za čitanje podataka o korisnuku iz fajla
        public static string ReadUser(string username)
        {
            string line; // Spremnik za liniju na kojoj se čitač trenutno nalazi
            using (StreamReader reader = new StreamReader(filePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    // Ako je username isti kao proslijeđeni parametar vrati cijelu liniju(string)
                    if (line.Contains("Username") && GetUsernameFromLine(line) == username)
                    {
                        return line;
                    }
                }
                return null;
            }
        }

        // Metoda za čitanje vrijednosti username-a iz linije u fajlu
        private static string GetUsernameFromLine(string line)
        {
            // ex. Username: test | Password: testpass | IsAdmin: False
            return line.Split("|")[0].Split(":")[1].Trim();
        }
    }
}

