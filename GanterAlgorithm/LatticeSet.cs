using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public class LatticeSet : IComparable
    {
        public List<Attribute> AttributeSet { get; set; }
        public int Level { get; set; }
        public bool Processed { get; set; }
        public List<LatticeSet> Subsets { get; private set; }

        public LatticeSet()
        {
            Subsets = new List<LatticeSet>();
        }

        public static bool operator == (LatticeSet a, LatticeSet b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            if (a.Level == b.Level)
            {
                if (a.AttributeSet.Count == b.AttributeSet.Count)
                {
                    return a.LecticEquals(b) == 0;
                }
                else return false;
            }
            else return false;
        }

        public static bool operator != (LatticeSet a, LatticeSet b)
        {
            return !(a == b);
        }

        public void ProcessPossibleSubset(LatticeSet pivot)
        {
            if(this.AttributeSet.Contains(pivot.AttributeSet))
            {
                // transitive reduction
                Subsets.RemoveAll(s => pivot.Subsets.Contains(s));
                this.Subsets.Add(pivot);
                if(this.Level <= pivot.Level)
                    this.Level = pivot.Level + 1;
                Console.WriteLine("Level : {0} - {1} is a subset of {2}", Level, pivot, this);
            }
            else
            {

                Console.WriteLine("Level : {0} - {1} is not a subset of {2}", Level, pivot, this);
            }
        }

        public static bool operator < (LatticeSet a, LatticeSet b)
        {
            if (a == null || b == null) return false;
            if (a.Level == b.Level)
            {
                if (a.AttributeSet.Count == b.AttributeSet.Count)
                {
                    return a.LecticEquals(b) == -1;
                }
                else return a.AttributeSet.Count < b.AttributeSet.Count;
            }
            else return a.Level < b.Level;
        }

        public static bool operator > (LatticeSet a, LatticeSet b)
        {
            if (a == null || b == null) return false;
            if (a.Level == b.Level)
            {
                if (a.AttributeSet.Count == b.AttributeSet.Count)
                {
                    return a.LecticEquals(b) == 1;
                }
                else return a.AttributeSet.Count > b.AttributeSet.Count;
            }
            else return a.Level > b.Level;
        }

        public static bool operator <= (LatticeSet a, LatticeSet b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) <= 0;
            }
            else return a.Level <= b.Level;
        }

        public static bool operator >= (LatticeSet a, LatticeSet b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) >= 0;
            }
            else return a.Level >= b.Level;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(LatticeSet)) return false;
            return this == obj as LatticeSet;            
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private int LecticEquals(LatticeSet b)
        {
            Attribute smallestAdifference = this.AttributeSet.Except(b.AttributeSet).OrderBy(a => a.LecticPosition).FirstOrDefault();
            Attribute smallestBdifference = b.AttributeSet.Except(this.AttributeSet).OrderBy(a => a.LecticPosition).FirstOrDefault();

            if (smallestAdifference == null && smallestBdifference == null) return 0;
            else if (smallestAdifference == null) return 1;
            else if (smallestBdifference == null) return -1;
            // According to Ganter, if the smallest difference belongs to A, than A is greater than B.
            else if (smallestAdifference.LecticPosition > smallestBdifference.LecticPosition) return 1;
            else return -1;
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(LatticeSet)) throw new InvalidCastException();
            else
            {
                var latticeSet = obj as LatticeSet;
                if (this < latticeSet) return -1;
                else if (this > latticeSet) return 1;
                else return 0;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Level, "{" + string.Join(", ", AttributeSet.Select(l => l.Name)) + "}");
        }
    }
}
