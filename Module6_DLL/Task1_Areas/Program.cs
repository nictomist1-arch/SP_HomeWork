namespace Task1_Areas
{
    public static class Program
    {
        public static double Square(double side)
        {
            return side * side;
        }

        public static double Rectangle(double width, double height)
        {
            return width * height;
        }

        public static double Triangle(double basis, double height)
        {
            return basis * height / 2;
        }
    }
}