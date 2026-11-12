using System;
using System.Collections.Generic;
using System.Threading;using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task2_HorseRace
{
    public class Program
    {
        public static async Task<double[]> Start(IProgress<int>[] progress)
        {
            Stopwatch watch = Stopwatch.StartNew();
            Task<double>[] horses = new Task<double>[progress.Length];
            for (int i = 0; i < horses.Length; i++)
            {
                int number = i;
                horses[i] = Task.Run(() =>
                {
                    int position = 0;
                    while (position < 100)
                    {
                        position = Math.Min(100, position + Random.Shared.Next(1, 6));
                        progress[number].Report(position);
                        if (position < 100)
                            Thread.Sleep(100);
                    }
                    return watch.Elapsed.TotalSeconds;
                });
            }
            return await Task.WhenAll(horses);
        }

    }
}
