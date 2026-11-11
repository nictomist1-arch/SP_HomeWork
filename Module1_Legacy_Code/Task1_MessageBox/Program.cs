using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    static void Main()
    {
        MessageBox(IntPtr.Zero, "Ифатев Даниэль", "Обо мне", 0);
        MessageBox(IntPtr.Zero, "Группа: РПО-2", "Моя группа", 0);
        MessageBox(IntPtr.Zero, "Разработка программного обеспечения", "Моя специальность", 0);
        MessageBox(IntPtr.Zero, "Программирование на C#", "Мои интересы", 0);
    }
}
