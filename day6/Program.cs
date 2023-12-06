using System.Text.RegularExpressions;

const string inputPattern = @"T\D+(?:(?'time'\d+) *)+\r\nD\D+(?:(?'distance'\d+) *)+";

try
{
    using var sr = new StreamReader("input.txt");
    var match = Regex.Match(sr.ReadToEnd(), inputPattern);
    
    List<Race> part1Races = new();
    var nbRaces = match.Groups["time"].Captures.Count;

    for (int i = 0; i < nbRaces; i++)
    {
        part1Races.Add(
            new Race(
                int.Parse(match.Groups["time"].Captures[i].Value),
                int.Parse(match.Groups["distance"].Captures[i].Value) + 1));
    }

    Race part2Race = new(
        double.Parse(string.Join("", match.Groups["time"].Captures)),
        double.Parse(string.Join(string.Empty, match.Groups["distance"].Captures)));

    var waysToBeat = part1Races.Select(r => r.GetWaysToBeat()).Aggregate((acc, w) => acc * w);
    Console.WriteLine($"- Part1 : {waysToBeat} ways to beat the record for all races");

    Console.WriteLine($"- Part2 : {part2Race.GetWaysToBeat()} ways to beat the record");
}
catch (IOException e)
{
    Console.WriteLine("Input could not be read:");
    Console.WriteLine(e.Message);
}


class Race {
    public double Time { get; set; }

    public double BestDistance { get; set; }

    public Race(double time, double bestDistance){
        Time = time;
        BestDistance = bestDistance;
    }

    public (int Min, int Max) GetTimesToPress(){
        var min = (-Time + Math.Sqrt(Math.Pow(Time, 2) + (4 * -BestDistance))) / -2;
        var max = (-Time - Math.Sqrt(Math.Pow(Time, 2) + (4 * -BestDistance))) / -2;
        return ((int)Math.Round(min, MidpointRounding.ToPositiveInfinity), (int)Math.Round(max, MidpointRounding.ToNegativeInfinity));
    }

    public int GetWaysToBeat(){
        var (Min, Max) = GetTimesToPress();
        return Max - Min + 1;
    }
}

