using System.Text;

namespace AdventOfCode2022.Solutions
{
    internal class Day10 : ISolution
    {
        public int DayNumber => 10;

        private Day10() { }

        private string[] fileContent;

        public static Day10 Init(string fileName)
        {
            return new Day10
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var signalStrengths = new List<int>();
            var X = 1;
            int clock = 0;
            foreach(var instruction in fileContent)
            {
                var splitted = instruction.Split(' ');
                switch(splitted[0])
                {
                    case "noop":
                        clock++;
                        if (CheckAndDump(clock, signalStrengths, X))
                        {
                            goto end;
                        }
                        break;
                    case "addx":
                        clock++;
                        if (CheckAndDump(clock, signalStrengths, X))
                        {
                            goto end;
                        }
                        clock++;
                        if (CheckAndDump(clock, signalStrengths, X))
                        {
                            goto end;
                        }
                        X += int.Parse(splitted[1]);
                        break;
                }
            }
            end:
            return signalStrengths.Sum().ToString();
        }

        private static bool CheckAndDump(int clock, List<int> signalStrengths, int X)
        {
            if (DumpX(clock))
            {
                signalStrengths.Add(clock * X);
            }
            return clock >= 220;
        }

        private static bool DumpX(int clock)
        {
            return (clock - 20) % 40 == 0;
        }

        public string Part2()
        {
            var screenBuffer = new char[40 * 6];
            var X = 1;
            int clock = 0;
            foreach (var instruction in fileContent)
            {
                var splitted = instruction.Split(' ');
                switch (splitted[0])
                {
                    case "noop":
                        screenBuffer[clock] = GetPixel(clock, X);
                        clock++;
                        break;
                    case "addx":
                        screenBuffer[clock] = GetPixel(clock, X);
                        clock++;
                        screenBuffer[clock] = GetPixel(clock, X);
                        clock++;
                        X += int.Parse(splitted[1]);
                        break;
                }
            }

            var lines = screenBuffer.Chunk(40);
            var bob = new StringBuilder();
            foreach (var line in lines)
            {
                bob.AppendLine(new string(line));
            }
            return bob.ToString();
        }

        private char GetPixel(int clock, int X)
        {
            var line = clock % 40;
            return Math.Abs(line - X)<= 1 ? '#' : '.';
        }

    }
}
