using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    class Program
    {
        private static FormalContext Context { get; set; }
        

        static void Main(string[] args)
        {
        }

        public static List<List<Attribute>> PerformAlgorithm()
        {
            List<Attribute> setA = new List<Attribute>();
            List<List<Attribute>> resultSets = new List<List<Attribute>>();
            bool wasFound = true;

            // adding empty set
            resultSets.Add(new List<Attribute>());

            while (wasFound)
            {
                foreach (var mi in Context.Attributes.Where(a => !setA.Contains(a)).OrderBy(a => a.LecticPosition))
                {
                    List<Attribute> closure = mi.Closure(setA, Context);
                    Attribute newMinimal = closure.Where(c => !setA.Contains(c)).OrderBy(c => c.LecticPosition).FirstOrDefault();

                    if (newMinimal == mi)
                    {
                        setA = closure;
                        resultSets.Add(closure);
                        wasFound = true;

                        if (setA.SetEquals(Context.Attributes))
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
