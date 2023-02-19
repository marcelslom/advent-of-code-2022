using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day14 : ISolution
    {
        public int DayNumber => 14;

        private Day14() { }

        private string[] fileContent;

        public static Day14 Init(string fileName)
        {
            return new Day14
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var paths = fileContent.Select(x => new Path(x)).ToArray();
            var restingSand = new Dictionary<int, List<int>>();
            var sand = (500, 0);
            while (true)
            {
                if (WillFallToVoid(sand, paths))
                {
                    break;
                }
                if (CanMoveDown(sand, paths, restingSand))
                {
                    sand = (sand.Item1, sand.Item2 + 1);
                }
                else if (CanMoveLeft(sand, paths, restingSand))
                {
                    sand = (sand.Item1 - 1, sand.Item2 + 1);
                }
                else if (CanMoveRight(sand, paths, restingSand))
                {
                    sand = (sand.Item1 + 1, sand.Item2 + 1);
                }
                else
                {
                    AddDefaultAndGet(restingSand, sand.Item2).Add(sand.Item1);
                    sand = (500, 0);
                }
                //Print(paths, restingSand, sand);
            }
            return restingSand.Values.Sum(x => x.Count).ToString();
        }

        private TValue AddDefaultAndGet<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            var element = (TValue)Activator.CreateInstance(typeof(TValue));
            dictionary.Add(key, element);
            return element;
        }

        private void Print(Path[] paths, Dictionary<int, List<int>> restingSand, (int, int) sand)
        {
            var yMax = paths.Select(x => x.Ymax).Max();
            var xMin = paths.Select(x => x.Xmin).Where(x => x != int.MinValue).Min();
            var xMax = paths.Select(x => x.Xmax).Where(x => x != int.MaxValue).Max();
            var bob = new StringBuilder();
            for (int y = 0; y <= yMax; y++)
            {
                for (int x = xMin - 50; x <= xMax + 50; x++)
                {
                    if (x == 500 && y == 0)
                    {
                        bob.Append('+');
                    }
                    else if (sand.Item1 == x && sand.Item2 == y)
                    {
                        bob.Append('x');
                    }
                    else if (restingSand.ContainsKey(y) && restingSand[y].Any(s => s == x))
                    {
                        bob.Append('o');
                    }
                    else if (paths.Any(path => path.CollidingDown((x, y)) || path.CollidingToTheSide((x, y))))
                    {
                        bob.Append('#');
                    }
                    else
                    {
                        bob.Append('.');
                    }
                }
                bob.AppendLine();
            }
            bob.AppendLine();
            Console.WriteLine(bob.ToString());
        }

        private bool CanMoveDown((int, int) sand, Path[] paths, Dictionary<int, List<int>> restingSand)
        {
            return CanMove((sand.Item1, sand.Item2 + 1), paths, restingSand, (s, p) => p.CollidingDown(s));
        }

        private bool CanMoveLeft((int, int) sand, Path[] paths, Dictionary<int, List<int>> restingSand)
        {
            return CanMoveDiagonally((sand.Item1 - 1, sand.Item2 + 1), paths, restingSand);
        }

        private bool CanMoveRight((int, int) sand, Path[] paths, Dictionary<int, List<int>> restingSand)
        {
            return CanMoveDiagonally((sand.Item1 + 1, sand.Item2 + 1), paths, restingSand);
        }

        private bool CanMoveDiagonally((int, int) newSand, Path[] paths, Dictionary<int, List<int>> restingSand)
        {
            return CanMove(newSand, paths, restingSand, (s, p) => p.CollidingDown(s) || p.CollidingToTheSide(s));
        }

        private bool CanMove((int, int) newSand, Path[] paths, Dictionary<int, List<int>> restingSand, Func<(int, int), Path, bool> checkPathFunc)
        {
            if (paths.Where(x => x.IsInRange(newSand)).Any(x => checkPathFunc(newSand, x)))
                return false;
            if (restingSand.ContainsKey(newSand.Item2) && restingSand[newSand.Item2].Any(x => x == newSand.Item1))
                return false;
            return true;
        }

        private bool WillFallToVoid((int, int) sand, Path[] paths)
        {
            return !paths.Where(x => x.Ymax > sand.Item2).Any(x => x.WillEventuallyBlock(sand));
        }

        public string Part2()
        {
            var p = fileContent.Select(x => new Path(x)).ToList();
            var yMax = p.Select(x => x.LinePoints.Select(xx => xx.Item2).Max()).Max();
            var floor = new Path(new (int, int)[] { (int.MinValue, yMax + 2), (int.MaxValue, yMax + 2) });
            p.Add(floor);
            var paths = p.ToArray();
            var restingSand = new Dictionary<int, List<int>>();
            var sand = (500, 0);
            while (true)
            {
                if (CanMoveDown(sand, paths, restingSand))
                {
                    sand = (sand.Item1, sand.Item2 + 1);
                }
                else if (CanMoveLeft(sand, paths, restingSand))
                {
                    sand = (sand.Item1 - 1, sand.Item2 + 1);
                }
                else if (CanMoveRight(sand, paths, restingSand))
                {
                    sand = (sand.Item1 + 1, sand.Item2 + 1);
                }
                else if (sand == (500, 0))
                {
                    AddDefaultAndGet(restingSand, sand.Item2).Add(sand.Item1);
                    break;
                }
                else
                {
                    AddDefaultAndGet(restingSand, sand.Item2).Add(sand.Item1);
                    sand = (500, 0);
                }
            }

            return restingSand.Values.Sum(x => x.Count).ToString();
        }

        private class Path
        {
            public (int, int)[] LinePoints { get; init; }

            public int Xmin { get; init; }
            public int Xmax { get; init; }
            public int Ymin { get; init; }
            public int Ymax { get; init; }

            public Path(string lines) : this(lines.Split(" -> ").Select(x => x.Split(",").Select(int.Parse).ToArray()).Select(x => (x[0], x[1])).ToArray())
            {
            }

            public Path((int, int)[] linePoints)
            {
                LinePoints = linePoints;
                Xmin = LinePoints.Select(x => x.Item1).Min();
                Xmax = LinePoints.Select(x => x.Item1).Max();
                Ymin = LinePoints.Select(x => x.Item2).Min();
                Ymax = LinePoints.Select(x => x.Item2).Max();
            }

            public bool CollidingDown((int, int) sand)
            {
                for (var i = 0; i < LinePoints.Length - 1; i++)
                {
                    var start = LinePoints[i];
                    var end = LinePoints[i + 1];
                    if (start.Item2 == end.Item2)
                    {
                        if (sand.Item2 == start.Item2 && Math.Min(start.Item1, end.Item1) <= sand.Item1 && sand.Item1 <= Math.Max(start.Item1, end.Item1))
                            return true;
                    }
                    else
                    {
                        if (sand.Item1 == start.Item1 && sand.Item2 == Math.Min(start.Item2, end.Item2))
                            return true;
                    }
                }
                return false;
            }

            public bool CollidingToTheSide((int, int) sand)
            {
                for (var i = 0; i < LinePoints.Length - 1; i++)
                {
                    var start = LinePoints[i];
                    var end = LinePoints[i + 1];
                    if (start.Item1 == end.Item1)
                    {
                        if (sand.Item1 == start.Item1 && Math.Min(start.Item2, end.Item2) <= sand.Item2 && sand.Item2 <= Math.Max(start.Item2, end.Item2))
                            return true;
                    }
                    else
                    {
                        if (sand.Item2 == start.Item2 && Math.Min(start.Item1, end.Item1) <= sand.Item1 && sand.Item1 <= Math.Max(start.Item1, end.Item1))
                            return true;
                    }
                }
                return false;
            }

            public bool WillEventuallyBlock((int, int) sand)
            {
                return sand.Item2 < Ymax && sand.Item1 > Xmin && sand.Item1 < Xmax;
            }

            public bool IsInRange((int, int) sand)
            {
                if (Xmin == int.MinValue && Xmax == int.MaxValue)
                    return sand.Item2 >= Ymin - 1;
                return sand.Item1 >= Xmin - 1 && sand.Item1 <= Xmax + 1 && sand.Item2 >= Ymin - 1 && sand.Item2 <= Ymax;
            }

        }

    }
}
