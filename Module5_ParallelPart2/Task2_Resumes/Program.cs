using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Task2_Resumes
{
    public class Program
    {
        public static async Task<string[]> Analyze(string[] paths, string city)
        {
            var resumes = await Task.Run(() => paths.AsParallel()
                .SelectMany(path => Directory.Exists(path)
                    ? Directory.GetFiles(path, "*.txt")
                    : new[] { path })
                .Select(path =>
                {
                    string[] lines = File.ReadAllLines(path);
                    return new
                    {
                        Name = lines[0],
                        City = lines[1],
                        Experience = int.Parse(lines[2]),
                        Salary = decimal.Parse(lines[3])
                    };
                }).ToArray());

            if (resumes.Length == 0)
                return new[] { "Резюме не найдены." };

            Task<string>[] reports =
            {
                Task.Run(() => "Самый опытный: " + resumes.AsParallel()
                    .OrderByDescending(r => r.Experience).Select(r =>
                        r.Name + ", стаж: " + r.Experience).First()),
                Task.Run(() => "Самый неопытный: " + resumes.AsParallel()
                    .OrderBy(r => r.Experience).Select(r =>
                        r.Name + ", стаж: " + r.Experience).First()),
                Task.Run(() => "Кандидаты из города " + city + ":" + Environment.NewLine +
                    string.Join(Environment.NewLine, resumes.AsParallel()
                        .Where(r => string.Equals(r.City, city, StringComparison.OrdinalIgnoreCase))
                        .Select(r => r.Name + ", стаж: " + r.Experience + ", зарплата: " + r.Salary))),
                Task.Run(() => "Минимальная зарплата: " + resumes.AsParallel()
                    .OrderBy(r => r.Salary).Select(r =>
                        r.Name + ", зарплата: " + r.Salary).First()),
                Task.Run(() => "Максимальная зарплата: " + resumes.AsParallel()
                    .OrderByDescending(r => r.Salary).Select(r =>
                        r.Name + ", зарплата: " + r.Salary).First())
            };
            return await Task.WhenAll(reports);
        }
    }
}