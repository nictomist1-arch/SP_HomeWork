using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task4_SearchInFile
{
    public class MainForm : Form
    {
        TextBox pathBox = new TextBox { PlaceholderText = "Путь к файлу" };
        Button browseButton = new Button { Text = "Обзор..." };
        TextBox wordBox = new TextBox { PlaceholderText = "Слово для поиска" };
        Button searchButton = new Button { Text = "Найти" };
        Label resultLabel = new Label { AutoSize = true };

        public MainForm()
        {
            Text = "Поиск слова в файле";
            ClientSize = new Size(480, 130);

            pathBox.SetBounds(15, 15, 350, 25);
            browseButton.SetBounds(375, 13, 90, 28);
            wordBox.SetBounds(15, 50, 350, 25);
            searchButton.SetBounds(375, 48, 90, 28);
            resultLabel.Location = new Point(15, 90);

            browseButton.Click += Browse;
            searchButton.Click += Search;

            Controls.Add(pathBox);
            Controls.Add(browseButton);
            Controls.Add(wordBox);
            Controls.Add(searchButton);
            Controls.Add(resultLabel);
        }

        void Browse(object? sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                pathBox.Text = dialog.FileName;
        }

        async void Search(object? sender, EventArgs e)
        {
            string path = pathBox.Text;
            string word = wordBox.Text;

            if (!File.Exists(path) || word == "")
            {
                MessageBox.Show("Укажите существующий файл и слово");
                return;
            }

            searchButton.Enabled = false;
            resultLabel.Text = "Идет поиск...";

            try
            {
                string text = await File.ReadAllTextAsync(path);
                int count = await Task.Run(() => CountWord(text, word));
                resultLabel.Text = "Слово \"" + word + "\" встретилось в файле " + count + " раз(а)";
            }
            catch (Exception ex)
            {
                resultLabel.Text = "Ошибка: " + ex.Message;
            }

            searchButton.Enabled = true;
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
