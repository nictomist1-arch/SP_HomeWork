using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Task4_5_DuplicateFiles
{
    public class Program
    {
        public static Task<string> MoveOriginals(string source, string destination, string reportPath = null)
        {
            return Task.Run(() =>
            {
                string sourcePath = Path.GetFullPath(source).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                string destinationPath = Path.GetFullPath(destination).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                if (destinationPath.StartsWith(sourcePath, StringComparison.OrdinalIgnoreCase) ||
                    sourcePath.StartsWith(destinationPath, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Исходная папка и папка приёмника должны быть отдельными.");

                if (!string.IsNullOrEmpty(reportPath))
                {
                    string fullReportPath = Path.GetFullPath(reportPath);
                    if (fullReportPath.StartsWith(sourcePath, StringComparison.OrdinalIgnoreCase) ||
                        fullReportPath.StartsWith(destinationPath, StringComparison.OrdinalIgnoreCase))
                        throw new ArgumentException("Сохраните отчёт за пределами обеих папок.");
                }

                EnumerationOptions options = new EnumerationOptions
                {
                    RecurseSubdirectories = true,
                    IgnoreInaccessible = false,
                    AttributesToSkip = FileAttributes.ReparsePoint
                };
                string[] files = Directory.GetFiles(source, "*", options);
                Array.Sort(files, StringComparer.OrdinalIgnoreCase);
                Directory.CreateDirectory(destination);

                Dictionary<string, string> originals = new Dictionary<string, string>();
                StringBuilder report = new StringBuilder();
                int moved = 0;
                int duplicates = 0;
                int errors = 0;

                foreach (string file in files)
                {
                    try
                    {
                        string hash;
                        using (FileStream input = File.OpenRead(file))
                        using (SHA256 sha = SHA256.Create())
                        {
                            hash = Convert.ToBase64String(sha.ComputeHash(input));
                        }

                        if (originals.TryGetValue(hash, out string original))
                        {
                            duplicates++;
                            report.AppendLine("Двойник оставлен: " + file);
                            report.AppendLine("Оригинал: " + original);
                            continue;
                        }

                        string relativePath = Path.GetFullPath(file).Substring(sourcePath.Length);
                        string target = Path.Combine(destinationPath, relativePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(target));
                        File.Move(file, target);
                        originals.Add(hash, target);
                        moved++;
                        report.AppendLine("Перенесён: " + file + " -> " + target);
                    }
                    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                    {
                        errors++;
                        report.AppendLine("Ошибка: " + file + ". " + ex.Message);
                    }
                }

                report.AppendLine("Перенесено оригиналов: " + moved);
                report.AppendLine("Оставлено двойников: " + duplicates);
                report.AppendLine("Ошибок: " + errors);
                string result = report.ToString();
                if (!string.IsNullOrEmpty(reportPath))
                    File.WriteAllText(reportPath, result);
                return result;
            });
        }
    }
}