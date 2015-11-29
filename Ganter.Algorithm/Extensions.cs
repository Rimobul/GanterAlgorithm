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

        /// <summary>
        /// Converts a set of attributes into its proper string representation
        /// </summary>
        /// <param name="set">The set of attributes.</param>
        /// <returns>A string in the form {attribute1, attribute2, ... attributeN}</returns>
        public static string AsString(this IEnumerable<Attribute> set)
        {
            return "{" + string.Join(", ", set.Select(a => a.Name)) + "}";
        }

        /// <summary>
        /// Converts a set of items into its proper string representation
        /// </summary>
        /// <param name="set">The set of items.</param>
        /// <returns>A string in the form {item1, item2, ... itemN}</returns>
        public static string AsString(this IEnumerable<Item> set)
        {
            return "{" + string.Join(", ", set.Select(i => i.Name)) + "}";
        }

        /// <summary>
        /// Creates a string representation of the single relation.
        /// </summary>
        /// <param name="relation">Related vertex.</param>
        /// <param name="currentIndex">Current vertex.</param>
        /// <returns>A string in the form (vertex0, vertex1).</returns>
        public static string RelationString(this int relation, int currentIndex)
        {
            return string.Format("({0},{1})", currentIndex, relation);
        }

        /// <summary>
        /// Creates a string representing the current vertex and all vertices that are related to it.
        /// </summary>
        /// <param name="relations">Related vertices.</param>
        /// <param name="currentIndex">Current vertex.</param>
        /// <returns>A string in the form (vertex0, vertex1), (vertex0, vertex2), ...(vertex0, vertexN)</returns>
        public static string RelationsString(this IEnumerable<int> relations, int currentIndex)
        {
            return string.Join(", ", relations.Select(r => r.RelationString(currentIndex)));
        }
    }
}