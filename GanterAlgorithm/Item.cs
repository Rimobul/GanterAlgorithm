using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public class Item
    {
        public string Name { get; set; }
        public int MatrixOrder { get; set; }

        public static bool operator <(Item a, Item b)
        {
            return a.MatrixOrder < b.MatrixOrder;
        }

        public static bool operator >(Item a, Item b)
        {
            return a.MatrixOrder > b.MatrixOrder;
        }

        public static bool operator <=(Item a, Item b)
        {
            return a.MatrixOrder <= b.MatrixOrder;
        }

        public static bool operator >=(Item a, Item b)
        {
            return a.MatrixOrder >= b.MatrixOrder;
        }

        public static bool operator ==(Item a, Item b)
        {
            return a.MatrixOrder == b.MatrixOrder;
        }

        public static bool operator !=(Item a, Item b)
        {
            return a.MatrixOrder != b.MatrixOrder;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Item))
            {
                return this.MatrixOrder == (obj as Item).MatrixOrder;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return MatrixOrder;
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}", Name, MatrixOrder);
        }
    }
}
