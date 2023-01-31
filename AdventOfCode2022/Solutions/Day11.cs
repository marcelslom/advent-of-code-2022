using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solutions
{
    internal class Day11 : ISolution
    {
        public int DayNumber => 11;

        private Day11() { }

        private string[] fileContent;

        public static Day11 Init(string fileName)
        {
            return new Day11
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var monkeys = LoadInput().OrderBy(x => x.Number).ToList();
            for (int i = 0; i < 20; i++)
            {
                MakeRoundPart1(monkeys);
            }

            return CalculateMonkeyBusiness(monkeys);
        }

        private static string CalculateMonkeyBusiness(List<Monkey> monkeys)
        {
            var sorted = monkeys.OrderByDescending(x => x.InspectedItemsNumber).ToList();
            var business = (long) sorted[0].InspectedItemsNumber * sorted[1].InspectedItemsNumber;
            return business.ToString();
        }

        private void MakeRoundPart1(List<Monkey> monkeys)
        {
            foreach (var monkey in monkeys)
            {
                monkey.StoreNumberOfItemsToBeInspected();
                while (monkey.HasItems())
                {
                    var item = monkey.GetItem();
                    var worry = monkey.CalculateNewWorry(item) / 3;
                    var newMonkey = monkey.ChooseMonkeyToThrowTo(worry);
                    monkeys.Single(x => x.Number == newMonkey).Items.Add(worry);
                }
            }
        }

        private List<Monkey> LoadInput()
        {
            var monkeyDescriptions = fileContent.Aggregate(new List<List<string>> { new List<string>() }, (list, value) =>
                {
                    list.Last().Add(value);
                    if (string.IsNullOrEmpty(value))
                    {
                        list.Add(new List<string>());
                    }
                    return list;
                });

            return monkeyDescriptions.Select(DecodeMonkey).ToList();
        }

        private Monkey DecodeMonkey(List<string> description)
        {
            var monkeyNumber = int.Parse(Regex.Match(description[0], @"\d+").Value);
            var startingItems = description[1].Replace("  Starting items: ", "").Split(", ").Select(long.Parse).ToList();
            var operationParams = description[2].Replace("  Operation: new = old ", "").Split(" ");
            var testFactor = int.Parse(Regex.Match(description[3], @"\d+").Value);
            var ifTrue = int.Parse(Regex.Match(description[4], @"\d+").Value);
            var ifFalse = int.Parse(Regex.Match(description[5], @"\d+").Value);
            var operationIsPower = operationParams[0] == "*" && operationParams[1] == "old";
            var operation = new Operation { Type = operationIsPower ? "^" : operationParams[0], Factor = operationIsPower ? null : int.Parse(operationParams[1]) };
            var test = new Test { Divisor = testFactor, MonkeyIfTrue = ifTrue, MonkeyIfFalse = ifFalse };
            return new Monkey { Number = monkeyNumber, Items = startingItems, Operation = operation, Test = test };
        }

        private void MakeRoundPart2(List<Monkey> monkeys)
        {
            var commonDivisor = monkeys.Aggregate(1, (d, m) => d * m.Test.Divisor);
            foreach (var monkey in monkeys)
            {
                monkey.StoreNumberOfItemsToBeInspected();
                while (monkey.HasItems())
                {
                    var item = monkey.GetItem();
                    var worry = monkey.CalculateNewWorry(item) % commonDivisor;
                    var newMonkey = monkey.ChooseMonkeyToThrowTo(worry);
                    monkeys.Single(x => x.Number == newMonkey).Items.Add(worry);
                }
            }
        }

        public string Part2()
        {
            var monkeys = LoadInput().OrderBy(x => x.Number).ToList();
            for (int i = 0; i < 10_000; i++)
            {
                MakeRoundPart2(monkeys);
            }

            return CalculateMonkeyBusiness(monkeys);
        }

        private class Monkey
        {
            public int Number { get; init; }
            public List<long> Items { get; init; }
            public Operation Operation { get; init; }
            public Test Test { get; init; }
            public int InspectedItemsNumber { get; private set; }


            public override string ToString()
            {
                return $"Monkey number: {Number}; Inspected items number: {InspectedItemsNumber}; Items: {string.Join(',', Items)}; Operation: {Operation}; Test: {Test};";
            }

            public void StoreNumberOfItemsToBeInspected()
            {
                InspectedItemsNumber += Items.Count;
            }

            public long GetItem()
            {
                var item = Items.ElementAt(0);
                Items.RemoveAt(0);
                return item;
            }

            public bool HasItems() => Items.Any();

            public long CalculateNewWorry(long oldWorry)
            {
                return Operation.Exec(oldWorry);
            }

            public int ChooseMonkeyToThrowTo(long worry)
            {
                return Test.GetMonkey(worry);
            }
        }

        private class Operation
        {
            public string Type { get; init; }
            public int? Factor { get; init; }

            public long Exec(long worry)
            {
                return Type switch
                {
                    "*" => worry * Factor.Value,
                    "+" => worry + Factor.Value,
                    "^" => worry * worry,
                    _ => throw new NotImplementedException(),
                };
            }

            public override string ToString()
            {
                return $"Operation Type: {Type}; Factor: {Factor};";
            }
        }

        private class Test
        {
            public int Divisor { get; init; }
            public int MonkeyIfTrue { get; init; }
            public int MonkeyIfFalse { get; init; }

            public int GetMonkey(long worry)
            {
                return worry % Divisor == 0 ? MonkeyIfTrue : MonkeyIfFalse;
            }

            public override string ToString()
            {
                return $"Test Factor: {Divisor}; MonkeyIfTrue: {MonkeyIfTrue}; MonkeyIfFalse: {MonkeyIfFalse};";
            }
        }

    }
}
