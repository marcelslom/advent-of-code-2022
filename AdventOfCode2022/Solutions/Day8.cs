namespace AdventOfCode2022.Solutions
{
    internal class Day8 : ISolution
    {
        public int DayNumber => 8;

        private Day8() { }

        private string[] fileContent;

        public static Day8 Init(string fileName)
        {
            return new Day8
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string? Part1()
        {
            var forest = LoadForest();
            for (var y = 1; y < forest.Length - 1; y++)
            {
                for (var x = 1; x < forest[y].Length - 1; x++)
                {
                    var tree = forest[y][x];
                    tree.IsVisible = IsTreeVisible(forest, tree);
                }
            }

            var numberOfVisibleTrees = forest.SelectMany(x => x).Where(x => x.IsVisible).Count();
            return numberOfVisibleTrees.ToString();
        }

        private bool IsTreeVisible(Tree[][] forest, Tree tree)
        {
            if (CheckXBeforeTree(forest, tree))
            {
                return true;
            }

            if (CheckXAfterTree(forest, tree))
            {
                return true;
            }

            if (CheckYBeforeTree(forest, tree))
            {
                return true;
            }

            return CheckYAfterTree(forest, tree);
        }

        private bool CheckXBeforeTree(Tree[][] forest, Tree tree)
        {
            return CheckX(forest, tree, 0, tree.X);
        }

        private bool CheckXAfterTree(Tree[][] forest, Tree tree)
        {
            return CheckX(forest, tree, tree.X + 1, forest[tree.Y].Length);
        }

        private bool CheckYBeforeTree(Tree[][] forest, Tree tree)
        {
            return CheckY(forest, tree, 0, tree.Y);
        }

        private bool CheckYAfterTree(Tree[][] forest, Tree tree)
        {
            return CheckY(forest, tree, tree.Y + 1, forest.Length);
        }

        private bool CheckX(Tree[][] forest, Tree tree, int startIndex, int endIndex)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                if (forest[tree.Y][i].Height >= tree.Height)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckY(Tree[][] forest, Tree tree, int startIndex, int endIndex)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                if (forest[i][tree.X].Height >= tree.Height)
                {
                    return false;
                }
            }
            return true;
        }

        private Tree[][] LoadForest()
        {
            var forest = new Tree[fileContent.Length][];
            for (var y = 0; y < fileContent.Length; y++)
            {
                var line = fileContent[y];
                forest[y] = new Tree[line.Length];
                for (var x = 0; x < line.Length; x++)
                {
                    forest[y][x] = new Tree(line[x] - '0', y, x);
                }
            }
            return forest;
        }

        public string Part2()
        {
            var forest = LoadForest();
            for (var y = 0; y < forest.Length; y++)
            {
                for (var x = 0; x < forest[y].Length; x++)
                {
                    var tree = forest[y][x];
                    tree.ScenicScore = CalculateScenicScore(forest, tree);
                }
            }

            var maxScenicScore = forest.SelectMany(x => x).Max(x => x.ScenicScore);
            return maxScenicScore.ToString();
        }

        private int CalculateScenicScore(Tree[][] forest, Tree tree)
        {
            return CalculateScenicScoreXBefore(forest, tree)
                * CalculateScenicScoreXAfter(forest, tree)
                * CalculateScenicScoreYBefore(forest, tree)
                * CalculateScenicScoreYAfter(forest, tree);
        }

        private int CalculateScenicScoreXBefore(Tree[][] forest, Tree tree)
        {
            for (var i = tree.X - 1; i >= 0; i--)
            {
                if (forest[tree.Y][i].Height >= tree.Height)
                {
                    return tree.X - i;
                }
            }
            return tree.X;
        }

        private int CalculateScenicScoreXAfter(Tree[][] forest, Tree tree)
        {
            for (var i = tree.X + 1; i < forest[tree.Y].Length; i++)
            {
                if (forest[tree.Y][i].Height >= tree.Height)
                {
                    return i - tree.X;
                }
            }
            return forest[tree.Y].Length - tree.X - 1;
        }

        private int CalculateScenicScoreYBefore(Tree[][] forest, Tree tree)
        {
            for (var i = tree.Y - 1; i >= 0; i--)
            {
                if (forest[i][tree.X].Height >= tree.Height)
                {
                    return tree.Y - i;
                }
            }
            return tree.Y;
        }

        private int CalculateScenicScoreYAfter(Tree[][] forest, Tree tree)
        {
            for (var i = tree.Y + 1; i < forest.Length; i++)
            {
                if (forest[i][tree.X].Height >= tree.Height)
                {
                    return i - tree.Y;
                }
            }
            return forest.Length - tree.Y - 1;
        }

        private class Tree
        {
            public int Height { get; init; }
            public int Y { get; init; }
            public int X { get; init; }
            public Tree(int height, int y, int x)
            {
                Height = height;
                Y = y;
                X = x;
            }

            public bool IsVisible { get; set; } = true;
            public int ScenicScore { get; set; }

        }

    }
}
