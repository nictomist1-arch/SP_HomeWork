using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Task4_5_FileSystem
{
    public static class Program
    {
        public static void CopyFile(string source, string destination)
        {
            File.Copy(source, destination);
        }

        public static void CopyDirectory(string source, string destination)
        {
            string sourcePath = Path.GetFullPath(source).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string destinationPath = Path.GetFullPath(destination).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (destinationPath.StartsWith(sourcePath, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Копию нужно сохранить за пределами исходной папки.");

            Directory.CreateDirectory(destination);
            foreach (string file in Directory.GetFiles(source))
                File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
            foreach (string folder in Directory.GetDirectories(source))
            {
                if ((File.GetAttributes(folder) & FileAttributes.ReparsePoint) != 0)
                    continue;
                CopyDirectory(folder, Path.Combine(destination, Path.GetFileName(folder)));
            }
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static void DeleteFiles(string[] paths)
        {
            foreach (string path in paths)
                File.Delete(path);
        }

        public static void DeleteByMask(string directory, string mask)
        {
            foreach (string path in Directory.GetFiles(directory, mask))
                File.Delete(path);
        }

        public static void MoveFile(string source, string destination)
        {
            File.Move(source, destination);
        }

        public static void SearchInFile(string path, string word, string reportPath)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException("Введите слово.", nameof(word));
            if (string.Equals(Path.GetFullPath(path), Path.GetFullPath(reportPath), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Отчёт нужно сохранить в другой файл.");

            int count = CountWord(File.ReadAllText(path), word);
            string report = "Файл: " + path + Environment.NewLine +
                "Слово: " + word + Environment.NewLine +
                "Количество вхождений: " + count;
            File.WriteAllText(reportPath, report);
        }

        public static void SearchInDirectory(string directory, string word, string reportPath)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException("Введите слово.", nameof(word));

            string fullReportPath = Path.GetFullPath(reportPath);
            StringBuilder report = new StringBuilder();
            foreach (string path in GetFiles(directory))
            {
                if (string.Equals(Path.GetFullPath(path), fullReportPath, StringComparison.OrdinalIgnoreCase))
                    continue;

                int count = CountWord(File.ReadAllText(path), word);
                report.AppendLine("Файл: " + path);
                report.AppendLine("Слово: " + word);
                report.AppendLine("Количество вхождений: " + count);
                report.AppendLine();
            }
            File.WriteAllText(reportPath, report.ToString());
        }

        static IEnumerable<string> GetFiles(string directory)
        {
            foreach (string file in Directory.EnumerateFiles(directory))
                if ((File.GetAttributes(file) & FileAttributes.ReparsePoint) == 0)
                    yield return file;

            foreach (string folder in Directory.EnumerateDirectories(directory))
            {
                if ((File.GetAttributes(folder) & FileAttributes.ReparsePoint) != 0)
                    continue;
                foreach (string file in GetFiles(folder))
                    yield return file;
            }
        }

        static int CountWord(string text, string word)
        {
            return Regex.Matches(text, @"(?<!\w)" + Regex.Escape(word) + @"(?!\w)",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).Count;
        }
    }
}