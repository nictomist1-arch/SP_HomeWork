using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    static ManualResetEvent generated = new ManualResetEvent(false);

    static void Main()
    {
        Thread generator = new Thread(Generate);
        Thread sum = new Thread(() => Calculate("sums.txt", '+', (a, b) => a + b));
        Thread product = new Thread(() => Calculate("products.txt", '*', (a, b) => a * b));

        sum.Start();
        product.Start();
        generator.Start();

        generator.Join();
        sum.Join();
        product.Join();

        Console.WriteLine("Готово. Файлы: pairs.txt, sums.txt, products.txt");
    }

    static void Generate()
    {
        using (StreamWriter writer = new StreamWriter("pairs.txt"))
        {
            for (int i = 0; i < 10; i++)
                writer.WriteLine(Random.Shared.Next(1, 100) + " " + Random.Shared.Next(1, 100));
        }

        Console.WriteLine("Пары чисел сгенерированы");
        generated.Set();
    }

    static void Calculate(string fileName, char sign, Func<long, long, long> operation)
    {
        generated.WaitOne();

        List<string> lines = new List<string>();
        foreach (string line in File.ReadAllLines("pairs.txt"))
        {
            string[] parts = line.Split(' ');
            long a = long.Parse(parts[0]);
            long b = long.Parse(parts[1]);
            lines.Add($"{a} {sign} {b} = {operation(a, b)}");
        }

        File.WriteAllLines(fileName, lines);
        Console.WriteLine("Записан файл " + fileName);
    }
}
