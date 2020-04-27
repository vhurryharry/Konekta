using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WCA.Domain.Extensions;
using WCA.Domain.Models;
using Xunit;

namespace WCA.UnitTests.Models
{
    public class FractionTests
    {
        [Fact]
        public void FractionSumExtensionOneValue()
        {
            var fractions = new List<Fraction>();
            fractions.Add(new Fraction(1, 2));
            Fraction result = fractions.Sum(f => f);
            Assert.Equal(new Fraction(1, 2), result);
        }

        [Theory]
        [InlineData(1, 2, 1, 4, 0.75)]
        public void FractionSumExtension(int num1, int den1, int num2, int den2, double expectedResult)
        {
            var fractions = new List<Fraction>();
            fractions.Add(new Fraction(num1, den1));
            fractions.Add(new Fraction(num2, den2));
            Fraction result = fractions.Sum(f => f);
            Assert.Equal(expectedResult, (double)result);
        }

        [Theory]
        [InlineData(4, 5, 0.8)]
        public void CastFractionToDouble(int num, int den, double expected)
        {
            var result = new Fraction(num, den);
            Assert.Equal(expected, (double)result);
        }

        [Theory]
        [InlineData(4, 5, 0.8)]
        public void CastFractionToDecimal(int num, int den, decimal expected)
        {
            var result = new Fraction(num, den);
            Assert.Equal(expected, (decimal)result);
        }

        [Theory]
        [InlineData(4, 5, 0.8)]
        public void CastFractionToFloat(int num, int den, float expected)
        {
            var result = new Fraction(num, den);
            Assert.Equal(expected, (float)result);
        }

        [Theory]
        [InlineData("4/5", 4, 5)]
        public void ParseFractionFromString(string parseMe, int expectedNum, int expectedDen)
        {
            var fraction = new Fraction(parseMe);
            Assert.Equal(expectedNum, fraction.Numerator);
            Assert.Equal(expectedDen, fraction.Denominator);
        }

        [Theory]
        [InlineData("4/5/a")]
        [InlineData("4 / 5 /")]
        [InlineData("4 / 5 / 1")]
        public void ParseFractionFromInvalidString(string parseMe)
        {
            Assert.Throws<FormatException>(() =>
            {
                var fraction = new Fraction(parseMe);
            });
        }

