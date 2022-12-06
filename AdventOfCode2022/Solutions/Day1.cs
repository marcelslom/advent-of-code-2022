namespace AdventOfCode2022.Solutions
{
    internal class Day1 : ISolution
    {
        private string[] fileContent;

        public int DayNumber => 1;

        private Day1() { }

        public static Day1 Init(string fileName)
        {
            return new Day1
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        public string Part2()
        {
            var elfCalories = new List<int>();
            var elfMax = 0;
            for (var i = 0; i < fileContent.Length; i++)
            {
                var line = fileContent[i];
                if (string.IsNullOrEmpty(line))
                {
                    elfCalories.Add(elfMax);
                    elfMax = 0;
                }
                else
                {
                    elfMax += int.Parse(line);
                    if (i == fileContent.Length - 1)
                    {
                        elfCalories.Add(elfMax);
                    }
                }
            }
            var sorted = elfCalories.OrderByDescending(x => x);
            return (sorted.ElementAt(0) + sorted.ElementAt(1) + sorted.ElementAt(2)).ToString();
        }

        public string Part1()
        {
            var globalMax = 0;
            var currentElfMax = 0;
            for (var i = 0; i < fileContent.Length; i++)
            {
                var line = fileContent[i];
                if (string.IsNullOrEmpty(line))
                {
                    CheckMax(currentElfMax, ref globalMax);
                    currentElfMax = 0;
                }
                else
                {
                    currentElfMax += int.Parse(line);
                    if (i == fileContent.Length - 1)
                    {
                        CheckMax(currentElfMax, ref globalMax);
                    }
                }
            }
            return globalMax.ToString();
        }

        private void CheckMax(int currentElfMax, ref int globalMax)
        {
            if (currentElfMax > globalMax)
            {
                globalMax = currentElfMax;
            }
        }

    }
}
