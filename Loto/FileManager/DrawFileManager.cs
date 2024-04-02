using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loto.modules;
using Loto.Modules;

namespace Loto.FileManager
{
    public class DrawFileManager
    {
        public static string LastDrawDataPath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\LastDrawData.txt";
        public static string DrawsPath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\Draws.txt";
        public static async Task<int> IncrementDrawRound()
        {
            int LastRoundValue = ReadDrawRound();
            int CurrentRoundValue = LastRoundValue + 1;
            using (StreamWriter writer = File.CreateText(LastDrawDataPath))
            {
                await writer.WriteLineAsync($"RoundNumber={CurrentRoundValue}");
                await writer.FlushAsync();
                writer.Close();
            }
            return CurrentRoundValue;
        }

        public static int ReadDrawRound()
        {
            int RoundNumber;
            using (StreamReader reader = new StreamReader(LastDrawDataPath))
            {
                RoundNumber = Convert.ToInt32(reader.ReadLine().Split("=")[1]);
            }
            return RoundNumber;
        }

        public static async void SaveDraw(string DrawText)
        {
            using (StreamWriter writer = File.CreateText("C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\Draws.txt"))
            {
                await writer.WriteLineAsync(DrawText);
                await writer.FlushAsync();
                writer.Close();
            }
        }

        public static Draw LoadDraw() {

            string line;
            int i = 0;
            Draw draw = new Draw();
            using (StreamReader reader = new StreamReader("C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\Draws.txt"))
            {
                while ((line = reader.ReadLine()) != null) {
                    if (line.Contains("Round")) draw.Round = Convert.ToInt32(line.Split(":")[1]);
                    if (line.Contains("WinningSupplementaryNumber")) draw.WinningSupplementaryNumber = Convert.ToInt32(line.Split(":")[1]);
                    if (line.Contains("LottoWinningCombination") || line.Contains("JokerWinningCombination"))
                    {
                        string[] CombinationString = line.Split(":")[1].Split(" "); 
                        int[] WinningCombination = new int[CombinationString.Length];
                        int counter = 0;
                        foreach(string StringNum in CombinationString)
                        {
                            int Num = Convert.ToInt32(StringNum);
                            WinningCombination[counter] = Num;
                            counter++;
                        }
                        if (line.Contains("LottoWinningCombination")) draw.WinningLottoCombination = WinningCombination;
                        else draw.WinningJokerCombination = WinningCombination;
                    } 
                }
            }
            return draw;
        }


    }
}
