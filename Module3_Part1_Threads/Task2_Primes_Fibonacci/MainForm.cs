using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Task2_Primes_Fibonacci
{
    public class MainForm : Form
    {
        TextBox lowerBox = new TextBox { PlaceholderText = "От (по умолч. 2)" };
        TextBox upperBox = new TextBox { PlaceholderText = "До (пусто = беск.)" };
        Button startButton = new Button { Text = "Старт" };
        ListBox primesList = new ListBox();
        ListBox fibList = new ListBox();

        public MainForm()
        {
            Text = "Простые числа и числа Фибоначчи";
            ClientSize = new Size(620, 390);

            lowerBox.SetBounds(15, 15, 130, 25);
            upperBox.SetBounds(155, 15, 150, 25);
            startButton.SetBounds(315, 13, 120, 28);
            primesList.SetBounds(15, 75, 290, 300);
            fibList.SetBounds(315, 75, 290, 300);

            startButton.Click += (sender, e) => Start();

            Controls.Add(new Label { Text = "Простые числа", Location = new Point(15, 52), AutoSize = true });
            Controls.Add(new Label { Text = "Числа Фибоначчи", Location = new Point(315, 52), AutoSize = true });
            Controls.Add(lowerBox);
            Controls.Add(upperBox);
            Controls.Add(startButton);
            Controls.Add(primesList);
            Controls.Add(fibList);
        }

        void Start()
        {
            long lower = 2, upper = long.MaxValue;
            if ((lowerBox.Text != "" && !long.TryParse(lowerBox.Text, out lower)) ||
                (upperBox.Text != "" && !long.TryParse(upperBox.Text, out upper)))
            {
                MessageBox.Show("Границы должны быть целыми числами");
                return;
            }

            startButton.Enabled = false;

            long fibLower = lowerBox.Text == "" ? 0 : lower;

            StartThread(Primes(lower, upper), primesList);
            StartThread(Fibonacci(fibLower, upper), fibList);
        }

        void StartThread(IEnumerable<long> numbers, ListBox list)
        {
            Thread thread = new Thread(() => Generate(numbers, list));
            thread.IsBackground = true;
            thread.Start();
        }

        void Generate(IEnumerable<long> numbers, ListBox list)
        {
            foreach (long n in numbers)
            {
                try
                {
                    BeginInvoke(new Action(() => list.Items.Add(n)));
                }
                catch (InvalidOperationException) { return; }

                Thread.Sleep(100);
            }
        }

        static IEnumerable<long> Primes(long lower, long upper)
        {
            for (long n = Math.Max(lower, 2); n <= upper; n++)
            {
                if (IsPrime(n)) yield return n;
                if (n == long.MaxValue) yield break;
            }
        }

        static bool IsPrime(long n)
        {
            for (long d = 2; d <= n / d; d++)
                if (n % d == 0) return false;
            return true;
        }

        static IEnumerable<long> Fibonacci(long lower, long upper)
        {
            long a = 0, b = 1;
            while (a <= upper)
            {
                if (a >= lower) yield return a;
                if (b > long.MaxValue - a) yield break;
                long next = a + b;
                a = b;
                b = next;
            }
        }
    }
}
