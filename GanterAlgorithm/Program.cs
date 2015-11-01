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

        public static void PerformAlgorithm()
        {
            List<Attribute> setA = new List<Attribute>();
            Attribute mi = Context.Attributes.Where(a => !setA.Contains(a)).OrderBy(a => a.LecticPosition).FirstOrDefault();

            List<Attribute> closure = mi.Closure(setA, Context);
            Attribute newMinimal = closure.Where(c => !setA.Contains(c)).OrderBy(c => c.LecticPosition).FirstOrDefault();

            if(newMinimal == mi)
            {
                setA = closure;
                mi = Context.Attributes.Where(a => !setA.Contains(a)).OrderBy(a => a.LecticPosition).FirstOrDefault();

                if(setA.SetEquals(Context.Attributes))
                {
                    //TODO: Win!
                }
            }
        }
    }
}
