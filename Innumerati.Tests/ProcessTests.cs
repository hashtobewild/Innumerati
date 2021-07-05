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
        [TestCase("I", 1)]
        [TestCase("V", 5)]
        [TestCase("X", 10)]
        [TestCase("L", 50)]
        [TestCase("C", 100)]
        [TestCase("D", 500)]
        [TestCase("M", 1000)]
        public void TestBasicNumeralToInt_ReturnsExactMatchedIntegrals(string input, int expected)
        {
            Assert.AreEqual(expected, _processor.NumeralToInt(input));
        }

        [TestCase(1, "I")]
        [TestCase(2, "I")]
        [TestCase(5, "V")]
        [TestCase(6, "V")]
        [TestCase(10, "X")]
        [TestCase(20, "X")]
        [TestCase(30, "X")]
        [TestCase(50, "L")]
        [TestCase(60, "L")]
        [TestCase(100, "C")]
        [TestCase(300, "C")]
        [TestCase(500, "D")]
        [TestCase(800, "D")]
        [TestCase(1000, "M")]
        [TestCase(2000, "M")]
        public void TestGetLargestFittingNumeralFromInt_ReturnsTheLargestNumeralSmallerThanOrEqualToInt(int input, string expected)
        {
            Assert.AreEqual(expected, _processor.GetLargestFittingNumeral(input));
        }

        [Test]
        public void TestListBaseValues_DefaultFactory_ReturnsCountOfKnownNumerals()
        {
            Assert.AreEqual(7, _processor.Numerals.Count);
        }
    }
}