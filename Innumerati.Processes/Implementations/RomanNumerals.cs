using Innumerati.Processes.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Innumerati.Processes.Implementations
{
    public class RomanNumerals : IRomanNumerals
    {
        private Dictionary<string, int> _numerals;

        public RomanNumerals()
        {
            DefaultFactory();
        }

        /// <summary>
        /// Gets or sets the numerals that are known to the application.
        /// </summary>
        /// <value>
        /// The numerals.
        /// </value>
        public Dictionary<string, int> Numerals { get => _numerals; set => _numerals = value; }

        public string GetLargestFittingNumeral(int input)
        {
            var x = Numerals.Where(x => x.Value <= input).OrderByDescending(x => x.Value).First().Key;
            return x;
        }

        public string IntToNumeral(int input)
        {
            var working = input;
            string output = string.Empty;

            while (working > 0)
            {
                var temp = GetLargestFittingNumeral(working);
                output += temp;
                working -= Numerals[temp];
            }
            return output;
        }

        public int NumeralToInt(string input)
        {
            var x = Numerals[input];
            return x;
        }

        /// <summary>
        /// Initializes the default values
        /// </summary>
        private void DefaultFactory()
        {
            Numerals = new Dictionary<string, int>()
            {
                {"I", 1},
                {"V", 5},
                {"X", 10},
                {"L", 50},
                {"C", 100},
                {"D", 500},
                {"M", 1000},
            };
        }
    }
}