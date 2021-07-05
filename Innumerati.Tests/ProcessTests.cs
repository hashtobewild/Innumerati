using Innumerati.Processes.Implementations;
using NUnit.Framework;

namespace Innumerati.Tests
{
    public class ProcessTests
    {
        private RomanNumerals _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new RomanNumerals();
        }

        [Test]
        [TestCase(1, "I")]
        [TestCase(5, "V")]
        [TestCase(10, "X")]
        [TestCase(50, "L")]
        [TestCase(100, "C")]
        [TestCase(500, "D")]
        [TestCase(1000, "M")]
        public void TestBasicIntToNumeral_ReturnsExactMatchedNumerals(int input, string expected)
        {
            Assert.AreEqual(expected, _processor.IntToNumeral(input));
        }

        [Test]
        public void TestListBaseValues_DefaultFactory_ReturnsCountOfKnownNumerals()
        {
            Assert.AreEqual(7, _processor.Numerals.Count);
        }
    }
}