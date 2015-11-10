using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    public class Lattice
    {
        public List<LatticeSet> Sets { get; private set; }

        public Lattice(List<List<Attribute>> ganterResult)
        {
            GenerateLattice(ganterResult);
        }

        private void GenerateLattice(List<List<Attribute>> ganterResult)
        {
            Sets = GetLatticeSets(ganterResult).OrderBy(s => s).ToList();

            int currentLevel = Sets.Where(s => !s.Processed).Min(s => s.Level);
            var pivot = Sets.Where(s => s.Level == currentLevel).FirstOrDefault();

            foreach(var set in Sets.Where(s => s > pivot))
            {
                set.ProcessPossibleSubset(pivot);
            }

            pivot.Processed = true;
            Sets = Sets.OrderBy(s => s).ToList();
        }

        private IEnumerable<LatticeSet> GetLatticeSets(List<List<Attribute>> ganterResult)
        {
            foreach(var attributeSet in ganterResult)
            {
                yield return new LatticeSet() { AttributeSet = attributeSet, Level = 1 };
            }
        }
    }
}
