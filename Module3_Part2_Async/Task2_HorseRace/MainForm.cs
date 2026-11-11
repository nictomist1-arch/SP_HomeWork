using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Task2_HorseRace
{
    public class MainForm : Form
    {
        const int HorseCount = 5;

        ProgressBar[] horses = new ProgressBar[HorseCount];
        Button startButton = new Button { Text = "Старт" };
        ListView results = new ListView { View = View.Details };
        Stopwatch watch = new Stopwatch();

        public MainForm()
        {
            Text = "Конные скачки";
            ClientSize = new Size(500, 400);

            for (int i = 0; i < HorseCount; i++)
            {
                Label label = new Label { Text = "Лошадь " + (i + 1), Location = new Point(15, 18 + i * 35), AutoSize = true };
                horses[i] = new ProgressBar();
                horses[i].SetBounds(100, 15 + i * 35, 385, 22);
                Controls.Add(label);
                Controls.Add(horses[i]);
            }

            startButton.SetBounds(15, 195, 100, 28);
            startButton.Click += (sender, e) => Start();

            results.SetBounds(15, 235, 470, 150);
            results.Columns.Add("Место", 80);
            results.Columns.Add("Лошадь", 180);
            results.Columns.Add("Время, с", 180);

            Controls.Add(startButton);
            Controls.Add(results);
        }

        void Start()
        {
            startButton.Enabled = false;
            results.Items.Clear();
            watch.Restart();

            for (int i = 0; i < HorseCount; i++)
            {
                int number = i;
                horses[i].Value = 0;

                Thread thread = new Thread(() => Run(number));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        void Run(int number)
        {
            int position = 0;
            while (position < 100)
            {
                position = Math.Min(100, position + Random.Shared.Next(1, 6));
                int current = position;
                UpdateForm(() => horses[number].Value = current);
                Thread.Sleep(100);
            }

            double time = watch.Elapsed.TotalSeconds;
            UpdateForm(() => Finish(number, time));
        }

        void Finish(int number, double time)
        {
            string place = (results.Items.Count + 1).ToString();
            results.Items.Add(new ListViewItem(new string[] { place, "Лошадь " + (number + 1), time.ToString("F2") }));

            if (results.Items.Count == HorseCount)
                startButton.Enabled = true;
        }

        void UpdateForm(Action action)
        {
            try
            {
                BeginInvoke(action);
            }
            catch (InvalidOperationException) { }
        }
    }
}
