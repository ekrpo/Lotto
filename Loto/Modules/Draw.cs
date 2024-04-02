using Loto.FileManager;

namespace Loto.Modules
{
    public class Draw
    {
        public int Round;
        public int[] WinningLottoCombination = new int[6];
        public int WinningSupplementaryNumber;
        public int[] WinningJokerCombination = new int[6];

        public void GenerateLottoCombination()
        {
            Random random = new Random();
            int[] combination = new int[6];
            for (int i = 0; i < 6; i++)
            {
                int RandomNumber = random.Next(1, 39);
                combination[i] = RandomNumber;
            }
            WinningLottoCombination = combination;
        }

        public void GenerateSupplementaryNumber()
        {
            Random random = new Random();
            WinningSupplementaryNumber = random.Next(1, 39);
        }

        public void SetWinningJokerNomber()
        {
            int[] JokerCombination = new int[6];
            int counter = 0;
            foreach (int Num in WinningLottoCombination)
            {
                int JokerNum = Num>9 ? Num % 10 : Num;
                JokerCombination[counter] = JokerNum;
                counter++;
            }
            WinningJokerCombination = JokerCombination;
        }

        public string FormatForFile()
        {
            return $@"
            Round:{Round}
            Date:{DateTime.Now.ToString("yyyy-MM-dd")}
            WinningSupplementaryNumber:{WinningSupplementaryNumber}
            LottoWinningCombination:{WinningLottoCombination[0]} {WinningLottoCombination[1]} {WinningLottoCombination[2]} {WinningLottoCombination[3]} {WinningLottoCombination[4]} {WinningLottoCombination[5]}
            JokerWinningCombination:{WinningJokerCombination[0]} {WinningJokerCombination[1]} {WinningJokerCombination[2]} {WinningJokerCombination[3]} {WinningJokerCombination[4]} {WinningJokerCombination[5]}
            #
            ";
        }

        public void SaveDraw()
        {
            DrawFileManager.SaveDraw(FormatForFile());
        }

        public Draw() { }
        public Draw(int i) {
            Round = DrawFileManager.IncrementDrawRound().Result;
        }

    }

}