        [Fact]
        public void SuppliedFractionTests()
        {
            Fraction frac = new Fraction(); // we'll get NaN
            Assert.Equal(Fraction.NaN, frac);
            Assert.Equal(NumberFormatInfo.CurrentInfo.NaNSymbol, frac.ToString());

            frac = new Fraction(1, 5);       // we'll get 1/5
            Assert.Equal("1/5", frac.ToString());

            frac = new Fraction(25);        // we'll get 25
            Assert.Equal("25", frac.ToString());

            frac = new Fraction(0.0);       // we'll get 0
            Assert.Equal("0", frac.ToString());

            frac = new Fraction(0.25);      // we'll get 1/4
            Assert.Equal("1/4", frac.ToString());

            frac = new Fraction(9.25);      // we'll get 37/4
            Assert.Equal("37/4", frac.ToString());

            frac = new Fraction(long.MaxValue, 1);
            string compareTo = string.Format(CultureInfo.InvariantCulture, "{0}", long.MaxValue);
            Assert.Equal(compareTo, frac.ToString());

            frac = new Fraction(1, long.MaxValue);
            compareTo = string.Format(CultureInfo.InvariantCulture, "1/{0}", long.MaxValue);
            Assert.Equal(compareTo, frac.ToString());

            frac = new Fraction(long.MaxValue, long.MaxValue);
            Assert.Equal("1", frac.ToString());

            // the plus-one issue is because of twos-complement representing one more negtive value than positive
            frac = new Fraction(long.MinValue + 1, 1);
            compareTo = string.Format(CultureInfo.InvariantCulture, "{0}", long.MinValue + 1);
            Assert.Equal(compareTo, frac.ToString());

            frac = new Fraction(1, long.MinValue + 1);
            compareTo = string.Format(CultureInfo.InvariantCulture, "-1/{0}", Math.Abs(long.MinValue + 1));
            Assert.Equal(compareTo, frac.ToString());

            frac = new Fraction(long.MinValue + 1, long.MinValue + 1);
            Assert.Equal("1", frac.ToString());

            frac = new Fraction(long.MaxValue, long.MinValue + 1);
            Assert.Equal("-1", frac.ToString());

            frac = new Fraction(long.MinValue + 1, long.MaxValue);
            Assert.Equal("-1", frac.ToString());

            frac = new Fraction(0.025);     // we'll get 1/40
            Assert.Equal("1/40", frac.ToString());
            frac = new Fraction(1 / 2.0);   // we'll get 1/2
            Assert.Equal("1/2", frac.ToString());
            frac = new Fraction(1 / 3.0);   // we'll get 1/3
            Assert.Equal("1/3", frac.ToString());
            frac = new Fraction(1 / 4.0);   // we'll get 1/4
            Assert.Equal("1/4", frac.ToString());
            frac = new Fraction(1 / 5.0);   // we'll get 1/5
            Assert.Equal("1/5", frac.ToString());
            frac = new Fraction(1 / 6.0);   // we'll get 1/6
            Assert.Equal("1/6", frac.ToString());
            frac = new Fraction(1 / 7.0);   // we'll get 1/7
            Assert.Equal("1/7", frac.ToString());
            frac = new Fraction(1 / 8.0);   // we'll get 1/8
            Assert.Equal("1/8", frac.ToString());
            frac = new Fraction(1 / 9.0);   // we'll get 1/9
            Assert.Equal("1/9", frac.ToString());
            frac = new Fraction(1 / 10.0);   // we'll get 1/10
            Assert.Equal("1/10", frac.ToString());
            frac = new Fraction(1 / 49.0);   // we'll get 1/49
            Assert.Equal("1/49", frac.ToString());

            frac = new Fraction(6);
            Assert.Equal("6", frac.ToString());

            Fraction divisor = new Fraction(4);
            Assert.Equal("4", divisor.ToString());

            frac %= divisor;
            Assert.Equal("2", frac.ToString());

            frac = new Fraction(9, 4);
            Assert.Equal("9/4", frac.ToString());

            divisor = new Fraction(2);
            Assert.Equal("2", divisor.ToString());

            frac %= divisor;
            Assert.Equal("1/4", frac.ToString());

            frac = new Fraction(5, 12);
            Assert.Equal("5/12", frac.ToString());

            divisor = new Fraction(1, 4);
            Assert.Equal("1/4", divisor.ToString());

            frac %= divisor;
            Assert.Equal("1/6", frac.ToString());

            frac = new Fraction(1.0);     // we'll get 1
            Assert.Equal("1", frac.ToString());

            frac = new Fraction(2.0);     // we'll get 2
            Assert.Equal("2", frac.ToString());

            frac = new Fraction(-2.0);    // we'll get -2
            Assert.Equal("-2", frac.ToString());

            frac = new Fraction(-1.0);    // we'll get -1
            Assert.Equal("-1", frac.ToString());

            frac = new Fraction(0.5);       // we'll get 1/2
            Assert.Equal("1/2", frac.ToString());

            frac = new Fraction(1.5);     // we'll get 3/2
            Assert.Equal("3/2", frac.ToString());

            for (int numerator = -100; numerator < 100; numerator++)
            {
                for (int denominator = -100; denominator < 100; denominator++)
                {
                    Fraction frac1 = new Fraction(numerator, denominator);

                    double dbl = (double)numerator / (double)denominator;
                    Fraction frac2 = new Fraction(dbl);

                    Assert.Equal(frac1, frac2);
                }
            }

            frac = new Fraction("6.25");    // we'll get 25/4
            Assert.Equal("25/4", frac.ToString());

            frac = 0;
            Assert.Equal("0", frac.ToString());

            frac = 1;
            Assert.Equal("1", frac.ToString());

            frac /= new Fraction(0);
            Assert.Equal(Fraction.PositiveInfinity, frac);
            Assert.Equal(NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol, frac.ToString());

            frac = -1;
            Assert.Equal("-1", frac.ToString());

            frac /= new Fraction(0);
            Assert.Equal(Fraction.NegativeInfinity, frac);
            Assert.Equal(NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol, frac.ToString());

            frac = new Fraction("1/2"); // initialize a fraction with 1/2
            Assert.Equal("1/2", frac.ToString());

            Assert.Equal("3", (frac + 2.5).ToString());

            frac = "1/2";           // implicit cast from string to
            Assert.Equal("1/2", frac.ToString());

            frac = "22.5";         // implicit cast from string to fraction
            Assert.Equal("45/2", frac.ToString());

            frac = 10.25;         // implicit cast from double to fraction
            Assert.Equal("41/4", frac.ToString());

            frac = 15;             // implicit cast from integer/long to fraction
            Assert.Equal("15", frac.ToString());

            frac = 0.5;                 // initialize frac=1/2
            Assert.Equal("1/2", frac.ToString());

            Assert.Equal("1/4", (frac - 0.25).ToString());

            Assert.Equal("3/4", (frac + "1/4").ToString());

            Assert.False(frac.Equals(0.5));

            frac += 0.5;
            Assert.Equal("1", frac.ToString());

            Assert.False(frac.Equals(1));

            frac = double.NaN;
            Assert.Equal(NumberFormatInfo.CurrentInfo.NaNSymbol, frac.ToString());

            frac = double.PositiveInfinity;
            Assert.Equal(NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol, frac.ToString());

            frac = double.NegativeInfinity;
            Assert.Equal(NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol, frac.ToString());

            frac = "33";
            frac += "1/3";
            Assert.Equal(new Fraction(100, 3), frac);

            frac *= 3;
            Assert.Equal(100M, (decimal)frac);
        }
    }
}