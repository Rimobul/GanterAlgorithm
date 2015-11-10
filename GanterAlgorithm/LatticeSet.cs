using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public class LatticeSet
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
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) == 0;
            }
            else return false;
        }

        public static bool operator != (LatticeSet a, LatticeSet b)
        {
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) != 0;
            }
            else return true;
        }

        public void ProcessPossibleSubset(LatticeSet pivot)
        {
            if(this.AttributeSet.Contains(pivot.AttributeSet))
            {
                // transitive reduction
                Subsets.RemoveAll(s => pivot.Subsets.Contains(s));
                this.Subsets.Add(pivot);
            }
        }

        public static bool operator < (LatticeSet a, LatticeSet b)
        {
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) == -1;
            }
            else return a.Level < b.Level;
        }

        public static bool operator > (LatticeSet a, LatticeSet b)
        {
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) == 1;
            }
            else return a.Level > b.Level;
        }

        public static bool operator <= (LatticeSet a, LatticeSet b)
        {
            if (a.Level == b.Level)
            {
                return a.LecticEquals(b) <= 0;
            }
            else return a.Level <= b.Level;
        }

        public static bool operator >= (LatticeSet a, LatticeSet b)
        {
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
            LatticeSet b = obj as LatticeSet;
            if (this.Level == b.Level)
            {
                return this.LecticEquals(b) == 0;
            }
            else return false;
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
            else if (smallestAdifference.LecticPosition < smallestBdifference.LecticPosition) return 1;
            else return -1;
        }
    }
}
