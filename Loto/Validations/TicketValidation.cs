using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loto.Validations
{
    public class TicketValidation
    {

        public static void ValidateSaveOption(int SaveOption)
        {
            if (SaveOption < 1 || SaveOption > 2) { throw new Exception("Please enter 1 or 2"); }
        }

        public static void ValidateTicketSelection(int selectedTicketNumber)
        {
            if (selectedTicketNumber < 0 || selectedTicketNumber > 3) { throw new Exception("Please enter one number from 1 to 3"); }
        }

        public static void ValidateSystemCombinationSize(int combinationSize)
        {
            if (combinationSize < 7 || combinationSize > 16) { throw new Exception("Please enter one number from 7 to 16"); }
        }

        public static void ValidatePartialCombinationSize(int combinationSize)
        {
            if (combinationSize < 9 || combinationSize > 23) { throw new Exception("Please enter one number from 9 to 23"); }
        }

        public static void ValidateNumberOfCombination(int numberOfCombinations)
        {
            if (numberOfCombinations < 1 || numberOfCombinations > 10) { throw new Exception("One ticket should has minimum 1 and maximum 10 combinations"); }
        }

        public static void ValidateNumberOfJoker(int numberOfJoker)
        {
            if (numberOfJoker < 0 || numberOfJoker > 3) { throw new Exception("You can select maximum 3 Joker combinations per ticket or enter 0 to skip"); }
        }

        public static void ValidateCombinationMember(int combinationMember, int[] combination)
        {
            if (combinationMember < 1 || combinationMember > 39) { throw new Exception("Each number in combination shold be in range from 1 to 39"); }
            if (combination.Contains(combinationMember)) { throw new Exception("You already select this number in current combination"); }
        }

        public static void ValidateSupplementaryNumber(int combinationMember)
        {
            if (combinationMember < 1 || combinationMember > 39)
            {
                throw new Exception("Each number in combination shold be in range from 1 to 39");
            }
        }
    }
}
