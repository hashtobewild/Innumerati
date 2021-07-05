using Innumerati.Processes.Implementations;
using NUnit.Framework;

namespace Innumerati.Tests
{
    public class Tests
    {
        private RomanNumerals _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new RomanNumerals();
        }

        [Test]
        public void TestListBaseValues_ReturnsCountOfKnownNumerals()
        {
            Assert.AreEqual(7, _processor.Numerals.Count);
        }
    }
}