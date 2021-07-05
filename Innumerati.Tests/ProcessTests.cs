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
        [TestCase(2, "II")]
        [TestCase(4, "IIII")]
        [TestCase(5, "V")]
        [TestCase(6, "VI")]
        [TestCase(10, "X")]
        [TestCase(11, "XI")]
        [TestCase(15, "XV")]
        [TestCase(40, "XXXX")]
        [TestCase(50, "L")]
        [TestCase(55, "LV")]
        [TestCase(90, "LXXXX")]
        [TestCase(100, "C")]
        [TestCase(200, "CC")]
        [TestCase(500, "D")]
        [TestCase(550, "DL")]
        [TestCase(650, "DCL")]
        [TestCase(1000, "M")]
        [TestCase(2000, "MM")]
        public void TestBasicIntToNumeral_ReturnsExactMatchedNumerals(int input, string expected)
        {
            Assert.AreEqual(expected, _processor.IntToNumerals(input));
        }

        [Test]
        [TestCase("I", 1)]
        [TestCase("II", 2)]
        [TestCase("IV", 4)]
        [TestCase("V", 5)]
        [TestCase("VI", 6)]
        [TestCase("X", 10)]
        [TestCase("XI", 11)]
        [TestCase("L", 50)]
        [TestCase("LX", 60)]
        [TestCase("XC", 90)]
        [TestCase("C", 100)]
        [TestCase("CC", 200)]
        [TestCase("D", 500)]
        [TestCase("M", 1000)]
        [TestCase("MM", 2000)]
        public void TestBasicNumeralToInt_ReturnsExactMatchedIntegrals(string input, int expected)
        {
            Assert.AreEqual(expected, _processor.NumeralsToInt(input));
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

        [Test]
        [TestCase("", false)]
        [TestCase("XA", false)]
        [TestCase("A", false)]
        [TestCase("I", true)]
        [TestCase("II", true)]
        [TestCase("III", true)]
        [TestCase("IV", true)]
        [TestCase("IIII", false)]
        [TestCase("V", true)]
        [TestCase("VV", false)]
        [TestCase("X", true)]
        [TestCase("L", true)]
        [TestCase("LL", false)]
        [TestCase("C", true)]
        [TestCase("D", true)]
        [TestCase("DD", false)]
        [TestCase("M", true)]
        public void TestValidRomanNumeralString_ReturnsWhetherARomanNumeralStringIsValid(string input, bool expected)
        {
            Assert.AreEqual(expected, _processor.IsValidRomanNumeral(input));
        }
    }
}