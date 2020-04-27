using WCA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Domain.Extensions
{
    public static class FractionExtensions
    {
        public static Fraction Sum(this IEnumerable<Fraction> source)
        {
            var sourceList = source.ToList();

            if (sourceList.Count > 0)
            {
                return source.Aggregate((x, y) => x + y);
            }

            return new Fraction(0);
        }

        public static Fraction Sum<T>(this IEnumerable<T> source, Func<T, Fraction> selector)
        {
            var sourceList = source.Select(selector).ToList();

            if (sourceList.Count > 0)
            {
                return sourceList.Aggregate((x, y) => x + y);
            }

            return new Fraction(0);
        }
    }
}