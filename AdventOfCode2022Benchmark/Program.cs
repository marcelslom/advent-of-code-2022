using AdventOfCode2022Benchmark.Benchmarks;
using BenchmarkDotNet.Running;

namespace AdventOfCode2022Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Day14Benchmark>();
            Console.ReadKey();
        }
    }
}