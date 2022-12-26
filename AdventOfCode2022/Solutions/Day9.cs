namespace AdventOfCode2022.Solutions
{
    internal class Day9 : ISolution
    {
        public int DayNumber => 9;

        private Day9() { }

        private string[] fileContent;

        public static Day9 Init(string fileName)
        {
            return new Day9
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var tailPositions = new List<Position>();

            var head = new Position();
            var tail = new Position();
            tailPositions.Add(Position.Copy(tail));
            foreach (var line in fileContent)
            {
                var splitted = line.Split(' ');
                var move = splitted[0];
                var count = int.Parse(splitted[1]);

                for (var i = 0; i < count; i++)
                {
                    MoveHead(head, move);
                    if (Touching(head, tail))
                    {
                        continue;
                    }
                    if (head.X == tail.X)
                    {
                        MoveVertically(head, tail);
                    }
                    else if (head.Y == tail.Y)
                    {
                        MoveHorizontally(head, tail);
                    }
                    else
                    {
                        MoveVertically(head, tail);
                        MoveHorizontally(head, tail);
                    }
                    tailPositions.Add(Position.Copy(tail));
                }
            }
            return tailPositions.Distinct().Count().ToString();
        }

        private static void MoveHorizontally(Position lead, Position follow)
        {
            if (lead.X > follow.X)
            {
                follow.X++;
            }
            else
            {
                follow.X--;
            }
        }

        private static void MoveVertically(Position lead, Position follow)
        {
            if (lead.Y > follow.Y)
            {
                follow.Y++;
            }
            else
            {
                follow.Y--;
            }
        }

        private static void MoveHead(Position position, string move)
        {
            switch (move)
            {
                case "R":
                    position.X++;
                    break;
                case "L":
                    position.X--;
                    break;
                case "U":
                    position.Y++;
                    break;
                case "D":
                    position.Y--;
                    break;
                default:
                    throw new Exception(move);
            }
        }

        private static bool Touching(Position first, Position second)
        {
            return Math.Abs(first.X - second.X) <= 1 && Math.Abs(first.Y - second.Y) <= 1;
        }

        public string Part2()
        {
            var tailPositions = new List<Position>();
            var state = Enumerable.Range(0, 10).Select(x => new Position()).ToArray();
            tailPositions.Add(Position.Copy(state[9]));
            foreach (var line in fileContent)
            {
                var splitted = line.Split(' ');
                var move = splitted[0];
                var count = int.Parse(splitted[1]);

                for (var i = 0; i < count; i++)
                {
                    MoveHead(state[0], move);
                    for (var j = 0; j < state.Length - 1; j++)
                    {
                        var lead = state[j];
                        var follow = state[j + 1];
                        if (Touching(lead, follow))
                        {
                            continue;
                        }

                        if (lead.X == follow.X)
                        {
                            MoveVertically(lead, follow);
                        }
                        else if (lead.Y == follow.Y)
                        {
                            MoveHorizontally(lead, follow);
                        }
                        else
                        {
                            MoveVertically(lead, follow);
                            MoveHorizontally(lead, follow);
                        }
                    }

                    tailPositions.Add(Position.Copy(state[9]));
                }
            }

            return tailPositions.Distinct().Count().ToString();
        }

        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public static Position Copy(Position source)
            {
                return new() { X = source.X, Y = source.Y };
            }

            public override bool Equals(object? obj)
            {
                return obj is Position other && X == other.X && Y == other.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }
    }
}
