using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            Child(args[0], args[1]);
            return;
        }

        Console.Write("Введите путь к файлу: ");
        string path = Console.ReadLine() ?? "";
        Console.Write("Введите слово для поиска: ");
        string word = Console.ReadLine() ?? "";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Environment.ProcessPath!,
            UseShellExecute = false
        };
        startInfo.ArgumentList.Add(path);
        startInfo.ArgumentList.Add(word);

        using Process child = Process.Start(startInfo)!;
        child.WaitForExit();
    }

    static void Child(string path, string word)
    {
        Console.WriteLine("Файл: " + path);
        Console.WriteLine("Слово: " + word);

        if (!File.Exists(path) || word == "")
        {
            Console.WriteLine("Файл не найден или слово не задано");
            return;
        }

        string text = File.ReadAllText(path);
        int count = 0;
        int index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
        while (index != -1)
        {
            count++;
            index = text.IndexOf(word, index + word.Length, StringComparison.OrdinalIgnoreCase);
        }

        Console.WriteLine("Слово встречается в файле " + count + " раз(а)");
    }
}
