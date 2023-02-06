using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode2022.Solutions
{
    internal class Day13 : ISolution
    {
        public int DayNumber => 13;

        private Day13() { }

        private string[] fileContent;

        public static Day13 Init(string fileName)
        {
            return new Day13
            {
                fileContent = System.IO.File.ReadAllLines(fileName)
            };
        }

        public string Part1()
        {
            var pairs = LoadPairs();
            var solution = pairs.Where(x => Compare(x.LeftValues, x.RightValues)).Sum(x => x.Indice);
            return solution.ToString();
        }

        private bool Compare(JsonDocument leftPacket, JsonDocument rightPacket)
        {
            var left = leftPacket.RootElement.EnumerateArray().ToList();
            var right = rightPacket.RootElement.EnumerateArray().ToList();
            var result = Compare(left, right);
            return result ?? throw new Exception();
        }

        private bool? Compare(List<JsonElement> left, List<JsonElement> right)
        {
            for (int i = 0; i < Math.Min(left.Count, right.Count); i++)
            {
                var l = left[i];
                var r = right[i];
                if (l.ValueKind == JsonValueKind.Number && r.ValueKind == JsonValueKind.Number)
                {
                    var lVal = l.GetInt32();
                    var rVal = r.GetInt32();
                    if (lVal < rVal) return true;
                    else if (lVal > rVal) return false;
                    else continue;
                }
                else if (l.ValueKind == JsonValueKind.Array && r.ValueKind == JsonValueKind.Array)
                {
                    var result = Compare(l.EnumerateArray().ToList(), r.EnumerateArray().ToList());
                    if (result != null) return result;
                    else continue;
                }
                else if (l.ValueKind == JsonValueKind.Array && r.ValueKind == JsonValueKind.Number)
                {
                    var arr = new int[] { r.GetInt32() };
                    var jsonElement = JsonSerializer.SerializeToElement(arr);
                    var result = Compare(l.EnumerateArray().ToList(), jsonElement.EnumerateArray().ToList());
                    if (result != null) return result;
                    else continue;
                }
                else if (l.ValueKind == JsonValueKind.Number && r.ValueKind == JsonValueKind.Array)
                {
                    var arr = new int[] { l.GetInt32() };
                    var jsonElement = JsonSerializer.SerializeToElement(arr);
                    var result = Compare(jsonElement.EnumerateArray().ToList(), r.EnumerateArray().ToList());
                    if (result != null) return result;
                    else continue;
                }
                else throw new Exception();
            }

            return left.Count == right.Count ? null : left.Count < right.Count;
        }

        private List<Pair> LoadPairs()
        {
            var splitted = fileContent.Aggregate(new List<List<string>> { new List<string>() }, (list, value) =>
            {
                if (string.IsNullOrEmpty(value))
                {
                    list.Add(new List<string>());
                }
                else
                {
                    list.Last().Add(value);
                }
                return list;
            });
            var pairs = new List<Pair>();
            for (int i = 0; i < splitted.Count; i++)
            {
                pairs.Add(new Pair(i + 1, splitted[i][0], splitted[i][1]));
            }
            return pairs;
        }

        public string Part2()
        {
            var items = fileContent.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => JsonDocument.Parse(x)).ToList();
            var two_packet = JsonSerializer.SerializeToDocument(new int[][] { new int[] { 2 } });
            var six_packet = JsonSerializer.SerializeToDocument(new int[][] { new int[] { 6 } });
            items.Add(two_packet);
            items.Add(six_packet);
            items.Sort((x, y) => Compare(x, y) ? -1 : 1);
            var two_index = items.IndexOf(two_packet) + 1;
            var six_index = items.IndexOf(six_packet) + 1;
            return (two_index * six_index).ToString();
        }

        private class Pair : IDisposable
        {
            public int Indice { get; init; }
            public JsonDocument LeftValues { get; init; }
            public JsonDocument RightValues { get; init; }


            public Pair(int indice, string left, string right)
            {
                Indice = indice;
                LeftValues = JsonDocument.Parse(left);
                RightValues = JsonDocument.Parse(right);
            }

            public void Dispose()
            {
                LeftValues.Dispose();
                RightValues.Dispose();
            }
        }



    }
}
