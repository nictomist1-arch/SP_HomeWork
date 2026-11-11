using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern IntPtr FindWindow(string? className, string? windowName);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);

    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    const uint WM_SETTEXT = 0x000C;
    const uint WM_CLOSE = 0x0010;
    const uint WM_SYSCOMMAND = 0x0112;
    const int SC_MINIMIZE = 0xF020;

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }

    class MainForm : Form
    {
        TextBox searchBox = new TextBox();
        TextBox titleBox = new TextBox();

        public MainForm()
        {
            Text = "Мое приложение";
            Width = 400;
            Height = 300;

            Label searchLabel = new Label();
            searchLabel.Text = "Заголовок окна для поиска:";
            searchLabel.SetBounds(30, 15, 320, 20);

            searchBox.Text = Text;
            searchBox.SetBounds(30, 35, 320, 25);

            Label titleLabel = new Label();
            titleLabel.Text = "Новый заголовок:";
            titleLabel.SetBounds(30, 70, 320, 20);

            titleBox.SetBounds(30, 90, 320, 25);

            Button changeButton = new Button();
            changeButton.Text = "Изменить заголовок";
            changeButton.SetBounds(30, 130, 320, 30);
            changeButton.Click += ChangeTitle;

            Button closeButton = new Button();
            closeButton.Text = "Закрыть окно";
            closeButton.SetBounds(30, 165, 320, 30);
            closeButton.Click += CloseWindow;

            Button minimizeButton = new Button();
            minimizeButton.Text = "Свернуть окно";
            minimizeButton.SetBounds(30, 200, 320, 30);
            minimizeButton.Click += MinimizeWindow;

            Controls.Add(searchLabel);
            Controls.Add(searchBox);
            Controls.Add(titleLabel);
            Controls.Add(titleBox);
            Controls.Add(changeButton);
            Controls.Add(closeButton);
            Controls.Add(minimizeButton);
        }

        IntPtr Find()
        {
            IntPtr hwnd = FindWindow(null, searchBox.Text);

            if (hwnd == IntPtr.Zero)
                MessageBox.Show("Окно не обнаружено");

            return hwnd;
        }

        void ChangeTitle(object? sender, EventArgs e)
        {
            IntPtr hwnd = Find();

            if (hwnd != IntPtr.Zero)
            {
                SendMessage(hwnd, WM_SETTEXT, IntPtr.Zero, titleBox.Text);
                searchBox.Text = titleBox.Text;
            }
        }

        void CloseWindow(object? sender, EventArgs e)
        {
            IntPtr hwnd = Find();

            if (hwnd != IntPtr.Zero)
                SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        void MinimizeWindow(object? sender, EventArgs e)
        {
            IntPtr hwnd = Find();

            if (hwnd != IntPtr.Zero)
                SendMessage(hwnd, WM_SYSCOMMAND, (IntPtr)SC_MINIMIZE, IntPtr.Zero);
        }
    }
}
