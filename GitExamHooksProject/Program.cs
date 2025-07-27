using System;

namespace GitExamHooksProject
{
    public class Program
    {
        public static int Square(int x) => x * x;

        static void Main()
        {
            Console.WriteLine($"Square of 4 is {Square(4)}");
        }
    }
}
