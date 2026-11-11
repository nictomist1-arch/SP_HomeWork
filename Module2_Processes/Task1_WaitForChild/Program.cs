using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length > 0)
        {
            Console.WriteLine("Дочерний процесс работает...");
            Thread.Sleep(2000);
            Console.WriteLine("Дочерний процесс завершается");
            return 7;
        }

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = Environment.ProcessPath!,
            Arguments = "child",
            UseShellExecute = false
        };

        using Process child = Process.Start(startInfo)!;
        Console.WriteLine("Запущен дочерний процесс, id: " + child.Id);
        child.WaitForExit();
        Console.WriteLine("Код завершения дочернего процесса: " + child.ExitCode);
        return 0;
    }
}
