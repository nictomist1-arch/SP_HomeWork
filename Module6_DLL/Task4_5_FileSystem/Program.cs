using System.IO;
using System;
using Library = Task4_5_FileSystem.Program;

namespace Task4_5_FileSystem_App
{
    public static class Program
    {
        public static string Check()
        {
            string directory = Path.Combine(Path.GetTempPath(),
                "DLL_Check_" + Guid.NewGuid());
            Directory.CreateDirectory(directory);

            string source = Path.Combine(directory, "source.txt");
            string copy = Path.Combine(directory, "copy.txt");
            string moved = Path.Combine(directory, "moved.txt");
            File.WriteAllText(source, "кот КОТ котик");
            CopyFile(source, copy);
            MoveFile(copy, moved);
            if (File.Exists(copy) ||
                File.ReadAllText(moved) != File.ReadAllText(source))
                throw new Exception("Ошибка копирования или переноса.");

            string folder = Path.Combine(directory, "folder");
            Directory.CreateDirectory(Path.Combine(folder, "child"));
            File.WriteAllText(Path.Combine(folder, "child", "input.txt"), "кот");
            string folderCopy = Path.Combine(directory, "folderCopy");
            CopyDirectory(folder, folderCopy);
            string nestedFile = Path.Combine(folderCopy, "child", "input.txt");
            if (!File.Exists(nestedFile))
                throw new Exception("Ошибка копирования папки.");

            string report = Path.Combine(directory, "report.txt");
            SearchInFile(source, "кот", report);
            if (!File.ReadAllText(report).Contains("Количество вхождений: 2"))
                throw new Exception("Ошибка поиска в файле.");

            string folderReport = Path.Combine(directory, "folder-report.txt");
            SearchInDirectory(folder, "кот", folderReport);
            if (!File.ReadAllText(folderReport).Contains("Количество вхождений: 1"))
                throw new Exception("Ошибка поиска в папке.");

            DeleteFile(moved);
            if (File.Exists(moved))
                throw new Exception("Ошибка удаления файла.");
            File.WriteAllText(copy, "copy");
            File.WriteAllText(moved, "moved");
            DeleteFiles(new[] { copy, moved });
            if (File.Exists(copy) || File.Exists(moved))
                throw new Exception("Ошибка удаления файлов.");
            DeleteByMask(Path.Combine(folderCopy, "child"), "*.txt");
            if (File.Exists(nestedFile))
                throw new Exception("Ошибка удаления по маске.");

            return "Все файловые операции проверены." + Environment.NewLine +
                "Тестовые файлы и отчёты: " + directory;
        }

        public static void CopyFile(string source, string destination)
        {
            Library.CopyFile(source, destination);
        }

        public static void CopyDirectory(string source, string destination)
        {
            Library.CopyDirectory(source, destination);
        }

        public static void DeleteFile(string path)
        {
            Library.DeleteFile(path);
        }

        public static void DeleteFiles(string[] paths)
        {
            Library.DeleteFiles(paths);
        }

        public static void DeleteByMask(string directory, string mask)
        {
            Library.DeleteByMask(directory, mask);
        }

        public static void MoveFile(string source, string destination)
        {
            Library.MoveFile(source, destination);
        }

        public static void SearchInFile(string path, string word, string reportPath)
        {
            Library.SearchInFile(path, word, reportPath);
        }

        public static void SearchInDirectory(string directory, string word, string reportPath)
        {
            Library.SearchInDirectory(directory, word, reportPath);
        }
    }
}
