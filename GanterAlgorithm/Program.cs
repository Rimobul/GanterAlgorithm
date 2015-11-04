using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GanterAlgorithm
{
    class Program
    {     
        static void Main(string[] args)
        {
            var result = PerformAlgorithm(TestContext.GenerateFormalContext());

            foreach(var line in result)
            {
                Console.WriteLine("{" + string.Join(", ", line.Select(l => l.Name)) + "}");
            }

            Console.ReadLine();
        }

        public static List<List<Attribute>> PerformAlgorithm(FormalContext context)
        {
            List<Attribute> setA = context.Extent(context.Items).ToList();
            List<List<Attribute>> resultSets = new List<List<Attribute>>();
            bool wasFound = true;

            // adding empty set
            resultSets.Add(new List<Attribute>());

            while (wasFound)
            {
                foreach (var mi in context.Attributes.Where(a => !setA.Contains(a)).OrderByDescending(a => a.LecticPosition))
                {
                    List<Attribute> closure = mi.Closure(setA, context);
                    Attribute newMinimal = closure.Where(c => !setA.Contains(c)).OrderBy(c => c.LecticPosition).FirstOrDefault();

                    if (newMinimal == mi)
                    {
                        setA = closure;
                        resultSets.Add(closure);
                        wasFound = true;

                        if (setA.SetEquals(context.Attributes))
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
