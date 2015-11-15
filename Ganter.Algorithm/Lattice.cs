using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Algorithm
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
            var pivot = Sets.Where(s => !s.Processed).Min();
            var maxElement = Sets.Max();

            while (pivot != maxElement)
            {
                foreach (var latticeSet in Sets.Where(s => s > pivot && !s.Processed))
                {
                    latticeSet.ProcessPossibleSubset(pivot);
                }

                pivot.Processed = true;

                if (!Sets.Any(s => s.Processed))
                    break;

                pivot = Sets.Where(s => !s.Processed).Min();
                maxElement = Sets.Max();
            }
        }

        private IEnumerable<LatticeSet> GetLatticeSets(List<List<Attribute>> ganterResult)
        {
            foreach (var attributeSet in ganterResult)
            {
                yield return new LatticeSet() { AttributeSet = attributeSet, Level = 1 };
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (int level in Sets.Select(s => s.Level).Distinct())
            {
                sb.AppendLine(string.Format("{0} : {1}", level, string.Join(" + ", Sets
                                                                                .Where(s => s.Level == level)
                                                                                .Select(s => "{" + string.Join(", ", s.AttributeSet
                                                                                                                        .Select(l => l.Name)) + "}"))));
            }

            return sb.ToString();
        }
    }
}
