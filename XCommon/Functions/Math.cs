using System.Collections.Generic;
using System.Linq;

namespace XCommon.Functions
{
    public static class Math
    {
        public static IEnumerable<int> AllPrimes()
        {
            var it = AllNaturalNumbers().Where(x => x > 1);
            while (true)
            {
                var n = it.Take(1).FirstOrDefault();
                yield return n;
                it = it.Where(x => x % n > 0);
            }
        }

        public static IEnumerable<int> AllNaturalNumbers()
        {
            var n = 0;
            while (true)
            {
                yield return n;
            }
        }
    }
}
