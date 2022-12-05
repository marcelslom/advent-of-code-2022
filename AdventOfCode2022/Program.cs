using AdventOfCode2022.Solutions;

namespace AdventOfCode2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var solution = Day4.Init(@"Input\day4.txt");
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