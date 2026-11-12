using System;
using System.Collections.Generic;
using System.Threading;

namespace Task2_Primes_Fibonacci
{
    public class Program
    {
        public static Thread StartThread(IEnumerable<long> numbers, IProgress<long> progress,
            CancellationToken stop = default, ManualResetEvent pause = null)
        {
            Thread thread = new Thread(() =>
            {
                foreach (long number in numbers)
                {
                    if (pause != null)
                        pause.WaitOne();
                    if (stop.IsCancellationRequested)
                        return;
                    progress.Report(number);
                    if (stop.WaitHandle.WaitOne(100))
                        return;
                }
            });
            thread.IsBackground = true;
            thread.Start();
            return thread;
        }

        public static Thread[] Start(IProgress<long> primes, IProgress<long> fibonacci,
            long lower = 0, long upper = long.MaxValue)
        {
            return new[]
            {
                StartThread(Primes(lower, upper), primes),
                StartThread(Fibonacci(lower, upper), fibonacci)
            };
        }

        static IEnumerable<long> Primes(long lower, long upper)
        {
            for (long n = Math.Max(lower, 2); n <= upper; n++)
            {
                if (IsPrime(n)) yield return n;
                if (n == long.MaxValue) yield break;
            }
        }

        static bool IsPrime(long n)
        {
            for (long d = 2; d <= n / d; d++)
                if (n % d == 0) return false;
            return true;
        }

        static IEnumerable<long> Fibonacci(long lower, long upper)
        {
            long a = 0, b = 1;
            while (a <= upper)
            {
                if (a >= lower) yield return a;
                if (b > long.MaxValue - a) yield break;
                long next = a + b;
                a = b;
                b = next;
            }
        }
    }
}
