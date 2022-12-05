using AdventOfCode2022.Solutions;

namespace AdventOfCode2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var solution = Day2.Init(@"Input\day2.txt");
            PrintSolution(solution);
        }

        private static void PrintSolution(ISolution solution)
        {
            Console.WriteLine($"Day {solution.DayNumber}");
            Console.WriteLine($"Part 1: {solution.Part1()}");
            Console.WriteLine($"Part 2: {solution.Part2()}");
        }
    }
}