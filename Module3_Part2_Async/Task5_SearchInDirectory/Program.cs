using System;
using System.Collections.Generic;
using System.Threading;using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task5_SearchInDirectory
{
    public class Program
    {
        public static Task<string> Search(string path, string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException("Введите слово.", nameof(word));
            return Task.Run(() => SearchDirectory(path, word));
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
