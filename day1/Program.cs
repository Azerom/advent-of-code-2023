using System.Text.RegularExpressions;

var numberPattern = @"(?=(\d))|(?=(one))|(?=(two))|(?=(three))|(?=(four))|(?=(five))|(?=(six))|(?=(seven))|(?=(eight))|(?=(nine))";

try
{
    using (var sr = new StreamReader("input.txt"))
    {
        int result = 0;
        string? line;
        while ((line = sr.ReadLine()) != null)
        {

            if (line != null)
            {
                var numbersText = Regex.Matches(line, numberPattern);

                var digitInLine = numbersText.Select(n =>
                {

                    foreach (Group item in n.Groups)
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            if (char.IsDigit(item.Value.First()))
                            {
                                return item.Value.First().ToString();
                            }
                            else
                            {
                                return TextToNumber(item.Value).ToString();
                            }
                        }
                    }

                    return "";

                });

                var lineNumber = int.Parse($"{digitInLine.First()}{digitInLine.Last()}");

                result += lineNumber;
            }

        }

        Console.WriteLine(result);
    }
}
catch (IOException e)
{
    Console.WriteLine("Input could not be read:");
    Console.WriteLine(e.Message);
}

static int TextToNumber(string text)
{
    return text switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => 0,
    };
}