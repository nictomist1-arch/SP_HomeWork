using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Task1_ProgressBars
{
    public class MainForm : Form
    {
        NumericUpDown countBox = new NumericUpDown { Minimum = 1, Maximum = 15, Value = 5 };
        Button startButton = new Button { Text = "Старт" };
        FlowLayoutPanel panel = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };

        public MainForm()
        {
            Text = "Танцующие прогресс-бары";
            ClientSize = new Size(420, 480);

            Label label = new Label { Text = "Количество:", Location = new Point(15, 18), AutoSize = true };
            countBox.SetBounds(100, 15, 70, 25);
            startButton.SetBounds(185, 13, 100, 28);
            panel.SetBounds(15, 55, 390, 410);

            startButton.Click += (sender, e) => Start();

            Controls.Add(label);
            Controls.Add(countBox);
            Controls.Add(startButton);
            Controls.Add(panel);
        }

        void Start()
        {
            panel.Controls.Clear();

            for (int i = 0; i < countBox.Value; i++)
            {
                ProgressBar bar = new ProgressBar();
                bar.Size = new Size(360, 20);
                bar.Style = ProgressBarStyle.Continuous;
                bar.ForeColor = Color.FromArgb(Random.Shared.Next(256), Random.Shared.Next(256), Random.Shared.Next(256));
                panel.Controls.Add(bar);

                Thread thread = new Thread(() => Fill(bar));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        void Fill(ProgressBar bar)
        {
            int value = 0;
            while (value < 100)
            {
                value = Math.Min(100, value + Random.Shared.Next(1, 15));
                int current = value;

                try
                {
                    BeginInvoke(new Action(() => bar.Value = current));
                }
                catch (InvalidOperationException) { return; }

                Thread.Sleep(Random.Shared.Next(50, 400));
            }
        }
    }
}
