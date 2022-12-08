namespace AdventOfCode2022.Solutions
{
    internal class Day7 : ISolution
    {
        public int DayNumber => 7;

        private Day7() { }

        private string[] fileContent;

        public static Day7 Init(string fileName)
        {
            return new Day7
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var root = LoadFilesystem();
            var size = root.Flatten().Concat(new List<Directory>() { root }).Where(x => x.Size < 100_000).Sum(x => x.Size);
            return size.ToString();
        }

        private Directory LoadFilesystem()
        {
            var root = new Directory() { Name = "/" };
            var current = root;
            var lineCounter = 0;
            while (lineCounter < fileContent.Length)
            {
                var line = fileContent[lineCounter];
                if (line.StartsWith("$"))
                {
                    var splitted = line.Split(" ");
                    var command = splitted[1];
                    switch (command)
                    {
                        case "cd":
                            var arg = splitted[2];
                            if (arg == "/")
                            {
                                current = root;
                            }
                            else
                            {
                                current = arg == ".." ? current.Parent : current.Subdirectories.Single(x => x.Name == arg);
                            }
                            lineCounter++;
                            break;
                        case "ls":
                            var lsLinesCounter = 1;
                            while (true)
                            {
                                if (lineCounter + lsLinesCounter >= fileContent.Length)
                                {
                                    break;
                                }

                                var lsLine = fileContent[lineCounter + lsLinesCounter];
                                if (lsLine.StartsWith("$"))
                                {
                                    break;
                                }

                                var ls = lsLine.Split(" ");
                                var meta = ls[0];
                                var name = ls[1];
                                if (meta == "dir")
                                {
                                    var dir = new Directory() { Name = name, Parent = current };
                                    current.Subdirectories.Add(dir);
                                }
                                else
                                {
                                    var file = new File() { Name = name, Size = int.Parse(meta), Parent = current };
                                    current.Files.Add(file);
                                }
                                lsLinesCounter++;
                            }
                            lineCounter += lsLinesCounter;
                            break;
                    }
                }
                else
                {
                    throw new Exception($"Something went wrong, line {lineCounter}: {line} is parsed in main loop.");
                }
            }
            return root;
        }

        public string Part2()
        {
            var root = LoadFilesystem();
            var spaceToFree = root.Size - 40_000_000;
            var minSpace = root.Flatten().Where(x => x.Size >= spaceToFree).Min(x => x.Size);
            return minSpace.ToString();
        }

        private class Directory
        {
            public List<File> Files { get; } = new List<File>();
            public List<Directory> Subdirectories { get; } = new List<Directory>();
            public Directory Parent { get; init; }
            public string Name { get; init; }
            public int Size => Subdirectories.Sum(x => x.Size) + Files.Sum(x => x.Size);

            public IEnumerable<Directory> Flatten()
            {
                return Subdirectories.Concat(Subdirectories.SelectMany(x => x.Flatten()));
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private class File
        {
            public string Name { get; init; }
            public int Size { get; init; }
            public Directory Parent { get; init; }

            public override string ToString()
            {
                return $"{Parent}/{Name} ({Size})";
            }
        }

    }
}
