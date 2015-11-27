using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Algorithm
{
    /// <summary>
    /// A static class defining extension methods for the Ganter algorithm objects.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Compares two sets of formal attributes.
        /// </summary>
        /// <param name="setA">The first set to compare.</param>
        /// <param name="setB">The second set to compare.s</param>
        /// <returns>True, if both sets contain the same elements. Otherwise return false.</returns>
        public static bool SetEquals(this IEnumerable<Attribute> setA, IEnumerable<Attribute> setB)
        {
            var AnotB = setA.Except(setB);
            var BnotA = setB.Except(setA);

            return ((AnotB == null || !AnotB.Any()) && (BnotA == null || !BnotA.Any()));
        }

        /// <summary>
        /// Compares two sets of formal items.
        /// </summary>
        /// <param name="setA">The first set to compare.</param>
        /// <param name="setB">The second set to compare.s</param>
        /// <returns>True, if both sets contain the same elements. Otherwise return false.</returns>
        public static bool SetEquals(this IEnumerable<Item> setA, IEnumerable<Item> setB)
        {
            var AnotB = setA.Except(setB);
            var BnotA = setB.Except(setA);

            return ((AnotB == null || !AnotB.Any()) && (BnotA == null || !BnotA.Any()));
        }

        /// <summary>
        /// Decides, whether the first set contains (is a superset of) the second set.
        /// </summary>
        /// <param name="setA">The tested superset.</param>
        /// <param name="setB">The tested subset.</param>
        /// <returns>True, if setA is a superset of setB. Otherwise returns false.</returns>
        public static bool Contains(this IEnumerable<Attribute> setA, IEnumerable<Attribute> setB)
        {
            return !setB.Except(setA).Any();
        }
    }
}