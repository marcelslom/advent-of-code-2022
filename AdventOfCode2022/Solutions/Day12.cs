namespace AdventOfCode2022.Solutions
{
    internal class Day12 : ISolution
    {
        public int DayNumber => 12;

        private Day12() { }

        private string[] fileContent;

        public static Day12 Init(string fileName)
        {
            return new Day12
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var grid = LoadGrid();
            var startNode = grid.SelectMany(x => x).Single(x => x.IsStart);
            var finishNode = grid.SelectMany(x => x).Single(x => x.IsFinish);
            var solution = FindSolution(grid, startNode, finishNode);
            return solution.ToString();
        }

        private int FindSolution(Node[][] grid, Node startNode, Node finishNode)
        {
            var astar = new AStar(grid, startNode, finishNode, SelectNeighbours);
            astar.Solve();
            var node = astar.Finish.Parent;
            var counter = 0;
            while (node != null)
            {
                node = node.Parent;
                counter++;
            }

            return counter;
        }

        private List<Node> SelectNeighbours(Node node, Node[][] grid)
        {
            var result = new List<Node>();
            if (node.X > 0 && grid[node.Y][node.X - 1].Height <= node.Height + 1)
            {
                result.Add(grid[node.Y][node.X - 1]);
            }
            if (node.X < grid[node.Y].Length - 1 && grid[node.Y][node.X + 1].Height <= node.Height + 1)
            {
                result.Add(grid[node.Y][node.X + 1]);
            }
            if (node.Y > 0 && grid[node.Y - 1][node.X].Height <= node.Height + 1)
            {
                result.Add(grid[node.Y - 1][node.X]);
            }
            if (node.Y < grid.Length - 1 && grid[node.Y + 1][node.X].Height <= node.Height + 1)
            {
                result.Add(grid[node.Y + 1][node.X]);
            }
            return result;
        }

        private Node[][] LoadGrid()
        {
            var grid = new Node[fileContent.Length][];
            for (var y = 0; y < grid.Length; y++)
            {
                var line = fileContent[y].ToCharArray();
                grid[y] = new Node[line.Length];
                for (var x = 0; x < line.Length; x++)
                {
                    var isStart = line[x] == 'S';
                    var isFinish = line[x] == 'E';
                    grid[y][x] = new Node(x, y, isStart ? 'a' : isFinish ? 'z' : line[x], 1, isStart, isFinish);
                }
            }
            return grid;
        }

        public string? Part2()
        {
            var grid = LoadGrid();
            var startNodes = grid.SelectMany(x => x).Where(x => x.Height == 'a').ToList();
            var solutions = new List<int>();
            foreach (var start in startNodes)
            {
                grid = LoadGrid();
                var startNode = grid.SelectMany(x => x).Single(x => x.X == start.X && x.Y == start.Y);
                var finishNode = grid.SelectMany(x => x).Single(x => x.IsFinish);
                solutions.Add(FindSolution(grid, startNode, finishNode));
            }
            return solutions.Where(x => x > 0).Min().ToString();
        }

        private class AStar
        {
            private readonly Func<Node, Node[][], List<Node>> getNeighboursFunc;

            public AStar(Node[][] grid, Node start, Node finish, Func<Node, Node[][], List<Node>> getNeighboursFunc)
            {
                Grid = grid;
                Start = start;
                Finish = finish;
                this.getNeighboursFunc = getNeighboursFunc;
            }

            public Node[][] Grid { get; }
            public Node Start { get; }
            public Node Finish { get; }

            private int getH(Node node)
            {
                return Math.Abs(node.X - Finish.X) + Math.Abs(node.Y - Finish.Y);
            }

            public void Solve()
            {
                var openSet = new PriorityQueue<Node, Node>(new NodeComparer());
                Start.G = 0;
                Start.H = 0;
                openSet.Enqueue(Start, Start);
                while (!(openSet.Count == 0))
                {
                    var node = openSet.Dequeue();
                    if (node == Finish)
                    {
                        return;
                    }

                    var neighbours = getNeighboursFunc(node, Grid);
                    foreach (var neighbour in neighbours)
                    {
                        var newG = node.G + neighbour.Cost;
                        if (newG < neighbour.G)
                        {
                            neighbour.G = newG;
                            neighbour.H = getH(neighbour);
                            neighbour.CalculateF();
                            neighbour.Parent = node;
                            openSet.Enqueue(neighbour, neighbour);
                        }
                    }
                }
            }

        }

        private class Node
        {
            public Node(int x, int y, char height, int cost = 1, bool isStart = false, bool isFinish = false)
            {
                X = x;
                Y = y;
                Height = height;
                Cost = cost;
                IsStart = isStart;
                IsFinish = isFinish;
            }

            public int X { get; init; }
            public int Y { get; init; }
            public int Cost { get; init; }
            public char Height { get; init; }
            public Node? Parent { get; set; }
            public bool IsStart { get; init; }
            public bool IsFinish { get; init; }
            public int F { get; private set; } = int.MaxValue;
            public int G { get; set; } = int.MaxValue;
            public int H { get; set; } = int.MaxValue;

            public void CalculateF()
            {
                F = G + H;
            }
        }

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node? x, Node? y)
            {
                return x.F == y.F ? 0 : x.F < y.F ? -1 : 1;
            }

        }

    }
}
