using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Loto.modules;

namespace Loto.FileManager
{
    public class UserFileManager
    {
        public static string filePath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\UsersData.txt";
        public static async void  WriteUser(User user)
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                await writer.WriteLineAsync(user.ToString());
                await writer.FlushAsync();
                writer.Close();
            }
        }

        public static string ReadUser(string username)
        {
            string line;
            using (StreamReader reader = new StreamReader(filePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Username") && line.Split("|")[0].Split(":")[1].Trim() == username)
                    { 
                        return line;
                    }
                }
                return null;
            }
        }

    }
}
