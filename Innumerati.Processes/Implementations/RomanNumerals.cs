using Innumerati.Processes.Interfaces;
using System;
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

        public int GetSmallestNonFittingNumeralValue(int input)
        {
            var x = Numerals.Where(x => x.Value >= input).OrderBy(x => x.Value).First().Value;
            return x;
        }

        public string GetSubtractiveCandidate(int input)
        {
            string output = string.Empty;

            if (input >= 3 && input < Numerals.Values.Max())
            {
                var nextNumeralValue = GetSmallestNonFittingNumeralValue(input);
                var smallestNonFitting = Numerals.First(x => x.Value == nextNumeralValue).Key;
                var largestFitting = GetLargestFittingNumeral(input);
                var previousNumeralValue = Numerals[largestFitting];
                var delta = nextNumeralValue - input;
                if (
                    input != 4 // fixed exception to the rules
                    && (delta <= 0
                    || delta >= input
                    || delta > previousNumeralValue
                    ))
                {
                    return output;
                }
                else if (delta == 1 && smallestNonFitting != "I")
                {
                    output = "I" + smallestNonFitting;
                }
                else if (delta <= 10 && smallestNonFitting != "X")
                {
                    output = "X" + smallestNonFitting;
                }
                else if (delta > 10 && delta <= 100 && smallestNonFitting != "C")
                {
                    output = "C" + smallestNonFitting;
                }
            }
            return output;
        }

        /// <summary>
        /// Convert integers to roman numerals.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The roman numeral string that represents the integer</returns>
        public string IntToNumerals(int input)
        {
            if (!IsValidInteger(input))
            {
                throw new System.InvalidOperationException("This is an invalid input");
            }

            var working = input;

            string output = string.Empty;

            while (working > 0)
            {
                string temp = string.Empty;
                var sub = GetSubtractiveCandidate(working);
                int deduction = 0;
                if (!string.IsNullOrEmpty(sub))
                {
                    temp = sub;
                    var major = Numerals[CharToString(temp.Last())];
                    for (int i = 0; i < temp.Length - 1; i++)
                    {
                        major -= Numerals[CharToString(temp[i])];
                    }
                    deduction = major;
                }
                else
                {
                    temp = GetLargestFittingNumeral(working);
                    deduction = Numerals[temp];
                }

                output += temp;
                working -= deduction;
            }

            // Sanity check
            if (!IsValidRomanNumeral(output))
            {
                //throw new System.InvalidOperationException("This number produces an invalid roman numeral sequence");
            }
            return output;
        }

        public bool IsValidInteger(int input)
        {
            return input > 0
                && input < 4000;
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
            if (!string.IsNullOrEmpty(input))
            {
                var workingString = input.ToUpperInvariant();
                var workingArray = workingString.ToCharArray();

                // Only includes known characters
                if (workingArray.All(x => Numerals.ContainsKey(CharToString(x))))
                {
                    if (IsValidNumeralOccurance(workingString))
                    {
                        return IsValidNumeralOrdering(workingArray);
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
            if (!IsValidRomanNumeral(input))
            {
                throw new System.InvalidOperationException("Invalid Roman Numeral String");
            }

            // Add character items to a FIFO queue
            Queue<char> queue = new Queue<char>(input.ToCharArray());

            char workingChar;
            int workingValue = 0;
            while (queue.TryDequeue(out workingChar))
            {
                var working = CharToString(workingChar);
                workingValue += Numerals[working];
            }

            if (!IsValidInteger(workingValue))
            {
                // this is here for paranoia's sake and should not generally be possible to reach.
                // It could be reachable, if the code or memory was tampered with though
                throw new System.InvalidOperationException("This roman numeral produces an invalid integer");
            }
            return workingValue;
        }

        /// <summary>
        /// Utility to generate a string from a character
        /// </summary>
        /// <param name="input">The input character.</param>
        /// <returns>A string made up of that character</returns>
        private string CharToString(char input)
        {
            return new string(new char[] { input });
        }

        /// <summary>
        /// Initializes the default values /set initial state
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

        private bool IsValidNumeralOccurance(string input)
        {
            return
                // L,V,D cannot repeat:
                input.Count(x => x == 'L') <= 1
                && input.Count(x => x == 'V') <= 1
                && input.Count(x => x == 'D') <= 1
                // I,X,C can be repeated max 3 times
                && input.Count(x => x == 'I') <= 3
                && input.Count(x => x == 'X') <= 3
                && input.Count(x => x == 'C') <= 3;
        }

        private bool IsValidNumeralOrdering(char[] inputArray)
        {
            // Values must decrease (additive) except I,X,C which can be subtracive
            string[] subtractives = new string[] { "I", "X", "C" };
            // Assume true, unless proven false
            bool charsValid = true;

            for (int i = 0; i < inputArray.Length; i++)
            {
                if (i + 1 < inputArray.Length)
                {
                    var currentValue = Numerals[CharToString(inputArray[i])];
                    var nextNextValue = Numerals[CharToString(inputArray[i + 1])];

                    if (currentValue < nextNextValue && !subtractives.Contains(CharToString(inputArray[i])))
                    {
                        charsValid = false;
                    }
                }
            }
            return charsValid;
        }
    }
}