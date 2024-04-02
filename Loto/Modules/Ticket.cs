using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Loto.FileManager;

namespace Loto.modules
{
    public class Ticket
    {
        public string Username;
        public int RoundNumber;
        public int NumberOfCombinations { get; }
        public int CombinationSize { get; }
        public List<int[]> Combinations = new List<int[]>();
        public int SupplementaryNumber;

        public int[] PrintedJokerNumber { get; }
        public int NumberOfJokerCombinations;
        public List<int[]> JokerCombinations = new List<int[]>();


        private static int minimumLottoNumber = 1;
        private static int maximumLottoNumber = 39;
        private static int minimumJokerNumber = 0;
        private static int maximumJokerNumber = 9;

        private int[] GenerateJokerCombination(int size)
        {
            Random random = new Random();
            int[] combination = new int[size];
            for (int i = 0; i < size; i++)
            {
                int RandomNumber = random.Next(minimumJokerNumber, maximumJokerNumber);
                combination[i] = RandomNumber;
            }
            return combination;
        }

        public void GenerateJokerCombinations()
        { 
            for(int i = 0; i < NumberOfJokerCombinations; i++)
            {
                if (i == 0)
                {
                    JokerCombinations.Add(PrintedJokerNumber);
                    continue;
                }
                JokerCombinations.Add(GenerateJokerCombination(6));
            }
        }
        public static int[] GenerateLottoCombination(int size)
        {
            Random random = new Random();
            int[] combination = new int[size];
            for (int i = 0; i < size;)
            {
                int RandomNumber = random.Next(minimumLottoNumber, maximumLottoNumber);
                if (combination.Contains(RandomNumber)) continue;
                combination[i] = RandomNumber;
                i++;
            }
            return combination; 
        }
        public static string FormatCombinations(List<int[]> combinations, bool IsLotto)
        {
            string FormatedCombinations = "";
            for (int i = 0; i < combinations.Count; i++)
            {
                FormatedCombinations += IsLotto ? $"*Combination {i + 1}:" : $"$Combination {i + 1}:";
                foreach (int num in combinations[i])
                {
                    FormatedCombinations += $"{num}  ";
                }
                FormatedCombinations += "\n            ";
            }
            return FormatedCombinations;
        }

        public string FormatForFile()
        {
            return $@"
            Username:{Username}
            Round:{RoundNumber}
            Lotto Combinations: 
            {FormatCombinations(Combinations, true)}
            Supplementary Number:{SupplementaryNumber}
            Joker Combinations:
            {FormatCombinations(JokerCombinations, false)}
            #
            ";
        }

        public static int[] FormatCombinationsFromFile(string CombinationString)
        {
            string[] CombinationChars = CombinationString.Split("  "); 
            int[] CombinationNumbers = new int[CombinationChars.Length];
            for (int i = 0; i<CombinationChars.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(CombinationChars[i]))
                {
                    CombinationNumbers[i] = int.Parse(CombinationChars[i]);
                }
            }
            return CombinationNumbers;
        }

        public void SaveTicket()
        {
            TicketFileManager.SaveTicket(FormatForFile());
        }

        public static int GenerateSupplementaryNumber()
        {
            Random random = new Random();
            return random.Next(minimumLottoNumber, maximumLottoNumber);
        }

        public Ticket(string username, int numberOfCombinations, int combinationSize)
        {
            this.Username = username;
            this.PrintedJokerNumber = this.GenerateJokerCombination(6);
            this.RoundNumber = DrawFileManager.ReadDrawRound() + 1;
            this.NumberOfCombinations = numberOfCombinations;
            this.CombinationSize = combinationSize;
        }


        public Ticket(string username, List<int[]> combinations, int supplementaryNumber, List<int[]> jokerCombinations, int roundNumber)
        {
            Username = username;
            Combinations = combinations;
            SupplementaryNumber = supplementaryNumber;
            JokerCombinations = jokerCombinations;
            RoundNumber = roundNumber;
        }

        public Ticket(){ }

        public override string ToString()
        {
            return $@"#######################Ticket###############################
            Round: {this.RoundNumber}       Owner: {this.Username}

            Lotto Combinations
            {FormatCombinations(this.Combinations, true)}
            Supplementary number: {this.SupplementaryNumber.ToString() ?? "/"}

            Joker Combinations
            {FormatCombinations(this.JokerCombinations, false)}
#######################Ticket###############################";
        }
    }
}
