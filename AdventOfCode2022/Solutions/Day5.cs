using System.Text;

namespace AdventOfCode2022.Solutions
{
    internal class Day5 : ISolution
    {
        public int DayNumber => 5;

        private Day5() { }

        private string[] fileContent;

        public static Day5 Init(string fileName)
        {
            return new Day5
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var stacksInfoEndIndex = Array.FindIndex(fileContent, x => string.IsNullOrEmpty(x)) - 1;
            var stacks = LoadStacks(stacksInfoEndIndex);
            var steps = LoadSteps(stacksInfoEndIndex + 2);
            foreach (var step in steps)
            {
                Exec(stacks, step);
            }
            var sb = new StringBuilder(stacks.Count);
            foreach (var stack in stacks)
            {
                _ = sb.Append(stack.Value.Peek());
            }
            return sb.ToString();
        }

        private void Exec(Dictionary<int, Stack<char>> stacks, Step step)
        {
            for (var i = 0; i < step.Quantity; i++)
            {
                stacks[step.To].Push(stacks[step.From].Pop());
            }
        }

        private Dictionary<int, Stack<char>> LoadStacks(int stacksInfoEndIndex)
        {
            var stacks = new Dictionary<int, Stack<char>>(stacksInfoEndIndex);
            var stacksNumbersLine = fileContent[stacksInfoEndIndex];
            for (var i = 0; i < stacksNumbersLine.Length; i++)
            {
                if (stacksNumbersLine[i] == ' ')
                {
                    continue;
                }
                var stack = new Stack<char>();
                for (var j = stacksInfoEndIndex - 1; j >= 0; j--)
                {
                    var element = fileContent[j][i];
                    if (element != ' ')
                    {
                        stack.Push(element);
                    }
                }
                var stacksNumber = stacksNumbersLine[i] - '0';
                stacks.Add(stacksNumber, stack);
            }
            return stacks;
        }

        private List<Step> LoadSteps(int firstStepIndex)
        {
            var steps = new List<Step>(fileContent.Length - firstStepIndex);
            for (var i = firstStepIndex; i < fileContent.Length; i++)
            {
                steps.Add(new Step(fileContent[i]));
            }
            return steps;
        }

        public string Part2()
        {
            var stacksInfoEndIndex = Array.FindIndex(fileContent, x => string.IsNullOrEmpty(x)) - 1;
            var stacks = LoadStacks(stacksInfoEndIndex);
            var steps = LoadSteps(stacksInfoEndIndex + 2);
            foreach (var step in steps)
            {
                Exec2(stacks, step);
            }
            var sb = new StringBuilder(stacks.Count);
            foreach (var stack in stacks)
            {
                _ = sb.Append(stack.Value.Peek());
            }
            return sb.ToString();
        }

        private void Exec2(Dictionary<int, Stack<char>> stacks, Step step)
        {
            var temp = new Stack<char>();
            for (var i = 0; i < step.Quantity; i++)
            {
                temp.Push(stacks[step.From].Pop());
            }
            for (var i = 0; i < step.Quantity; i++)
            {
                stacks[step.To].Push(temp.Pop());
            }
        }

        private class Step
        {
            public int Quantity { get; init; }
            public int From { get; init; }
            public int To { get; init; }

            public Step(string description)
            {
                var splitted = description.Split(" ");
                Quantity = int.Parse(splitted[1]);
                From = int.Parse(splitted[3]);
                To = int.Parse(splitted[5]);
            }
        }
    }
}
