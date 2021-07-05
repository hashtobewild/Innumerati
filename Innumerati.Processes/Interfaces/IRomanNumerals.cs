using System.Collections.Generic;

namespace Innumerati.Processes.Interfaces
{
    internal interface IRomanNumerals
    {
        Dictionary<string, int> Numerals { get; set; }

        string GetLargestFittingNumeral(int input);

        string IntToNumerals(int input);

        bool IsValidRomanNumeral(string input);

        int NumeralsToInt(string input);
    }
}