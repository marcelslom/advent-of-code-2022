using System.Text;

namespace AdventOfCode2022.Solutions
{
    public class Day14Array : ISolution
    {
        public int DayNumber => 14;

        private Day14Array() { }

        private string[] fileContent;

        public static Day14Array Init(string fileName)
        {
            return new Day14Array
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        enum Cell
        {
            Empty, Path, Sand, Pouring
        }

        public string Part1()
        {
            var paths = fileContent.Select(lines => lines.Split(" -> ").Select(x => x.Split(",").Select(int.Parse).ToArray()).Select(x => (x[0], x[1])).ToArray()).ToList();
            var yMax = paths.Select(path => path.Select(x => x.Item2).Max()).Max();
            var xMin = paths.Select(path => path.Select(x => x.Item1).Min()).Min();
            var xMax = paths.Select(path => path.Select(x => x.Item1).Max()).Max();

            var cave = new Cell[(xMax - xMin + 3), yMax + 2];
            var x = cave.GetLength(0);
            var y = cave.GetLength(1);
            for (int i = 0; i < cave.Length; i++) cave[i % x, i / x] = Cell.Empty;
            var xOffset = xMin - 1;
            foreach( var path in paths)
            {
                PutPathToCave(cave, path, xOffset);
            }
            var sandStartX = 500 - xOffset;
            cave[sandStartX, 0] = Cell.Pouring;
            (int X, int Y) sand = (sandStartX, 0);
            while (true)
            {
                if (WillFallToVoid(sand, cave))
                {
                    break;
                }
                if (CanMoveDown(sand, cave))
                {
                    sand = (sand.X, sand.Y + 1);
                }
                else if (CanMoveLeft(sand, cave))
                {
                    sand = (sand.X - 1, sand.Y + 1);
                }
                else if (CanMoveRight(sand, cave))
                {
                    sand = (sand.X + 1, sand.Y + 1);
                }
                else
                {
                    cave[sand.X, sand.Y] = Cell.Sand;
                    sand = (sandStartX, 0);
                }
            }
            return cave.Cast<Cell>().Count(x => x == Cell.Sand).ToString();
        }

        private bool CanMoveRight((int X, int Y) sand, Cell[,] cave)
        {
            return cave[sand.X + 1, sand.Y + 1] == Cell.Empty;
        }

        private bool CanMoveLeft((int X, int Y) sand, Cell[,] cave)
        {
            return cave[sand.X - 1, sand.Y + 1] == Cell.Empty;
        }

        private bool CanMoveDown((int X, int Y) sand, Cell[,] cave)
        {
            return cave[sand.X, sand.Y + 1] == Cell.Empty;
        }

        private bool WillFallToVoid((int X, int Y) sand, Cell[,] cave)
        {
            var y = cave.GetLength(1);
            for (int i = sand.Y + 1; i < y; i++)
            {
                if (cave[sand.X, i] != Cell.Empty)
                    return false;
            }
            return true;
        }

        private void PutPathToCave(Cell[,] cave, (int, int)[] linePoints, int xOffset)
        {
            for (var i = 0; i < linePoints.Length - 1; i++)
            {
                var start = linePoints[i];
                var end = linePoints[i + 1];
                if (start.Item2 == end.Item2)
                {
                    var s = Math.Min(start.Item1, end.Item1) - xOffset;
                    var e = Math.Max(start.Item1, end.Item1) - xOffset;
                    for(var x = s; x <= e; x++)
                    {
                        cave[x, start.Item2] = Cell.Path;
                    }
                }
                else
                {
                    var s = Math.Min(start.Item2, end.Item2);
                    var e = Math.Max(start.Item2, end.Item2);
                    for (var y = s; y <= e; y++)
                    {
                        cave[start.Item1 - xOffset, y] = Cell.Path;
                    }
                }
            }
        }

        private void Print(Cell[,] cave, (int, int) sand)
        {
            var xMax = cave.GetLength(0);
            var yMax = cave.GetLength(1);
            var bob = new StringBuilder();
            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    var cell = cave[x,y];
                    if (cell == Cell.Pouring)
                    {
                        bob.Append('+');
                    }
                    else if (sand.Item1 == x && sand.Item2 == y)
                    {
                        bob.Append('x');
                    }
                    else if (cell == Cell.Sand)
                    {
                        bob.Append('o');
                    }
                    else if (cell == Cell.Path)
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
            Console.Clear();
            Console.WriteLine(bob.ToString());
        }


        public string Part2()
        {
            var paths = fileContent.Select(lines => lines.Split(" -> ").Select(x => x.Split(",").Select(int.Parse).ToArray()).Select(x => (x[0], x[1])).ToArray()).ToList();
            var yMax = paths.Select(path => path.Select(x => x.Item2).Max()).Max();
            var xMin = paths.Select(path => path.Select(x => x.Item1).Min()).Min();
            var xMax = paths.Select(path => path.Select(x => x.Item1).Max()).Max();

            var margin = yMax + 2;
            var caveXsize = (xMax - xMin + 3) + 2 * margin;
            var xOffset = xMin - margin - 1;
            paths.Add(new (int, int)[] { (xOffset, yMax + 2), (caveXsize - 1 + xOffset, yMax + 2) });
            var cave = new Cell[caveXsize, yMax + 3 ];
            var x = cave.GetLength(0);
            var y = cave.GetLength(1);
            for (int i = 0; i < cave.Length; i++) cave[i % x, i / x] = Cell.Empty;
            foreach (var path in paths)
            {
                PutPathToCave(cave, path, xOffset);
            }
            var sandStartX = 500 - xOffset;
            cave[sandStartX, 0] = Cell.Pouring;
            (int X, int Y) sand = (sandStartX, 0);

            while (true)
            {
                if (WillFallToVoid(sand, cave))
                {
                    break;
                }
                if (CanMoveDown(sand, cave))
                {
                    sand = (sand.X, sand.Y + 1);
                }
                else if (CanMoveLeft(sand, cave))
                {
                    sand = (sand.X - 1, sand.Y + 1);
                }
                else if (CanMoveRight(sand, cave))
                {
                    sand = (sand.X + 1, sand.Y + 1);
                }
                else if (sand == (sandStartX, 0))
                {
                    cave[sand.X, sand.Y] = Cell.Sand;
                    break;
                }
                else
                {
                    cave[sand.X, sand.Y] = Cell.Sand;
                    sand = (sandStartX, 0);
                }
            }
            return cave.Cast<Cell>().Count(x => x == Cell.Sand).ToString();
        }
    }
}
