using System;
using System.Collections.Generic;
using System.Threading;

namespace Task4_Pause_Resume
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

        CancellationTokenSource[] stop = new CancellationTokenSource[2];
        ManualResetEvent[] pause = { new ManualResetEvent(true), new ManualResetEvent(true) };
        Thread[] threads = new Thread[2];

        public void Start(IProgress<long> primes, IProgress<long> fibonacci,
            long lower = 0, long upper = long.MaxValue)
        {
            foreach (Thread thread in threads)
                if (thread != null && thread.IsAlive)
                    return;
            for (int i = 0; i < 2; i++)
            {
                if (stop[i] != null)
                    stop[i].Dispose();
                stop[i] = new CancellationTokenSource();
                pause[i].Set();
            }
            threads[0] = StartThread(Primes(lower, upper, stop[0].Token), primes, stop[0].Token, pause[0]);
            threads[1] = StartThread(Fibonacci(lower, upper), fibonacci, stop[1].Token, pause[1]);
        }

        public void Stop(int index)
        {
            if (stop[index] != null)
                stop[index].Cancel();
            pause[index].Set();
        }

        public void Pause(int index)
        {
            pause[index].Reset();
        }

        public void Resume(int index)
        {
            pause[index].Set();
        }

        static IEnumerable<long> Primes(long lower, long upper, CancellationToken token)
        {
            for (long n = Math.Max(lower, 2); n <= upper; n++)
            {
                if (token.IsCancellationRequested) yield break;
                if (IsPrime(n, token)) yield return n;
                if (n == long.MaxValue) yield break;
            }
        }

        static bool IsPrime(long n, CancellationToken token)
        {
            for (long d = 2; d <= n / d; d++)
                if (token.IsCancellationRequested || n % d == 0) return false;
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
