using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static int[] routes = { 175, 12, 48 };
    const int Capacity = 30;
    const int DayLength = 10000;

    static Dictionary<int, int> waiting = new Dictionary<int, int>();
    static object locker = new object();
    static AutoResetEvent stopFree = new AutoResetEvent(true);
    static ManualResetEvent dayOver = new ManualResetEvent(false);

    static void Main()
    {
        foreach (int route in routes) waiting[route] = 0;

        List<Thread> threads = new List<Thread>();
        threads.Add(new Thread(Passengers));
        foreach (int route in routes)
            threads.Add(new Thread(() => Bus(route)));

        foreach (Thread thread in threads) thread.Start();

        Thread.Sleep(DayLength);
        dayOver.Set();

        foreach (Thread thread in threads) thread.Join();

        Console.WriteLine("День закончен. На остановке осталось:");
        foreach (int route in routes)
            Console.WriteLine($"  на автобус №{route}: {waiting[route]}");
    }

    static void Passengers()
    {
        while (!dayOver.WaitOne(300))
        {
            int route = routes[Random.Shared.Next(routes.Length)];
            int came = Random.Shared.Next(1, 10);
            lock (locker)
            {
                waiting[route] += came;
                Console.WriteLine($"Пришло {came} чел. на автобус №{route}, ждут его: {waiting[route]}");
            }
        }
    }

    static void Bus(int route)
    {
        while (!dayOver.WaitOne(Random.Shared.Next(1000, 3000)))
        {
            stopFree.WaitOne();
            lock (locker)
            {
                int taken = Math.Min(waiting[route], Capacity);
                waiting[route] -= taken;
                Console.WriteLine($"Автобус №{route} забрал {taken} из {Capacity}, осталось ждать: {waiting[route]}");
            }
            Thread.Sleep(300);
            stopFree.Set();
        }
    }
}
