using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Task1_Numbers
{
    public class Program
    {
        public static Task<string> Calculate(string path)
        {
            return Task.Run(() =>
            {
                List<int> numbers = File.ReadAllText(path)
                    .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                    .AsParallel().AsOrdered().Select(int.Parse).ToList();
                int unique = numbers.AsParallel().Distinct().Count();
                int[] increasing = Longest(numbers, false);
                int[] positive = Longest(numbers, true);
                return "Уникальных значений: " + unique + Environment.NewLine +
                    "Возрастающая последовательность. Длина: " + increasing.Length + Environment.NewLine +
                    string.Join(" ", increasing) + Environment.NewLine +
                    "Положительная последовательность. Длина: " + positive.Length + Environment.NewLine +
                    string.Join(" ", positive);
            });
        }

        static int[] Longest(List<int> numbers, bool positive)
        {
            int[] starts = Enumerable.Range(0, numbers.Count).AsParallel().AsOrdered()
                .Where(i => positive
                    ? numbers[i] > 0 && (i == 0 || numbers[i - 1] <= 0)
                    : i == 0 || numbers[i] <= numbers[i - 1])
                .ToArray();

            return starts.AsParallel()
                .Select(start =>
                {
                    int end = start + 1;
                    while (end < numbers.Count && (positive
                        ? numbers[end] > 0
                        : numbers[end] > numbers[end - 1]))
                        end++;
                    return new { Start = start, Length = end - start };
                })
                .OrderByDescending(part => part.Length).ThenBy(part => part.Start)
                .Select(part => numbers.Skip(part.Start).Take(part.Length).ToArray())
                .FirstOrDefault() ?? Array.Empty<int>();
        }
    }
}
