using System.Collections.Generic;

namespace Innumerati.Processes.Interfaces
{
    internal interface IRomanNumerals
    {
        Dictionary<string, int> Numerals { get; set; }

        string IntToNumeral(int input);
    }
}