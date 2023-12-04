using day3;
using System.Text.RegularExpressions;
using System.Linq;

internal record Line(string Value, int Y, int XMax);

internal record Gear(string Id, List<Number> Elements)
{
    public int Ratio => Elements.Select(e => e.Value).Aggregate((p, e) => p * e);
}


internal class Program
{
    private static void Main(string[] args)
    {
        const string regex = @"(?:\D*(?'num'\d+)\D*)+";

        try
        {
            using var sr = new StreamReader("input.txt");

            List<Line> lines = [];
            string? line;
            int y = 0;
            List<Number> numbers = [];
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(new Line(line, y, line.Length - 1));

                var match = Regex.Match(line, regex);
                numbers.AddRange(match.Groups["num"].Captures.Cast<Capture>()
                    .Select(number =>
                    {
                        return new Number { Value = int.Parse(number.Value), Y = y, StartX = number.Index, EndX = number.Index + number.Length - 1 };
                    }));
                y++;
            }

            List<Gear> potentialGears = [];

            IEnumerable<Number> elements = numbers.Where(number =>
            {
                //Find lines close to the inspected number to scan them
                var closeLines = new List<Line>();
                if (number.Y > 0)
                {
                    closeLines.Add(lines[number.Y - 1]);
                }
                closeLines.Add(lines[number.Y]);
                if (number.Y < lines.Count - 1)
                {
                    closeLines.Add(lines[number.Y + 1]);
                }

                var report = number.ScanLines(closeLines);

                //Check if found possible gears are already reported and add the Number to their list
                //or create a new one if this is the first time we find this gear
                foreach (var gearID in report.GearsIds)
                {
                    var cGear = potentialGears.FirstOrDefault(g => g.Id == gearID);

                    if (cGear != null)
                    {
                        cGear.Elements.Add(number);
                    }
                    else
                    {
                        potentialGears.Add(new Gear(gearID, [number]));
                    }
                }

                return report.HasAdjacentSymbol;
            });

            IEnumerable<Gear> completeGears = potentialGears.Where(g => g.Elements.Count > 1);

            Console.WriteLine($"Sum is {elements.Sum(e => e.Value)}");
            Console.WriteLine($"Sum of gears ratio is {completeGears.Sum(e => e.Ratio)}");

        }
        catch (IOException e)
        {
            Console.WriteLine("Input could not be read:");
            Console.WriteLine(e.Message);
        }
    }
}