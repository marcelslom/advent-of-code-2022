using AdventOfCode2022.Solutions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace AdventOfCode2022Benchmark.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [MemoryDiagnoser]
    public class Day14Benchmark
    {
        private Day14 day14;
        private Day14Array day14array;

        [GlobalSetup]
        public void Setup()
        {
            day14 = Day14.Init(@"Input/day14.txt");
            day14array = Day14Array.Init(@"Input/day14.txt");
        }

        [Benchmark]
        public void OldApproachPart1()
        {
            day14.Part1();
        }

        [Benchmark]
        public void OldApproachPart2()
        {
            day14.Part2();
        }

        [Benchmark]
        public void ArrayPart1()
        {
            day14array.Part1();
        }

        [Benchmark]
        public void ArrayPart2()
        {
            day14array.Part2();
        }
    }
}
