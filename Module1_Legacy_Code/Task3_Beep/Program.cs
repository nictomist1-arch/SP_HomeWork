using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    [DllImport("kernel32.dll")]
    static extern bool Beep(uint frequency, uint duration);

    [DllImport("user32.dll")]
    static extern bool MessageBeep(uint type);

    const uint MB_OK = 0x00;
    const uint MB_ICONERROR = 0x10;
    const uint MB_ICONWARNING = 0x30;
    const uint MB_ICONINFORMATION = 0x40;

    static void Main()
    {
        uint[] notes = { 262, 294, 330, 349, 392, 440, 494, 523 };

        Console.WriteLine("Beep: гамма");
        foreach (uint note in notes)
        {
            Beep(note, 300);
            Thread.Sleep(200);
        }

        Console.WriteLine("MessageBeep: системные звуки");
        uint[] sounds = { MB_OK, MB_ICONINFORMATION, MB_ICONWARNING, MB_ICONERROR };
        foreach (uint sound in sounds)
        {
            MessageBeep(sound);
            Thread.Sleep(1000);
        }
    }
}
