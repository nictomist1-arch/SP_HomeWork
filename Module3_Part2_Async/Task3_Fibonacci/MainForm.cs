using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task3_Fibonacci
{
    public class MainForm : Form
    {
        TextBox limitBox = new TextBox { PlaceholderText = "Верхняя граница" };
        Button startButton = new Button { Text = "Посчитать" };
        Label countLabel = new Label { AutoSize = true };
        ListBox list = new ListBox();

        public MainForm()
        {
            Text = "Числа Фибоначчи";
            ClientSize = new Size(320, 400);

            limitBox.SetBounds(15, 15, 170, 25);
            startButton.SetBounds(195, 13, 110, 28);
            countLabel.Location = new Point(15, 50);
            list.SetBounds(15, 75, 290, 310);

            startButton.Click += Calculate;

            Controls.Add(limitBox);
            Controls.Add(startButton);
            Controls.Add(countLabel);
            Controls.Add(list);
        }

        async void Calculate(object? sender, EventArgs e)
        {
            if (!long.TryParse(limitBox.Text, out long limit) || limit < 0)
            {
                MessageBox.Show("Граница должна быть целым неотрицательным числом");
                return;
            }

            startButton.Enabled = false;

            List<long> numbers = await Task.Run(() => Fibonacci(limit));

            list.Items.Clear();
            foreach (long n in numbers)
                list.Items.Add(n);

            countLabel.Text = "Найдено чисел: " + numbers.Count;
            startButton.Enabled = true;
        }

        static List<long> Fibonacci(long limit)
        {
            List<long> numbers = new List<long>();
            long a = 0, b = 1;
            while (a <= limit)
            {
                numbers.Add(a);
                if (b > long.MaxValue - a) break;
                long next = a + b;
                a = b;
                b = next;
            }
            return numbers;
        }
    }
}
