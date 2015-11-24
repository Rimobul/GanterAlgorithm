using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Algorithm
{
    /// <summary>
    /// CLass representing an item of a formal context.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The position of the item in the formal context matrix. If none is provided, the formal context should assign a default one.
        /// </summary>
        public int MatrixOrder { get; set; }

        /// <summary>
        /// Compares two formal items based on their matrix order.
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>True, if the first item is smaller than the second. Otherwise returns false.</returns>
        public static bool operator <(Item a, Item b)
        {
            return a.MatrixOrder < b.MatrixOrder;
        }

        /// <summary>
        /// Compares two formal items based on their matrix order.
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>True, if the first item is larger than the second. Otherwise returns false.</returns>
        public static bool operator >(Item a, Item b)
        {
            return a.MatrixOrder > b.MatrixOrder;
        }

        /// <summary>
        /// Compares two formal items based on their matrix order.
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>True, if the first item is smaller or equal than the second. Otherwise returns false.</returns>
        public static bool operator <=(Item a, Item b)
        {
            return a.MatrixOrder <= b.MatrixOrder;
        }

        /// <summary>
        /// Compares two formal items based on their matrix order.
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>True, if the first item is larger or equal than the second. Otherwise returns false.</returns>
        public static bool operator >=(Item a, Item b)
        {
            return a.MatrixOrder >= b.MatrixOrder;
        }

        /// <summary>
        /// Compares two formal items based on their matrix order.
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>True, if both items are null or both have the same matrix order. Otherwise returns false.</returns>
        public static bool operator ==(Item a, Item b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.MatrixOrder == b.MatrixOrder;
        }

        /// <summary>
        /// Compares two formal items based on their matrix order.
        /// </summary>
        /// <param name="a">The first item to compare.</param>
        /// <param name="b">The second item to compare.</param>
        /// <returns>True, if one of the items is null or they have different matrix order. Otherwise returns false.</returns>
        public static bool operator !=(Item a, Item b)
        {
            return a.MatrixOrder != b.MatrixOrder;
        }

        /// <summary>
        /// Compares this item to another object.
        /// </summary>
        /// <param name="obj">An object, that should be compared to this item.</param>
        /// <returns>True, if the object is an item and has the same matrix order, as this item.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Item))
            {
                return this.MatrixOrder == (obj as Item).MatrixOrder;
            }
            else return false;
        }

        /// <summary>
        /// Gets the hash code of the object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return MatrixOrder;
        }

        /// <summary>
        /// Converts the object to its string representation.
        /// </summary>
        /// <returns>Returns the name of the item and its matrix order.</returns>
        public override string ToString()
        {
            return String.Format("{0} : {1}", Name, MatrixOrder);
        }
    }
}
