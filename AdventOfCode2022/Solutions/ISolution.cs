using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Solutions
{
    internal interface ISolution
    {
        int DayNumber { get; }
        int Part1();
        int Part2();
    }
}
