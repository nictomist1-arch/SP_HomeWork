using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Task1_Numbers
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                Numbers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
        static void Numbers()
        {
            Console.WriteLine("Путь к файлу с числами:");
            string path = Console.ReadLine().Trim('"');
            List<int> numbers = File.ReadAllText(path)
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                .AsParallel().AsOrdered().Select(int.Parse).ToList();

            Console.WriteLine("Уникальных значений: " + numbers.AsParallel().Distinct().Count());

            int[] increasing = Longest(numbers, false);
            Console.WriteLine("Возрастающая последовательность. Длина: " + increasing.Length);
            Console.WriteLine(string.Join(" ", increasing));

            int[] positive = Longest(numbers, true);
            Console.WriteLine("Положительная последовательность. Длина: " + positive.Length);
            Console.WriteLine(string.Join(" ", positive));
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
