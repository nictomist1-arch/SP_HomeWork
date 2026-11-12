using System;
using System.Collections.Generic;
using System.Threading;using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task3_Fibonacci
{
    public class Program
    {
        public static Task<List<long>> Calculate(long limit)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException(nameof(limit));
            return Task.Run(() => Fibonacci(limit));
        }

        static List<long> Fibonacci(long limit)
        {
            List<long> numbers = new List<long>();
            long a = 0, b = 1;
            while (a <= limit)
            {
                numbers.Add(a);
                if (b > long.MaxValue - a) break;
                long next = a + b;
                a = b;
                b = next;
            }
            return numbers;
        }
    }
}
