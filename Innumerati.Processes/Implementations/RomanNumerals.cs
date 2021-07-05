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

        /// <summary>
        /// Convert integers to roman numerals.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The roman numeral string that represents the integer</returns>
        public string IntToNumerals(int input)
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

        /// <summary>
        /// Determines whether the input is a valid roman numeral string.
        /// Validations from: https://www.cuemath.com/numbers/roman-numerals/ (Rules for Writing Roman Numerals)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [is valid roman numeral] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidRomanNumeral(string input)
        {
            // Expanded notation for clarity
            if (!string.IsNullOrEmpty(input))
            {
                var workingString = input.ToUpperInvariant();
                var workingArray = workingString.ToCharArray();

                // Only includes known characters
                if (workingArray.All(x => Numerals.ContainsKey(CharToString(x))))
                {
                    if (
                        // L,V,D cannot repeat:
                        workingString.Count(x => x == 'L') <= 1
                        && workingString.Count(x => x == 'V') <= 1
                        && workingString.Count(x => x == 'D') <= 1
                        // I,X,C can be repeated max 3 times
                        && workingString.Count(x => x == 'I') <= 3
                        && workingString.Count(x => x == 'X') <= 3
                        && workingString.Count(x => x == 'C') <= 3
                        )
                    {
                        // Values must decrease (additive) except I,X,C which can be subtracive
                        string[] subtractives = new string[] { "I", "X", "C" };
                        // Assume true, unless proven false
                        bool charsValid = true;

                        for (int i = 0; i < workingArray.Length; i++)
                        {
                            if (i + 1 < workingArray.Length)
                            {
                                var currentValue = Numerals[CharToString(workingArray[i])];
                                var nextNextValue = Numerals[CharToString(workingArray[i + 1])];

                                if (currentValue < nextNextValue && !subtractives.Contains(CharToString(workingArray[i])))
                                {
                                    charsValid = false;
                                }
                            }
                        }
                        return charsValid;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Convert roman numerals to integers.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The integer represented by the input roman numerals</returns>
        public int NumeralsToInt(string input)
        {
            // Add character items to a FIFO queue
            Queue<char> queue = new Queue<char>(input.ToCharArray());

            char workingChar;
            int workingValue = 0;
            while (queue.TryDequeue(out workingChar))
            {
                var working = CharToString(workingChar);
                workingValue += Numerals[working];
            }
            return workingValue;
        }

        private string CharToString(char input)
        {
            return new string(new char[] { input });
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