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

        /// <summary>
        /// Gets the largest numeral that is smaller than or equal to the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string GetLargestFittingNumeral(int input)
        {
            var x = Numerals.Where(x => x.Value <= input).OrderByDescending(x => x.Value).First().Key;
            return x;
        }

        /// <summary>
        /// Gets the subtractive candidate (where a smaller value subtracts from the higher next value).
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string GetSubtractiveCandidate(int input)
        {
            string output = string.Empty;

            if (input >= 3 && input < Numerals.Values.Max())
            {
                var nextNumeralValue = GetSmallestNonFittingNumeralValue(input);
                var smallestNonFitting = Numerals.First(x => x.Value == nextNumeralValue).Key;
                var largestFitting = GetLargestFittingPowerOfTenNumeral(input);
                var previousNumeralValue = Numerals[largestFitting];
                var delta = nextNumeralValue - input;
                if (delta <= 0
                    || delta >= input
                    || delta > previousNumeralValue
                    )
                {
                    return output;
                }
                return largestFitting + smallestNonFitting;
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
                string temp;
                var sub = GetSubtractiveCandidate(working);
                int deduction;
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
                throw new System.InvalidOperationException("This number produces an invalid roman numeral sequence");
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
        /// Lists all the roman numerals.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> ListAll()
        {
            Dictionary<int, string> output = new Dictionary<int, string>();
            for (int i = 1; i < 4000; i++)
            {
                output[i] = IntToNumerals(i);
            }
            return output;
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
                throw new System.InvalidOperationException("Invalid roman numeral string");
            }

            // Add character items to a FIFO queue
            Queue<char> queue = new Queue<char>(input.ToCharArray());

            char workingChar;
            int workingValue = 0;
            int lastValue = 0;
            while (queue.TryDequeue(out workingChar))
            {
                var working = CharToString(workingChar);
                var lookedUp = Numerals[working];
                if (lastValue == 0 /* first run*/ || lastValue >= lookedUp /* additive */)
                {
                    lastValue = lookedUp;
                    workingValue += lookedUp;
                }
                else
                {
                    // Subtractive
                    lookedUp -= lastValue;
                    workingValue -= lastValue;
                    workingValue += lookedUp;
                }
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
        /// Contiguouses the character maximum check to ensure that a character does not exceed the max number of contiguous repetitions.
        /// </summary>
        /// <param name="haystack">The haystack.</param>
        /// <param name="input">The input.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        private bool ContiguousCharMaxCheck(string haystack, char input, int count)
        {
            bool output = true;
            int found = 0;
            int length = haystack.Length;

            for (int i = 0; i < length; i++)
            {
                if (haystack[i] == input)
                {
                    if (found + 1 <= count)
                    {
                        found++;
                    }
                    else
                    {
                        output = false;
                        break;
                    }
                }
                else
                {
                    found = 0;
                }
            }
            return output;
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

        private string GetLargestFittingPowerOfTenNumeral(int input)
        {
            // Limit search to I,X,C, which are powers of 10
            var x = Numerals.Where(x => (x.Key == "I" || x.Key == "X" || x.Key == "C") && x.Value <= input).OrderByDescending(x => x.Value).First().Key;
            return x;
        }

        /// <summary>
        /// Gets the smallest numeral value that is larger than or equal to the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private int GetSmallestNonFittingNumeralValue(int input)
        {
            var x = Numerals.Where(x => x.Value >= input).OrderBy(x => x.Value).First().Value;
            return x;
        }

        private bool IsValidInteger(int input)
        {
            return input > 0
                && input < 4000;
        }

        private bool IsValidNumeralOccurance(string input)
        {
            return
                // L,V,D cannot repeat:
                ContiguousCharMaxCheck(input, 'L', 1)
                && ContiguousCharMaxCheck(input, 'V', 1)
                && ContiguousCharMaxCheck(input, 'D', 1)

                // I,X,C can be repeated max 3 times
                && ContiguousCharMaxCheck(input, 'I', 3)
                && ContiguousCharMaxCheck(input, 'X', 3)
                && ContiguousCharMaxCheck(input, 'C', 3);
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