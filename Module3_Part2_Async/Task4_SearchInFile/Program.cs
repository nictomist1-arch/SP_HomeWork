using System;
using System.Collections.Generic;
using System.Threading;using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task4_SearchInFile
{
    public class Program
    {
        public static async Task<int> Search(string path, string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException("Введите слово.", nameof(word));
            string text = await File.ReadAllTextAsync(path);
            return await Task.Run(() => CountWord(text, word));
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
