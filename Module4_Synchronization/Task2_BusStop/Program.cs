using System;
using System.Threading;

class Program
{
    const int BusNumber = 175;
    const int BusCount = 3;
    const int Capacity = 30;
    const int DayLength = 10000;

    static int waiting = 0;
    static object locker = new object();
    static AutoResetEvent stopFree = new AutoResetEvent(true);
    static ManualResetEvent dayOver = new ManualResetEvent(false);

    static void Main()
    {
        Thread passengers = new Thread(Passengers);
        passengers.Start();

        Thread[] buses = new Thread[BusCount];
        for (int i = 0; i < BusCount; i++)
        {
            int id = i + 1;
            buses[i] = new Thread(() => Bus(id));
            buses[i].Start();
        }

        Thread.Sleep(DayLength);
        dayOver.Set();

        passengers.Join();
        foreach (Thread bus in buses) bus.Join();

        Console.WriteLine("День закончен. На остановке осталось людей: " + waiting);
    }

    static void Passengers()
    {
        while (!dayOver.WaitOne(500))
        {
            int came = Random.Shared.Next(1, 15);
            lock (locker)
            {
                waiting += came;
                Console.WriteLine($"Пришло {came} чел., на остановке: {waiting}");
            }
        }
    }

    static void Bus(int id)
    {
        while (!dayOver.WaitOne(Random.Shared.Next(1000, 3000)))
        {
            stopFree.WaitOne();
            lock (locker)
            {
                int taken = Math.Min(waiting, Capacity);
                waiting -= taken;
                Console.WriteLine($"Автобус №{BusNumber} (машина {id}) забрал {taken} из {Capacity}, на остановке: {waiting}");
            }
            Thread.Sleep(300);
            stopFree.Set();
        }
    }
}
