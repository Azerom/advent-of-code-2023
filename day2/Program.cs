using System.Text.RegularExpressions;

const string _gameRegex = @"Game (?'gameId'\d+):(?: (?'reveal'[^;]*);?)*";
const string _revealRegex = @"((?>(?'blueCount'\d+) blue,? ?)|(?>(?'redCount'\d+) red,? ?)|(?>(?'greenCount'\d+) green,? ?))+";

const int _maxRed = 12;
const int _maxGreen = 13;
const int _maxBlue = 14;

try
{
    using (var sr = new StreamReader("input.txt"))
    {
        int possibleGamesSum = 0;
        int powerSum = 0;
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            var gameMatch = Regex.Match(line, _gameRegex);
            if (gameMatch != null)
            {
                var gameId = int.Parse(gameMatch.Groups["gameId"].Value);

                bool isPossible = true;
                var minRed = 0;
                var minGreen = 0;
                var minBlue = 0;

                foreach (Capture capture in gameMatch.Groups["reveal"].Captures.Cast<Capture>())
                {
                    var cubesMatch = Regex.Match(capture.Value, _revealRegex);
                    var redCubes = ParseCube(cubesMatch.Groups["redCount"].Value);
                    var greenCubes = ParseCube(cubesMatch.Groups["greenCount"].Value);
                    var blueCubes = ParseCube(cubesMatch.Groups["blueCount"].Value);

                    if (redCubes > minRed) minRed = redCubes;
                    if (greenCubes > minGreen) minGreen = greenCubes;
                    if (blueCubes > minBlue) minBlue = blueCubes;

                    if (redCubes > _maxRed || greenCubes > _maxGreen || blueCubes > _maxBlue)
                    {
                        isPossible = false;
                    }
                }

                var power = minRed * minGreen * minBlue;
                powerSum += power;

                if(isPossible)
                {
                    possibleGamesSum += gameId;
                }

            }
        }

        Console.WriteLine($"Sum of possible games ids : {possibleGamesSum}");
        Console.WriteLine($"Sum of set powers : {powerSum}");
    }
}
catch (IOException e)
{
    Console.WriteLine("Input could not be read:");
    Console.WriteLine(e.Message);
}

static int ParseCube(string input)
{
    if(string.IsNullOrEmpty(input)) return 0;

    return int.Parse(input);
}