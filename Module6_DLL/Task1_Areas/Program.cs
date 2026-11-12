using System;
using Library = Task1_Areas.Program;

namespace Task1_Areas_App
{
    public static class Program
    {
        public static string Check()
        {
            if (Square(5) != 25 || Rectangle(3, 4) != 12 || Triangle(5, 3) != 7.5)
                throw new Exception("Ошибка расчёта площади.");

            return "Квадрат: 25" + Environment.NewLine +
                "Прямоугольник: 12" + Environment.NewLine +
                "Треугольник: 7,5";
        }

        public static double Square(double side)
        {
            return Library.Square(side);
        }

        public static double Rectangle(double width, double height)
        {
            return Library.Rectangle(width, height);
        }

        public static double Triangle(double basis, double height)
        {
            return Library.Triangle(basis, height);
        }
    }
}
