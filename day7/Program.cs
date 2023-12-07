using System.Diagnostics;
using System.Text.RegularExpressions;

namespace day7
{
    internal static partial class Program
    {

        [GeneratedRegex(@"(?'cards'[A-Z2-9]+) (?'bid'\d+)")]
        private static partial Regex InputRegex();
        private static void Main()
        {
            try
            {
                using var sr = new StreamReader("input.txt");

                Stopwatch watch = Stopwatch.StartNew();
                var matches = InputRegex().Matches(sr.ReadToEnd());

                List<Hand> hands = matches.Select(m => new Hand(m.Groups["cards"].Value, int.Parse(m.Groups["bid"].Value), true)).ToList();

                watch.Stop();
                Console.WriteLine($"Finished parsing in {watch.ElapsedMilliseconds} ms");
                watch.Restart();

                var orderedHands = hands.OrderBy(h => h.Value).ToList();

                watch.Stop();
                Console.WriteLine($"Finished sorting in {watch.ElapsedMilliseconds} ms");
                watch.Restart();

                int sumOfWinning = 0;

                for (int i = 0; i < orderedHands.Count; i++)
                {
                    sumOfWinning += orderedHands[i].Bid * (i + 1);
                }

                watch.Stop();
                Console.WriteLine($"Calculated result {sumOfWinning} in {watch.ElapsedTicks} ticks");
            }
            catch (IOException e)
            {
                Console.WriteLine("Input could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}