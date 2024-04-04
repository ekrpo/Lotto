using System;
using Loto.FileManager;

namespace Loto.Modules
{
    // Klasa namijenjena za podatke i metode vezane za izvlačenje
    public class Draw
    {
        public int Round { get; set; }
        public int[] WinningLottoCombination { get; set; } = new int[6];
        public int WinningSupplementaryNumber { get; set; }
        public int[] WinningJokerCombination { get; set; } = new int[6];

        // Metoda za random generiranje Lotto kombinacije 
        public void GenerateLottoCombination()
        {
            Random random = new Random();
            for (int i = 0; i < WinningLottoCombination.Length; i++)
            {
                WinningLottoCombination[i] = random.Next(1, 39);
            }
        }

        // Metoda za random generisanje dopunskog broja na Lottu
        public void GenerateSupplementaryNumber()
        {
            while (true)
            {
                Random random = new Random();
                int randomNumber = random.Next(1, 39); // Generiši slučajni broj
                if (WinningLottoCombination.Contains(randomNumber)) continue; // Ako je već izvučen u lotu ponovi sve
                WinningSupplementaryNumber = randomNumber;
                break;
            }

        }

        // Metoda kojom se iz igre Lotto vade jedinice izvučenih brojeva i formira Joker kombinacija
        public void SetWinningJokerNumber()
        {
            for (int i = 0; i < WinningLottoCombination.Length; i++)
            {
                // Ako je broj na Lottu veći od 9 uzima se druga cifra izvučenog broja u suprotnom uzima se cijeli broj
                WinningJokerCombination[i] = WinningLottoCombination[i] > 9 ? WinningLottoCombination[i] % 10 : WinningLottoCombination[i];
            }
        }

        // Metoda koja formatira Draw objekat za smiještanje u fajl
        public string FormatForFile()
        {
            return $@"
            Round:{Round}
            Date:{DateTime.Now.ToString("yyyy-MM-dd")}
            WinningSupplementaryNumber:{WinningSupplementaryNumber}
            LottoWinningCombination:{string.Join(" ", WinningLottoCombination)}
            JokerWinningCombination:{string.Join(" ", WinningJokerCombination)}
            #
            ";
        }

        // Metoda kojom se samo inicira spašavanje listića 
        public void SaveDraw()
        {
            DrawFileManager.SaveDraw(FormatForFile());
        }

        // Prazan konstruktor koji služi za eventualnu inicijalizaciju radi kasnijeg popunjavanja
        public Draw() { } 

        public Draw(int i)
        {
            Round = DrawFileManager.IncrementDrawRound().Result;
        }
    }
}
