using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Task1_Primes
{
    public class MainForm : Form
    {
        TextBox lowerBox = new TextBox { PlaceholderText = "От (по умолч. 2)" };
        TextBox upperBox = new TextBox { PlaceholderText = "До (пусто = беск.)" };
        Button startButton = new Button { Text = "Старт" };
        ListBox primesList = new ListBox();

        public MainForm()
        {
            Text = "Простые числа";
            ClientSize = new Size(320, 400);

            lowerBox.SetBounds(15, 15, 130, 25);
            upperBox.SetBounds(155, 15, 150, 25);
            startButton.SetBounds(15, 48, 290, 28);
            primesList.SetBounds(15, 85, 290, 300);

            startButton.Click += (sender, e) => Start();

            Controls.Add(lowerBox);
            Controls.Add(upperBox);
            Controls.Add(startButton);
            Controls.Add(primesList);
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

            Thread thread = new Thread(() => Generate(Primes(lower, upper), primesList));
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
    }
}
