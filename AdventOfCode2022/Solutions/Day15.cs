using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    internal class Day15 : ISolution
    {
        public int DayNumber => 15;

        private Day15() { }

        private string[] fileContent;

        public static Day15 Init(string fileName)
        {
            return new Day15
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var sensors = fileContent.Select(Sensor.Parse).ToList();
            var lineNumber = 2000000;
            var ranges = GetCoveredXsInLine(sensors, lineNumber);
            foreach(int beaconX in sensors.Where(x => x.BeaconY == lineNumber).Select(x => x.BeaconX).ToList())
            {
                var index = ranges.FindIndex(x => x.Contains(beaconX));
                if (index == -1)
                    continue;
                var range = ranges.ElementAt(index);
                ranges.Remove(range);
                ranges.AddRange(range.Exclude(beaconX));
            }

            return ranges.Sum(x => x.Size).ToString();
        }

        private static List<Range> GetCoveredXsInLine(List<Sensor> sensors, int lineNumber)
        {
            var ranges = sensors.Select(x => GetXPositionsInRange(x, lineNumber)).Where(x => x != null).Cast<Range>().ToList();
            return Range.Collapse(ranges).ToList();
        }

        private static Range? GetXPositionsInRange(Sensor sensor, int line)
        {
            var range = sensor.GetRange();
            if (line < sensor.Y - range || line > sensor.Y + range)
            {
                return null;
            }
            var offset = range - Math.Abs(sensor.Y - line);
            return new Range(sensor.X - offset, sensor.X + offset);
        }


        public string Part2()
        {
            var sensors = fileContent.Select(Sensor.Parse).ToList();
            var (X, Y) = DoWork(sensors, 4000000);
            var tuningFrequency = (long)4000000 * X + Y;
            return tuningFrequency.ToString();

            static (int X, int Y) DoWork(List<Sensor> sensors, int maxValue)
            {
                for (int i = 0; i < maxValue; i++)
                {
                    var ranges = GetCoveredXsInLine(sensors, i).OrderBy(x => x.Start).ToList();
                    if (ranges.Count > 1)
                    {
                        for (int j = 0; j < ranges.Count - 1; j++)
                        {
                            if (ranges[j].End >= 0 && ranges[j].End <= maxValue && ranges[j].End + 2 == ranges[j + 1].Start)
                            {
                                var distressBeaconX = ranges[j].End + 1;
                                var distressBeaconY = i;
                                return (distressBeaconX, distressBeaconY);
                            }
                        }
                    }
                }
                return (-1, -1);
            }
        }

        private class Range
        {
            public int Start { get; init; }
            public int End { get; init; }

            public int Size => End - Start + 1;

            public Range(int from, int to)
            {
                Start = Math.Min(from, to);
                End = Math.Max(from, to);
            }

            public override string ToString()
            {
                return $"Start: {Start} End: {End}";
            }

            public Range[] Exclude(int value)
            {
                if (!Contains(value))
                {
                    throw new Exception($"Cannot remove value {value} from range {Start} - {End}");
                }

                if (value == Start)
                {
                    return new Range[] { new Range(value + 1, End) };
                }
                else if (value == End)
                {
                    return (new Range[] { new Range(Start, value - 1) });
                }
                else
                {
                    return (new Range[] { new Range(Start, value - 1), new Range(value + 1, End) });
                }
            }

            public bool Contains(int value)
            {
                return value >= Start && value <= End;
            }

            public static IEnumerable<Range> Collapse(IEnumerable<Range> ranges)
            {
                List<Range> result = new();
                if (!ranges.Any())
                    return result;

                var ordered = ranges.OrderBy(r => r.Start).ToList();

                var min = ordered[0].Start;
                var max = ordered[0].End;

                foreach (var item in ordered.Skip(1))
                {
                    if (item.End > max && item.Start > max)
                    {
                        result.Add(new Range(min, max));
                        min = item.Start;
                    }
                    max = Math.Max(max, item.End);
                }
                result.Add(new Range(min, max));

                return result;
            }
        }

        private record Sensor(int X, int Y, int BeaconX, int BeaconY)
        {
            public static Sensor Parse(string line)
            {
                var parameters = Regex.Matches(line, @"-?\d+").Select(x => int.Parse(x.Value)).ToList();
                return new Sensor(parameters[0], parameters[1], parameters[2], parameters[3]);
            }

            public int GetRange()
            {
                return Math.Abs(X - BeaconX) + Math.Abs(Y - BeaconY);
            }
        }

    }
}
