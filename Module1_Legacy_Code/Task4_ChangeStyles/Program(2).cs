using System;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern IntPtr FindWindow(string? className, string? windowName);

    [DllImport("user32.dll")]
    static extern bool EnumChildWindows(IntPtr hWnd, EnumWindowsProc callback, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int GetClassName(IntPtr hWnd, StringBuilder className, int maxCount);

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int index);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int index, int value);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr insertAfter, int x, int y, int width, int height, uint flags);

    const int GWL_STYLE = -16;
    const int ES_UPPERCASE = 0x0008;
    const int WS_BORDER = 0x00800000;
    const int WS_DISABLED = 0x08000000;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_FRAMECHANGED = 0x0020;

    static void Main()
    {
        IntPtr hwnd = FindWindow(null, "TargetApp");

        if (hwnd == IntPtr.Zero)
        {
            Console.WriteLine("Окно TargetApp не найдено, сначала запустите первое приложение");
            return;
        }

        EnumChildWindows(hwnd, ChangeControlStyle, IntPtr.Zero);
        Console.WriteLine("Стили элементов управления изменены");
    }

    static bool ChangeControlStyle(IntPtr hwnd, IntPtr lParam)
    {
        StringBuilder className = new StringBuilder(256);
        GetClassName(hwnd, className, className.Capacity);
        string name = className.ToString().ToUpper();

        int style = GetWindowLong(hwnd, GWL_STYLE);

        if (name.Contains("EDIT"))
            style |= ES_UPPERCASE;
        else if (name.Contains("BUTTON"))
            style |= WS_DISABLED;
        else if (name.Contains("STATIC"))
            style |= WS_BORDER;

        SetWindowLong(hwnd, GWL_STYLE, style);
        SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

        Console.WriteLine("Изменен элемент: " + name);
        return true;
    }
}
