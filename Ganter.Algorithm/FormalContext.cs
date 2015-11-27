using System;
using System.Collections.Generic;
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
        /// Creates the intent for a particular set of attributes.
        /// </summary>
        /// <param name="attributeSet">The attribute set, for which the intent should be created.</param>
        /// <returns>A set of items, which possess all desired attributes.</returns>
        public IEnumerable<Item> Intent(IEnumerable<Attribute> attributeSet)
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
        /// Creates the extent for a particular set of items.
        /// </summary>
        /// <param name="items">The item set, for which the extent should be created.</param>
        /// <returns>A set of attributes, which are all possessed by all the provided items.</returns>
        public IEnumerable<Attribute> Extent(IEnumerable<Item> items)
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
        /// <returns>A set of extents.</returns>
        public List<List<Attribute>> PerformAlgorithm()
        {
            List<Attribute> setA = Extent(Items).ToList();
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

        public bool[,] FormOutput(List<List<Attribute>> extents, List<List<Item>> intents, bool transitiveReduction)
        {
            bool[,] result = new bool[extents.Count, extents.Count];

            Parallel.For(0, extents.Count, i => 
                {
                    for (int j = 0; j < extents.Count; j++)
                    {
                        if (i == j)
                        {
                            result[i, j] = true;
                        }
                        else if (extents[i].Contains(extents[j]))
                        {
                            result[i, j] = true;
                        }
                    }

                    if(intents != null)
                        intents.Add(Intent(extents[i]).ToList());
                });

            return result;
        }

        // TODO prerobit na zapis do streamu
        public string FormOutputString(List<List<Attribute>> extents, bool transitiveReduction, bool attributes, bool items, string csvSeparator)
        {
            List<List<Item>> intents = null;

            if (items)
                intents = new List<List<Item>>();

            bool[,] matrix = FormOutput(extents, intents, transitiveReduction);
            StringBuilder sb = new StringBuilder();

            if (attributes)
            {
                sb.AppendLine("Extents:");
                foreach (var extent in extents)
                {
                    sb.AppendLine("{" + string.Join(", ", extent.Select(a => a.Name)) + "}");
                }
            }

            if (items)
            {
                sb.AppendLine("\r\nIntents:");
                foreach (var intent in intents)
                {
                    sb.AppendLine("{" + string.Join(", ", intent.Select(i => i.Name)) + "}");
                }
            }

            sb.AppendLine("\r\nMatrix:");
            for (int i = 0; i < extents.Count; i++)
            {
                for (int j = 0; j < extents.Count; j++)
                {
                    sb.Append(matrix[i, j].ToString().ToUpper() + csvSeparator + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
