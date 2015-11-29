using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Algorithm
{
    /// <summary>
    /// Represents a formal context. The context contains a set of attributes, items and a matrix (item x attributes).
    /// </summary>
    public class FormalContext
    {
        /// <summary>
        /// The set of attributes of the formal context.
        /// </summary>
        public List<Attribute> Attributes { get; private set; }
        /// <summary>
        /// The set of items of the formal context.
        /// </summary>
        public List<Item> Items { get; private set; }
        /// <summary>
        /// A boolean matrix of the size Items.Count x Attributes.Count. The matrix determines which items posses certain attributes.
        /// </summary>
        public bool[,] Matrix { get; private set; }

        /// <summary>
        /// Creates a new instance of formal context.
        /// </summary>
        /// <param name="attributes">The set of attributes of the formal context.</param>
        /// <param name="items">The set of item of the formal context.</param>
        /// <param name="matrix">The boolean matrix representing the item-attribute relationships. The dimensions of the matrix should be Items.Count x Attributes.Count.</param>
        /// <param name="assignDefaultPositions">Determines, whether default lectic positions should be assigned to the attributes and items.</param>
        public FormalContext(List<Attribute> attributes, List<Item> items, bool[,] matrix, bool assignDefaultPositions)
        {
            if (attributes == null
                || !attributes.Any()
                || items == null
                || !items.Any()
                || matrix == null)
                throw new ArgumentNullException("Invalid arguments");

            if (matrix.Length != attributes.Count * items.Count) throw new IndexOutOfRangeException("Wrong matrix");

            Attributes = attributes;
            Items = items;
            Matrix = matrix;

            if (assignDefaultPositions)
            {
                for (int i = 0; i < Attributes.Count; i++)
                {
                    Attributes[i].LecticPosition = i;
                }

                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].MatrixOrder = i;
                }
            }
        }

        /// <summary>
        /// Creates the extent for a particular set of attributes.
        /// </summary>
        /// <param name="attributeSet">The attribute set, for which the extent should be created.</param>
        /// <returns>A set of items, which possess all desired attributes.</returns>
        public IEnumerable<Item> Extent(IEnumerable<Attribute> attributeSet)
        {
            if (attributeSet == null || !attributeSet.Any())
            {
                foreach (var item in Items)
                    yield return item;
                yield break;
            }

            foreach (int order in Items.Select(i => i.MatrixOrder))
            {
                bool containsAllAttributes = true;
                foreach (int position in attributeSet.Select(a => a.LecticPosition))
                {
                    if (!Matrix[order, position])
                    {
                        containsAllAttributes = false;
                        break;
                    }
                }

                if (containsAllAttributes)
                    yield return Items.FirstOrDefault(i => i.MatrixOrder == order);
            }
        }

        /// <summary>
        /// Creates the intent for a particular set of items.
        /// </summary>
        /// <param name="items">The item set, for which the intent should be created.</param>
        /// <returns>A set of attributes, which are all possessed by all the provided items.</returns>
        public IEnumerable<Attribute> Intent(IEnumerable<Item> items)
        {
            if (items == null || !items.Any())
            {
                foreach (var attribute in Attributes)
                    yield return attribute;
                yield break;
            }

            foreach (int position in Attributes.Select(a => a.LecticPosition))
            {
                bool containsAllItems = true;
                foreach (int order in items.Select(i => i.MatrixOrder))
                {
                    if (!Matrix[order, position])
                    {
                        containsAllItems = false;
                        break;
                    }
                }

                if (containsAllItems)
                    yield return Attributes.FirstOrDefault(a => a.LecticPosition == position);
            }
        }

        /// <summary>
        /// Performs the Ganter algorithm.
        /// </summary>
        /// <returns>A set of intents.</returns>
        public List<List<Attribute>> PerformAlgorithm()
        {
            List<Attribute> setA = Intent(Items).ToList();
            List<List<Attribute>> resultSets = new List<List<Attribute>>();
            bool wasFound = true;

            // adding first set
            resultSets.Add(setA);

            while (wasFound)
            {
                // this is the only set containing all attributes
                if (!Attributes.Any(a => !setA.Contains(a)))
                    return resultSets;

                foreach (var mi in Attributes.Where(a => !setA.Contains(a)).OrderByDescending(a => a.LecticPosition))
                {
                    List<Attribute> closure = mi.Closure(setA, this);
                    Attribute newMinimal = closure.Where(c => !setA.Contains(c)).OrderBy(c => c.LecticPosition).FirstOrDefault();

                    if (newMinimal == mi)
                    {
                        setA = closure;
                        resultSets.Add(closure);
                        wasFound = true;

                        if (setA.SetEquals(Attributes))
                        {
                            return resultSets;
                        }

                        break;
                    }
                    else wasFound = false;
                }

                if (!wasFound)
                    throw new Exception("Not found.");
            }

            return null;
        }

        /// <summary>
        /// Creates the output, that serves as a base for Hasse diagram representation.
        /// </summary>
        /// <param name="intents">Set of intents, that were produced as a result of the Ganter algorithm.</param>
        /// <param name="extents">An empty set, into which sets of extents will be inserted. If extents should not be formed, pass NULL.</param>
        /// <param name="transitiveReduction">Determines, whether the result should pass through transitive reduction. Transitive reduction makes the resulting 
        /// file smaller, but takes more time.</param>
        /// <returns>A dictionary representing the sparse matrix of relations among the intents.</returns>
        public Dictionary<int, HashSet<int>> FormOutput(List<List<Attribute>> intents, List<List<Item>> extents, bool transitiveReduction)
        {
            Dictionary<int, HashSet<int>> result = new Dictionary<int, HashSet<int>>();

            for(int i = 0; i < intents.Count; i++)
            {
                result.Add(i, new HashSet<int>());

                if (extents != null)
                    extents.Add(Extent(intents[i]).ToList());
            }

            Parallel.For(0, intents.Count, i =>
            {
                for (int j = 0; j <= i; j++)
                {
                    if (i == j && !transitiveReduction)
                    {
                        result[i].Add(j);
                    }
                    else if (i != j && intents[i].Contains(intents[j]))
                    {
                        result[i].Add(j);
                    }
                }                
            });

            if(transitiveReduction)
            {
                for(int i = 1; i < result.Count; i++)
                {
                    for(int j = 0; j < i; j++)
                    {
                        if (result[i].Contains(j))
                            result[i].RemoveWhere(r => result[j].Contains(r));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Writes the result suitable for Hasse diagram creation into provided stream.
        /// </summary>
        /// <param name="writer">The stream writer, into which the result should be written.</param>
        /// <param name="intents">The result of Ganter algorithm.</param>
        /// <param name="transitiveReduction">Determines, whether the result should pass through transitive reduction. Transitive reduction makes the resulting 
        /// file smaller, but takes more time.</param>
        /// <param name="attributes">Determines, whether the set of attributes (intents) should be written at the beginning of the resulting document.</param>
        /// <param name="items">Determines, whether the set of item (extents) should be written at the beginning of the resulting document.</param>
        /// <param name="csvSeparator">The separator, that will be used to separate individual sets.</param>
        public void WriteOutput(StreamWriter writer, List<List<Attribute>> intents, bool transitiveReduction, bool attributes, bool items, string csvSeparator)
        {
            List<List<Item>> extents = null;

            if (items)
                extents = new List<List<Item>>();

            var dictionary = FormOutput(intents, extents, transitiveReduction);

            if (attributes)
            {
                writer.WriteLine("Intents:");
                foreach (var intent in intents)
                {
                    writer.WriteLine("{" + string.Join(", ", intent.Select(a => a.Name)) + "}");
                }
            }

            if (items)
            {
                writer.WriteLine("\r\nExtents:");
                foreach (var extent in extents)
                {
                    writer.WriteLine("{" + string.Join(", ", extent.Select(i => i.Name)) + "}");
                }
            }

            writer.WriteLine("\r\nAlias(Intent): List of relations");
            for (int i = 0; i < dictionary.Count; i++)
            {
                writer.WriteLine(string.Format("{0} ({1}) : \r\n\t{2}", i, intents[i].AsString(), dictionary[i].RelationsString(i)));
            }
        }
    }
}
