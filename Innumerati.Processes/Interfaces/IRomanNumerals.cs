using System.Collections.Generic;

namespace Innumerati.Processes.Interfaces
{
    public interface IRomanNumerals
    {
        Dictionary<string, int> Numerals { get; set; }

        string GetLargestFittingNumeral(int input);

        string GetSubtractiveCandidate(int input);

        string IntToNumerals(int input);

        bool IsValidRomanNumeral(string input);

        Dictionary<int, string> ListAll();

        int NumeralsToInt(string input);
    }
}