using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public static class Extensions
    {
        public static bool SetEquals(this IEnumerable<Attribute> setA, IEnumerable<Attribute> setB)
        {
            var AnotB = setA.Except(setB);
            var BnotA = setB.Except(setA);

            return ((AnotB == null || !AnotB.Any()) && (BnotA == null || !BnotA.Any()));
        }

        public static bool SetEquals(this IEnumerable<Item> setA, IEnumerable<Item> setB)
        {
            var AnotB = setA.Except(setB);
            var BnotA = setB.Except(setA);

            return ((AnotB == null || !AnotB.Any()) && (BnotA == null || !BnotA.Any()));
        }
    }
}
