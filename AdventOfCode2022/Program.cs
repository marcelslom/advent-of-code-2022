﻿using AdventOfCode2022.Solutions;

namespace AdventOfCode2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var solution = Day15.Init(@"test.txt");
            var solution = Day15.Init(@"Input/day15.txt");
            PrintSolution(solution);
        }

        private static void PrintSolution(ISolution solution)
        {
            Console.WriteLine($"Day {solution.DayNumber}");
            Console.WriteLine($"Part 1:\n{solution.Part1()}");
            Console.WriteLine($"Part 2:\n{solution.Part2()}");
        }
    }
}