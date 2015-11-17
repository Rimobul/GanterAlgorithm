using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Algorithm
{
    public class FormalContext
    {
        public List<Attribute> Attributes { get; private set; }
        public List<Item> Items { get; private set; }
        /// <summary>
        /// Matrix Items x Attributes
        /// </summary>
        public bool[,] Matrix { get; private set; }

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

        public bool? Value(Attribute attribute, Item item)
        {
            return Matrix[item.MatrixOrder, attribute.LecticPosition];
        }

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

        public List<List<Attribute>> PerformAlgorithm()
        {
            List<Attribute> setA = Extent(Items).ToList();
            List<List<Attribute>> resultSets = new List<List<Attribute>>();
            bool wasFound = true;

            // adding first set
            resultSets.Add(setA);

            while (wasFound)
            {
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
    }
}
