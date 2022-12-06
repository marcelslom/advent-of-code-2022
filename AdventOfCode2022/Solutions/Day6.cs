namespace AdventOfCode2022.Solutions
{
    internal class Day6 : ISolution
    {
        public int DayNumber => 6;

        private Day6() { }

        private string[] fileContent;

        public static Day6 Init(string fileName)
        {
            return new Day6
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            return DetectMarkerPosition(4);
        }

        private string DetectMarkerPosition(int length)
        {
            var stream = fileContent[0];
            for (var i = 0; i < stream.Length - length; i++)
            {
                if (stream.Substring(i, length).ToCharArray().Distinct().Count() == length)
                {
                    return (i + length).ToString();
                }
            }
            return "-1";
        }

        public string Part2()
        {
            return DetectMarkerPosition(14);
        }


    }
}
