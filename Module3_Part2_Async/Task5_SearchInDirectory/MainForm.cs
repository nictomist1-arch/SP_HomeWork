using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task5_SearchInDirectory
{
    public class MainForm : Form
    {
        TextBox pathBox = new TextBox { PlaceholderText = "Путь к директории" };
        Button browseButton = new Button { Text = "Обзор..." };
        TextBox wordBox = new TextBox { PlaceholderText = "Слово для поиска" };
        Button searchButton = new Button { Text = "Найти" };
        TextBox reportBox = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };

        public MainForm()
        {
            Text = "Поиск слова в директории";
            ClientSize = new Size(480, 420);

            pathBox.SetBounds(15, 15, 350, 25);
            browseButton.SetBounds(375, 13, 90, 28);
            wordBox.SetBounds(15, 50, 350, 25);
            searchButton.SetBounds(375, 48, 90, 28);
            reportBox.SetBounds(15, 90, 450, 315);

            browseButton.Click += Browse;
            searchButton.Click += Search;

            Controls.Add(pathBox);
            Controls.Add(browseButton);
            Controls.Add(wordBox);
            Controls.Add(searchButton);
            Controls.Add(reportBox);
        }

        void Browse(object? sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                pathBox.Text = dialog.SelectedPath;
        }

        async void Search(object? sender, EventArgs e)
        {
            string path = pathBox.Text;
            string word = wordBox.Text;

            if (!Directory.Exists(path) || word == "")
            {
                MessageBox.Show("Укажите существующую директорию и слово");
                return;
            }

            searchButton.Enabled = false;
            reportBox.Text = "Идет поиск...";

            string report = await Task.Run(() => SearchDirectory(path, word));

            reportBox.Text = report == "" ? "Слово не найдено" : report;
            searchButton.Enabled = true;
        }

        static string SearchDirectory(string path, string word)
        {
            EnumerationOptions options = new EnumerationOptions { RecurseSubdirectories = true, IgnoreInaccessible = true };
            StringBuilder report = new StringBuilder();

            foreach (string file in Directory.EnumerateFiles(path, "*", options))
            {
                int count;
                try
                {
                    count = CountWord(File.ReadAllText(file), word);
                }
                catch (Exception) { continue; }

                if (count > 0)
                {
                    report.AppendLine("Название файла: " + Path.GetFileName(file));
                    report.AppendLine("Путь к файлу: " + file);
                    report.AppendLine("Количество вхождений слова: " + count);
                    report.AppendLine();
                }
            }

            return report.ToString();
        }

        static int CountWord(string text, string word)
        {
            int count = 0;
            int index = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            while (index != -1)
            {
                count++;
                index = text.IndexOf(word, index + word.Length, StringComparison.OrdinalIgnoreCase);
            }
            return count;
        }
    }
}
