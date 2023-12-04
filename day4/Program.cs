using System.Text.RegularExpressions;

string inputRegex = $@"Card +(?'cardNumber'\d+): (?: ?(?'winningNumber'\d+) )+\| (?: ?(?'numberRevealed'\d+) ?)+";

List<Card> Cards = new();

try
{
    using var sr = new StreamReader("input.txt");
    string? line;
    while ((line = sr.ReadLine()) != null)
    {
        var match = Regex.Match(line, inputRegex);

        Card newCard = new(
            id: int.Parse(match.Groups["cardNumber"].Value),
            winningNumbers: match.Groups["winningNumber"].Captures.Select(c => int.Parse(c.Value)),
            numbersRevealed: match.Groups["numberRevealed"].Captures.Select(c => int.Parse(c.Value)));

        Cards.Add(newCard);
    }
}
catch (IOException e)
{
    Console.WriteLine("Input could not be read:");
    Console.WriteLine(e.Message);
}

for (int i = 0; i < Cards.Count; i++)
{
    for (int j = 1; j < Cards[i].MatchingNumbersCount + 1; j++)
    {
        Cards[i + j].Instances += Cards[i].Instances;
    }
}

Console.WriteLine($"Total cards : {Cards.Sum(c => c.Instances)}");
Console.WriteLine($"Total points : {Cards.Sum(c => c.Worth)}");

public class Card {

    public int Id { get; set; }
    public IEnumerable<int> WinningNumbers { get; set; }
    public IEnumerable<int> NumbersRevealed { get; set; }

    public int Instances { get; set; } = 1;

    public int MatchingNumbersCount => NumbersRevealed.Count( r => WinningNumbers.Contains(r));
    public int Worth => (int)Math.Pow(2, MatchingNumbersCount - 1);

    public Card(int id, IEnumerable<int> winningNumbers, IEnumerable<int> numbersRevealed){
        Id = id;
        WinningNumbers = winningNumbers;
        NumbersRevealed = numbersRevealed;
    }
}