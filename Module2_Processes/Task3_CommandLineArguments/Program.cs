using System;
using System.Diagnostics;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 3)
        {
            Child(args);
            return;
        }

        Console.Write("Введите первое число: ");
        string first = Console.ReadLine() ?? "0";
        Console.Write("Введите второе число: ");
        string second = Console.ReadLine() ?? "0";
        Console.Write("Введите операцию (+, -, *, /): ");
        string operation = Console.ReadLine() ?? "+";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Environment.ProcessPath!,
            UseShellExecute = false
        };
        startInfo.ArgumentList.Add(first);
        startInfo.ArgumentList.Add(second);
        startInfo.ArgumentList.Add(operation);

        using Process child = Process.Start(startInfo)!;
        child.WaitForExit();
    }

    static void Child(string[] args)
    {
        Console.WriteLine("Аргументы дочернего процесса:");
        Console.WriteLine("Первое число: " + args[0]);
        Console.WriteLine("Второе число: " + args[1]);
        Console.WriteLine("Операция: " + args[2]);

        if (!double.TryParse(args[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double a) ||
            !double.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double b))
        {
            Console.WriteLine("Переданы некорректные числа");
            return;
        }

        double result;
        switch (args[2])
        {
            case "+": result = a + b; break;
            case "-": result = a - b; break;
            case "*": result = a * b; break;
            case "/":
                if (b == 0) { Console.WriteLine("На ноль делить нельзя"); return; }
                result = a / b;
                break;
            default: Console.WriteLine("Неизвестная операция"); return;
        }

        Console.WriteLine("Результат: " + result.ToString(CultureInfo.InvariantCulture));
    }
}
