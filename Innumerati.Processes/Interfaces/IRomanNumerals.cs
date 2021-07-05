using System.Collections.Generic;

namespace Innumerati.Processes.Interfaces
{
    internal interface IRomanNumerals
    {
        Dictionary<string, int> Numerals { get; set; }

        string GetLargestFittingNumeral(int input);

        string IntToNumeral(int input);

        int NumeralToInt(string input);
    }
}