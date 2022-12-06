namespace AdventOfCode2022.Solutions
{
    internal class Day4 : ISolution
    {
        public int DayNumber => 4;

        private Day4() { }

        private string[] fileContent;

        public static Day4 Init(string fileName)
        {
            return new Day4
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            return fileContent.Where(ElvesSectionsFullyOverlaps).Count().ToString();
        }

        private static bool ElvesSectionsFullyOverlaps(string sectionsInfo)
        {
            return ElvesSectionsOverlaps(sectionsInfo, FullyOverlaps);
        }

        private static bool ElvesSectionsOverlaps(string sectionsInfo, Func<List<int>, List<int>, bool> checkOverlapping)
        {
            List<int> firstElfSectionsRange, secondElfSectionsRange;
            DecodeRanges(sectionsInfo, out firstElfSectionsRange, out secondElfSectionsRange);
            return checkOverlapping(firstElfSectionsRange, secondElfSectionsRange) || checkOverlapping(secondElfSectionsRange, firstElfSectionsRange);
        }

        private static void DecodeRanges(string sectionsInfo, out List<int> firstElfSectionsRange, out List<int> secondElfSectionsRange)
        {
            var elvesSectionsInfo = sectionsInfo.Split(",");
            firstElfSectionsRange = elvesSectionsInfo[0].Split('-').Select(x => int.Parse(x)).ToList();
            secondElfSectionsRange = elvesSectionsInfo[1].Split('-').Select(x => int.Parse(x)).ToList();
        }

        private static bool FullyOverlaps(List<int> innerSectionIndexes, List<int> outerSectionIndexes)
        {
            return innerSectionIndexes[0] >= outerSectionIndexes[0] && innerSectionIndexes[1] <= outerSectionIndexes[1];
        }

        private static bool OverlapsAtAll(List<int> firstSectionIndexes, List<int> secondSectionIndexes)
        {
            return firstSectionIndexes[0] >= secondSectionIndexes[0] && firstSectionIndexes[0] <= secondSectionIndexes[1];
        }

        public string Part2()
        {
            return fileContent.Where(x => ElvesSectionsOverlaps(x, OverlapsAtAll)).Count().ToString();
        }
    }
}
