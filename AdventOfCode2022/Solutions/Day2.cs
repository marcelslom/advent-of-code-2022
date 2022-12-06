namespace AdventOfCode2022.Solutions
{
    internal class Day2 : ISolution
    {
        public int DayNumber => 2;

        private Day2() { }

        private string[] fileContent;

        public static Day2 Init(string fileName)
        {
            return new Day2
            {
                fileContent = File.ReadAllLines(fileName)
            };
        }

        private class Shape
        {
            public const string ROCK_NAME = "Rock";
            public const string PAPER_NAME = "Paper";
            public const string SCISSORS_NAME = "Scissors";

            public static Shape Rock { get; }
            public static Shape Paper { get; }
            public static Shape Scissors { get; }

            static Shape()
            {
                var rock = new Shape
                {
                    Name = ROCK_NAME
                };
                var paper = new Shape
                {
                    Name = PAPER_NAME
                };
                var scissors = new Shape
                {
                    Name = SCISSORS_NAME
                };
                rock.DefeatsShape = scissors;
                rock.IsDefeatedByShape = paper;
                paper.DefeatsShape = rock;
                paper.IsDefeatedByShape = scissors;
                scissors.DefeatsShape = paper;
                scissors.IsDefeatedByShape = rock;
                Rock = rock;
                Paper = paper;
                Scissors = scissors;
            }

            private Shape()
            {
            }

            public Shape DefeatsShape { get; private set; }
            public Shape IsDefeatedByShape { get; private set; }

            public string Name { get; private set; }

            public bool Defeats(Shape opponentShape)
            {
                return opponentShape == DefeatsShape;
            }

            public bool IsDefeatedBy(Shape opponentShape)
            {
                return opponentShape == IsDefeatedByShape;
            }
        }

        private enum Outcome { Lost, Draw, Won }

        public string Part1()
        {
            var total = 0;
            foreach (var line in fileContent)
            {
                var moves = line.Split(' ');
                var opponentMove = MapOpponentMove(moves[0]);
                var myMove = MapMyMove(moves[1]);
                var outcome = GetOutcome(opponentMove, myMove);
                total += ShapeScore(myMove) + OutcomeScore(outcome);
            }
            return total.ToString();
        }

        private static int ShapeScore(Shape shape)
        {
            return shape.Name switch
            {
                Shape.ROCK_NAME => 1,
                Shape.PAPER_NAME => 2,
                Shape.SCISSORS_NAME => 3,
                _ => throw new ArgumentException($"Provided shape {shape} is not valid"),
            };
        }

        private static Outcome GetOutcome(Shape opponentMove, Shape myMove)
        {
            if (opponentMove == myMove)
            {
                return Outcome.Draw;
            }
            if (opponentMove.IsDefeatedBy(myMove))
            {
                return Outcome.Won;
            }
            if (opponentMove.Defeats(myMove))
            {
                return Outcome.Lost;
            }
            throw new Exception($"Combination on opponent's move {opponentMove} and your move {myMove} is unahandled.");
        }

        private static int OutcomeScore(Outcome outcome)
        {
            return outcome switch
            {
                Outcome.Lost => 0,
                Outcome.Draw => 3,
                Outcome.Won => 6,
                _ => throw new ArgumentException($"Provided outcome {outcome} is not valid")
            };
        }

        private static Shape MapOpponentMove(string shape)
        {
            return shape switch
            {
                "A" => Shape.Rock,
                "B" => Shape.Paper,
                "C" => Shape.Scissors,
                _ => throw new ArgumentException($"Provided shape {shape} is not valid")
            };
        }

        private static Shape MapMyMove(string shape)
        {
            return shape switch
            {
                "X" => Shape.Rock,
                "Y" => Shape.Paper,
                "Z" => Shape.Scissors,
                _ => throw new ArgumentException($"Provided shape {shape} is not valid")
            };
        }

        public string Part2()
        {
            var total = 0;
            foreach (var line in fileContent)
            {
                var moves = line.Split(' ');
                var opponentMove = MapOpponentMove(moves[0]);
                var end = HowShouldRoundEnd(moves[1]);
                var myMove = GetMyShape(opponentMove, end);
                total += ShapeScore(myMove) + OutcomeScore(end);
            }
            return total.ToString();
        }

        private static Outcome HowShouldRoundEnd(string end)
        {
            return end switch
            {
                "X" => Outcome.Lost,
                "Y" => Outcome.Draw,
                "Z" => Outcome.Won,
                _ => throw new ArgumentException($"Provided info how round would end {end} is not valid")
            };
        }

        private static Shape GetMyShape(Shape opponentMove, Outcome neededOutcome)
        {
            return neededOutcome switch
            {
                Outcome.Lost => opponentMove.DefeatsShape,
                Outcome.Draw => opponentMove,
                Outcome.Won => opponentMove.IsDefeatedByShape,
                _ => throw new ArgumentException($"Provided outcome {neededOutcome} is not valid")
            };
        }


    }
}
