using System;
using System.Diagnostics;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length > 0)
        {
            Console.WriteLine("Дочерний процесс. Нажмите Enter для завершения");
            Console.ReadLine();
            return 5;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Environment.ProcessPath!,
            Arguments = "child",
            UseShellExecute = true
        };

        using Process child = Process.Start(startInfo)!;
        Console.WriteLine("Запущен дочерний процесс, id: " + child.Id);
        Console.WriteLine("1 - ждать завершения");
        Console.WriteLine("2 - завершить принудительно");
        Console.Write("Ваш выбор: ");
        string choice = Console.ReadLine() ?? "";

        if (choice == "2")
        {
            if (!child.HasExited) child.Kill();
            child.WaitForExit();
            Console.WriteLine("Дочерний процесс завершен принудительно");
        }
        else
        {
            child.WaitForExit();
            Console.WriteLine("Код завершения дочернего процесса: " + child.ExitCode);
        }
        return 0;
    }
}
