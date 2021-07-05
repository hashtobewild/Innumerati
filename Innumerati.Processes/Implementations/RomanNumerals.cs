using Innumerati.Processes.Interfaces;
using System.Collections.Specialized;

namespace Innumerati.Processes.Implementations
{
    public class RomanNumerals : IRomanNumerals
    {
        private OrderedDictionary _numerals;

        public RomanNumerals()
        {
            DefaultFactory();
        }

        /// <summary>
        /// Gets or sets the numerals that are known to the application.
        /// The numerals are stored in an ordered dictionary, as it can be accessed by key, value and enumerator
        /// </summary>
        /// <value>
        /// The numerals.
        /// </value>
        public OrderedDictionary Numerals { get => _numerals; set => _numerals = value; }

        /// <summary>
        /// Initializes the default values
        /// </summary>
        private void DefaultFactory()
        {
            Numerals = new OrderedDictionary()
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000},
            };
        }
    }
}