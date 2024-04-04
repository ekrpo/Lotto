using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loto.FileManager;

namespace Loto.modules
{
    public class Ticket
    {
        public string Username { get; set; }
        public int RoundNumber { get; set; }
        public int NumberOfCombinations { get; }
        public int CombinationSize { get; }
        public List<int[]> Combinations { get; set; } = new List<int[]>();
        public int SupplementaryNumber { get; set; }

        public int[] PrintedJokerNumber { get; }
        public int NumberOfJokerCombinations { get; set; }
        public List<int[]> JokerCombinations { get; } = new List<int[]>();

        private static readonly int minimumLottoNumber = 1;
        private static readonly int maximumLottoNumber = 39;
        private static readonly int minimumJokerNumber = 0;
        private static readonly int maximumJokerNumber = 9;

        public Ticket(string username, int numberOfCombinations, int combinationSize)
        {
            Username = username;
            NumberOfCombinations = numberOfCombinations;
            CombinationSize = combinationSize;
            PrintedJokerNumber = GenerateJokerCombination(6); // Joker kombinacija odštampana na listiću random
            RoundNumber = DrawFileManager.ReadDrawRound() + 1; // Listić odigran za sljedeće izvlačenje
        }

        public Ticket() { }

        // Metoda koja poziva metodu za generisanje Joker kombinacije (koliko puta je i odigrano)
        public void GenerateJokerCombinations()
        {
            for (int i = 0; i < NumberOfJokerCombinations; i++)
            {
                // Prvi Joker broj će biti već generisan uz listić
                if (i == 0)
                {
                    JokerCombinations.Add(PrintedJokerNumber);
                    continue;
                }
                // Ostali Joker brojevi bit će ovdje nasumično generisani
                JokerCombinations.Add(GenerateJokerCombination(6));
            }
        }

        // Metoda za generisanje pojedinačne Joker kombinacije 
        public static int[] GenerateJokerCombination(int size)
        {
            Random random = new Random();
            int[] combination = new int[size];
            for (int i = 0; i < size;)
            {
                int randomNumber = random.Next(minimumJokerNumber, maximumJokerNumber);
                if (combination.Contains(randomNumber)) continue; // Ne dozvoliti dva ista broja u jednoj kombinaciji
                combination[i] = randomNumber;
                i++;
            }
            return combination;
        }

        public static int[] GenerateLottoCombination(int size)
        {
            Random random = new Random();
            int[] combination = new int[size];
            for (int i = 0; i < size;)
            {
                int randomNumber = random.Next(minimumLottoNumber, maximumLottoNumber);
                if(combination.Contains(randomNumber)) continue; // Ne dozvoliti dva ista broja u jednoj kombinaciji
                combination[i] = randomNumber;
                i++;
            }
            return combination;
        }

        // Metoda koja inicijalizuje spašavanje Tiketa
        public void SaveTicket()
        {
            TicketFileManager.SaveTicket(FormatForFile());
        }

        // Metoda za random generisanje dopunskog broja od 1 do 39
        public static int GenerateSupplementaryNumber()
        {
            Random random = new Random();
            return random.Next(minimumLottoNumber, maximumLottoNumber);
        }

        // Metoda za formatiranje tiketa za fajl
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

        // Posebna metoda za formatiranje kombinacija(nizova) za fajl
        private string FormatCombinations(List<int[]> combinations, bool isLotto)
        {
            var formattedCombinations = new StringBuilder();
            for (int i = 0; i < combinations.Count; i++)
            {
                formattedCombinations.Append(isLotto ? "*Combination " : "$Combination ");
                formattedCombinations.Append($"{i + 1}: ");
                formattedCombinations.Append(string.Join(" ", combinations[i]));
                formattedCombinations.AppendLine();
                // Lotto ex. *Combination 1: 11 22 33 4 5 6
                // Joker ex. $Combination 1: 11 22 33 4 5 6
            }
            return formattedCombinations.ToString();
        }
public override string ToString()
{
return $@"#######################Ticket###############################

Round: {this.RoundNumber}       Owner: {this.Username}

Lotto Combinations
{FormatCombinations(this.Combinations, true)}
Supplementary number: {SupplementaryNumber.ToString() ?? "/"}

Joker Combinations
{FormatCombinations(this.JokerCombinations, false)}
#######################Ticket###############################";
        }
    }


}
