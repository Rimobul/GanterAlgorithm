using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ganter.Algorithm
{
    /// <summary>
    /// A class representing the lattice formed from the result of Ganter algorithm.
    /// </summary>
    public class Lattice
    {
        /// <summary>
        /// The list of sets, representing individual nodes of the lattice.
        /// </summary>
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
                Parallel.ForEach(Sets.Where(s => s > pivot && !s.Processed), l => l.ProcessPossibleSubset(pivot));

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
            return ReducedAttributeString(true);
        }

        public string ReducedAttributeString(bool includeSubsets)
        {
            StringBuilder sb = new StringBuilder("Level:\tSet:\r\n");

            foreach (var set in Sets)
            {
                sb.AppendLine(set.ToString());

                if (includeSubsets && set.Subsets.Any())
                {
                    sb.AppendLine("\t\tSubsets:");

                    foreach (var subset in set.Subsets)
                    {
                        sb.AppendLine("\t\t{" + string.Join(",", subset.AttributeSet.Select(a => a.Name)) + "}");
                    }
                }
            }

            return sb.ToString();
        }

        public string ReducedItemString(bool includeSubsets, FormalContext context)
        {
            StringBuilder sb = new StringBuilder("Level:\tSet:\r\n");

            foreach(var set in Sets)
            {
                sb.AppendLine(set.Level.ToString() + " : {" + string.Join(", ", context.Intent(set.AttributeSet).ToArray().Select(i => i.Name)) + "}");

                if(includeSubsets && set.Subsets.Any())
                {
                    sb.AppendLine("\t\tSupersets:");

                    foreach(var subset in set.Subsets)
                    {
                        sb.AppendLine("\t\t{" + string.Join(", ", context.Intent(subset.AttributeSet).ToArray().Select(i => i.Name)) + "}");
                    }
                }
            }

            return sb.ToString();
        }

        public string FullAttributeString(bool includeSubsets, FormalContext context)
        {
            StringBuilder sb = new StringBuilder("Level:\tSet:\r\n");

            foreach (var set in Sets)
            {
                sb.AppendLine(set.ToString());

                if (includeSubsets)
                {
                    var subsets = Sets.Select(s => s.AttributeSet).Where(a => !a.SetEquals(set.AttributeSet) && set.AttributeSet.Contains(a)).ToArray();
                    if (subsets == null || subsets.Length <= 0) continue;

                    sb.AppendLine("\t\tSubsets:");

                    foreach (var subset in subsets)
                    {
                        sb.AppendLine("\t\t{" + string.Join(",", subset.Select(a => a.Name)) + "}");
                    }
                }
            }

            return sb.ToString();
        }

        public string FullItemString(bool includeSubsets, FormalContext context)
        {
            StringBuilder sb = new StringBuilder("Level:\tSet:\r\n");

            foreach (var set in Sets)
            {
                sb.AppendLine(set.Level.ToString() + " : {" + string.Join(", ", context.Intent(set.AttributeSet).ToArray().Select(i => i.Name)) + "}");

                if (includeSubsets)
                {
                    var subsets = Sets.Select(s => s.AttributeSet).Where(a => !a.SetEquals(set.AttributeSet) && set.AttributeSet.Contains(a)).ToArray();
                    if (subsets == null || subsets.Length <= 0) continue;

                    sb.AppendLine("\t\tSupersets:");

                    foreach (var subset in subsets)
                    {
                        sb.AppendLine("\t\t{" + string.Join(",", context.Intent(subset).Select(a => a.Name)) + "}");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
