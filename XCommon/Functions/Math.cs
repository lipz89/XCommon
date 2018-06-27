using System;
using System.Collections.Generic;
using System.Linq;

namespace XCommon.Functions
{
    public static class Math
    {
        public static IEnumerable<int> NaturalNumbers()
        {
            var n = 0;
            while (true)
            {
                yield return n;
                if (n == int.MaxValue)
                    yield break;
                n++;
            }
        }
        public static IEnumerable<int> Primes()
        {
            var it = NaturalNumbers().Where(x => x > 1);
            while (true)
            {
                var n = it.Take(1).FirstOrDefault();
                yield return n;
                it = it.Where(x => x % n > 0);
            }
        }

        public static IEnumerable<int> Fibonaccis()
        {
            int x = 1, y = 1;
            while (x < int.MaxValue)
            {
                yield return x;
                y = y + x;
                x = y - x;
            }
        }
    }
}
