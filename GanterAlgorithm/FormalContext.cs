using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public class FormalContext
    {
        public List<Attribute> Attributes { get; private set; }
        public List<Item> Items { get; private set; }
        /// <summary>
        /// Matrix Attributes x Items
        /// </summary>
        public bool[,] Matrix { get; private set; }
        
        public bool? Value(Attribute attribute, Item item)
        {
            return Matrix[attribute.LecticPosition, item.MatrixOrder];
        } 

        public IEnumerable<Item> Intent(IEnumerable<Attribute> attributeSet)
        {
            if(attributeSet == null || !attributeSet.Any())
            {
                foreach (var item in Items)
                    yield return item;
                yield break;
            }

            foreach(int order in Items.Select(i => i.MatrixOrder))
            {
                bool containsAllAttributes = true;
                foreach(int position in attributeSet.Select(a => a.LecticPosition))
                {
                    if(!Matrix[position, order])
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
            if(items == null || !items.Any())
            {
                foreach (var attribute in Attributes)
                    yield return attribute;
                yield break; 
            }

            foreach(int position in Attributes.Select(a => a.LecticPosition))
            {
                bool containsAllItems = true;
                foreach(int order in items.Select(i => i.MatrixOrder))
                {
                    if(!Matrix[position, order])
                    {
                        containsAllItems = false;
                        break;
                    }
                }

                if (containsAllItems)
                    yield return Attributes.FirstOrDefault(a => a.LecticPosition == position);
            }
        }
    }
}
