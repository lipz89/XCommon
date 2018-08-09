using System.Collections.Generic;
using System.Linq;
using XCommon.Functions;
using Xunit;

namespace XCommon.TestProject.Functions
{
    public class TestMath
    {
        [Fact]
        public void TestNaturalNumbers()
        {
            var list = new List<int>();
            foreach (var number in Math.NaturalNumbers())
            {
                list.Add(number);
                if (list.Count == 100)
                {
                    break;
                }
            }

            var list2 = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list2.Add(i);
            }

            var result = list.SequenceEqual(list2);
            Assert.True(result);
        }
        [Fact]
        public void TestPrimes()
        {
            var list = new List<int>();
            foreach (var prime in Math.Primes())
            {
                if (prime > 100)
                    break;
                list.Add(prime);
            }

            var list2 = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

            var result = list.SequenceEqual(list2);
            Assert.True(result);
        }

        [Fact]
        public void TestFibonaccis()
        {
            var list = new List<int>();
            foreach (var prime in Math.Fibonaccis())
            {
                if (prime > 100)
                    break;
                list.Add(prime);
            }

            var list2 = new List<int> { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 };

            var result = list.SequenceEqual(list2);
            Assert.True(result);
        }
    }
}
