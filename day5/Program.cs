using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace day5
{
    public record Range(long First, long Last);

    internal class Program
    {
        private static void Main()
        {
            const string inputRegex = @"seeds:(?: (?'seed'\d+))+\r\n\r\nseed-to-soil map:\r\n(?:(?'seedToSoil'[\d ]+)\r\n)+\r\nsoil-to-fertilizer map:\r\n(?:(?'soilToFertilizer'[\d ]+)\r\n)+\r\nfertilizer-to-water map:\r\n(?:(?'fertilizerToWater'[\d ]+)\r\n)+\r\nwater-to-light map:\r\n(?:(?'waterToLight'[\d ]+)\r\n)+\r\nlight-to-temperature map:\r\n(?:(?'lightToTemperature'[\d ]+)\r\n)+\r\ntemperature-to-humidity map:\r\n(?:(?'temperatureToHumidity'[\d ]+)\r\n)+\r\nhumidity-to-location map:\r\n(?:(?'humidityToLocation'[\d ]+)\r?\n?)+";
            const string seedRangeRegex = @"seeds:(?: (?'seedRange'\d+ \d+))+";
            try
            {
                //Parsing
                Console.WriteLine("- Read input and build Convertors");

                using var sr = new StreamReader("input.txt");

                var input = sr.ReadToEnd();

                Match match = Regex.Match(input, inputRegex);

                IEnumerable<long> part1Seeds = match.Groups["seed"].Captures.Select(c => long.Parse(c.Value));

                IEnumerable<Range> part2Seeds = Regex.Match(input, seedRangeRegex).Groups["seedRange"].Captures.Select(c =>
                {
                    var seedCouple = c.Value.Split(' ').Select(s => long.Parse(s));
                    return new Range(seedCouple.First(), seedCouple.First() + seedCouple.Last() - 1);
                });

                ConvertionProcess process = new();

                process.AddConvertionStep(BuildConvertorFromMatch(match, "seedToSoil"));
                process.AddConvertionStep(BuildConvertorFromMatch(match, "soilToFertilizer"));
                process.AddConvertionStep(BuildConvertorFromMatch(match, "fertilizerToWater"));
                process.AddConvertionStep(BuildConvertorFromMatch(match, "waterToLight"));
                process.AddConvertionStep(BuildConvertorFromMatch(match, "lightToTemperature"));
                process.AddConvertionStep(BuildConvertorFromMatch(match, "temperatureToHumidity"));
                process.AddConvertionStep(BuildConvertorFromMatch(match, "humidityToLocation"));

                //Part 1
                Stopwatch watch = Stopwatch.StartNew();
                Console.WriteLine("- Part 1 :");

                var part1Result = part1Seeds.Min(s => process.Convert(s));

                watch.Stop();
                Console.WriteLine($"The lowest location number is n°{part1Result}. Done in {watch.ElapsedMilliseconds} ms");

                //Part 2
                watch.Restart();
                Console.WriteLine("- Part 2 :");

                var part2Result = part2Seeds.Select(s => process.ConvertRange(s)).Min(s => s.Min(r => r.First));

                watch.Stop();
                Console.WriteLine($"The lowest location number is n°{part2Result} in {watch.ElapsedMilliseconds} ms");
            }
            catch (IOException e)
            {
                Console.WriteLine("Input could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private static Convertor BuildConvertorFromMatch(Match match, string groupName)
        {
            return Convertor.FromStrings(match.Groups[groupName].Captures.Select(c => c.Value));
        }
    }
}