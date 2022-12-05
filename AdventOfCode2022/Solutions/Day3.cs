namespace AdventOfCode2022.Solutions
{
    internal class Day3 : ISolution
    {
        public int DayNumber => 3;

        private Day3() { }

        private string[] fileContent;

        public static Day3 Init(string fileName)
        {
            return new Day3
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        public int Part1()
        {
            var sum = 0;
            foreach (var line in fileContent)
            {
                var common = CommonItem(line);
                sum += ItemPriority(common);
            }
            return sum;
        }

        private static char CommonItem(string rucksackContent)
        {
            var firstCompartment = rucksackContent[..(rucksackContent.Length / 2)].ToCharArray();
            var secondCompartment = rucksackContent[(rucksackContent.Length / 2)..].ToCharArray();
            return firstCompartment.Intersect(secondCompartment).Single();
        }

        private int ItemPriority(char item)
        {
            return item is >= 'a' and <= 'z' ? item - 'a' + 1 : item - 'A' + 27;
        }

        public int Part2()
        {
            var sum = 0;
            for (var i = 0; i < fileContent.Length; i += 3)
            {
                var badge = fileContent[i].ToCharArray().Intersect(fileContent[i + 1].ToCharArray()).Intersect(fileContent[i + 2].ToCharArray()).Single();
                sum += ItemPriority(badge);
            }
            return sum;
        }
    }
}
