using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public class Attribute
    {
        public string Name { get; set; }
        public int LecticPosition { get; set; }

        public static bool operator < (Attribute a, Attribute b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            else return a.LecticPosition < b.LecticPosition;
        }

        public static bool operator > (Attribute a, Attribute b)
        {
            if (a == null || b == null) throw new ArgumentNullException();
            else return a.LecticPosition > b.LecticPosition;
        }

        public static bool operator <= (Attribute a, Attribute b)
        {
            if (a == null && b == null) return true;
            else if (a == null || b == null) return false;
            else return a.LecticPosition <= b.LecticPosition;
        }

        public static bool operator >= (Attribute a, Attribute b)
        {
            if (a == null && b == null) return true;
            else if (a == null || b == null) return false;
            else return a.LecticPosition >= b.LecticPosition;
        }

        public static bool operator == (Attribute a, Attribute b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.LecticPosition == b.LecticPosition;
        }

        public static bool operator != (Attribute a, Attribute b)
        {
            return !(a == b);
        }

        public List<Attribute> Closure(List<Attribute> setA, FormalContext formalContext)
        {
            List<Attribute> lecticSet = FormLecticSet(setA, formalContext.Attributes);
            return formalContext.Extent(formalContext.Intent(lecticSet)).ToList();
        }

        private List<Attribute> FormLecticSet(List<Attribute> setA, List<Attribute> attributes)
        {
            List<Attribute> result = new List<Attribute>();
            result = setA.Where(a => a.LecticPosition < this.LecticPosition).ToList();
            result.Add(this);
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Attribute))
            {
                return this.LecticPosition == (obj as Attribute).LecticPosition;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return this.LecticPosition;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Name, LecticPosition);
        }
    }
}
