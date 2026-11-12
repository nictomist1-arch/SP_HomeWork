using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Task2_Resumes
{
    internal class Program
    {
        static async Task Main()
        {
            try
            {
                await Resumes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
        static async Task Resumes()
        {
            Console.WriteLine("Путь к папке или к файлам резюме через ;");
            Console.WriteLine("В файле 4 строки: имя, город, стаж, зарплата.");
            string input = Console.ReadLine().Trim().Trim('"');
            string[] paths = Directory.Exists(input)
                ? Directory.GetFiles(input, "*.txt")
                : input.Split(';').Select(path => path.Trim().Trim('"')).ToArray();

            var resumes = paths.AsParallel().Select(path =>
            {
                string[] lines = File.ReadAllLines(path);
                return new
                {
                    Name = lines[0],
                    City = lines[1],
                    Experience = int.Parse(lines[2]),
                    Salary = decimal.Parse(lines[3])
                };
            }).ToArray();

            if (resumes.Length == 0)
            {
                Console.WriteLine("Резюме не найдены.");
                return;
            }

            Console.WriteLine("Введите город:");
            string city = Console.ReadLine().Trim();

            Task<string>[] reports =
            {
                Task.Run(() => "Самый опытный: " + resumes.AsParallel()
                    .OrderByDescending(r => r.Experience).First()),
                Task.Run(() => "Самый неопытный: " + resumes.AsParallel()
                    .OrderBy(r => r.Experience).First()),
                Task.Run(() => "Кандидаты из города " + city + ":" + Environment.NewLine +
                    string.Join(Environment.NewLine, resumes.AsParallel()
                        .Where(r => string.Equals(r.City, city, StringComparison.OrdinalIgnoreCase))
                        .Select(r => r.Name + ", стаж: " + r.Experience + ", зарплата: " + r.Salary))),
                Task.Run(() => "Минимальная зарплата: " + resumes.AsParallel()
                    .OrderBy(r => r.Salary).First()),
                Task.Run(() => "Максимальная зарплата: " + resumes.AsParallel()
                    .OrderByDescending(r => r.Salary).First())
            };

            string[] results = await Task.WhenAll(reports);
            foreach (string result in results)
                Console.WriteLine(result);
        }

    }
}
