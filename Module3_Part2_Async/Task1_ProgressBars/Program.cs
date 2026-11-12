using System;
using System.Collections.Generic;
using System.Threading;using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task1_ProgressBars
{
    public class Program
    {
        public static Thread[] Start(IProgress<int>[] progress)
        {
            Thread[] threads = new Thread[progress.Length];
            for (int i = 0; i < threads.Length; i++)
            {
                int number = i;
                threads[i] = new Thread(() => Fill(progress[number]));
                threads[i].IsBackground = true;
                threads[i].Start();
            }
            return threads;
        }

        static void Fill(IProgress<int> progress)
        {
            int value = 0;
            while (value < 100)
            {
                value = Math.Min(100, value + Random.Shared.Next(1, 15));
                progress.Report(value);
                Thread.Sleep(Random.Shared.Next(50, 400));
            }
        }

    }
}
