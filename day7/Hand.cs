namespace day7
{
    enum HandType {
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
    }

    class Hand
    {

        const int NbCardLabel = 14;

        public string Cards { get; set; }

        public double Value { get; set; }

        public int Bid { get; set; }

        public Hand(string cards, int bid, bool part2 = false)
        {
            Value = GetValue(cards, part2);
            Bid = bid;
            Cards = cards;
        }

        /// <summary>
        /// Calculate an absolute value for a given hand
        /// Hand type give values between 537824(base 10)/100000(base 14) to 3764768(base 10)/700000(base 14)
        /// Each individual card give a value according to the label and her position in the string
        /// A 'A' as a first card will give 499408(base 10)/D0000(base 14)
        /// A 5 as the third card will give 980(base 10)/500(base 14) --For part 2
        /// The value of the whole hand will be the sum of the value of the type and of the cards
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        private static double GetValue(string cards, bool part2)
        {
            double result;

            result = GetTypeValue(cards, part2);

            for (int i = 4; i >= 0; i--)
            {
                result += GetCardValue(cards[i], i, part2);
            }

            return result;
        }

        private static double GetTypeValue(string cards, bool part2)
        {

            HandType handType = 0;
            int jokerCount = cards.Count(c => c == 'J');

            //Nb of cards in identical cards groups
            IEnumerable<int> cardsGroupsCountsList = cards.Distinct().Select(d => cards.Count(c => c == d));

            //Get base type
            handType = cardsGroupsCountsList.Max() switch
            {
                5 => HandType.FiveOfAKind,
                4 => HandType.FourOfAKind,
                3 => HandType.ThreeOfAKind,
                2 => HandType.OnePair,
                _ => HandType.HighCard
            };

            //Check for FullHouse or TwoPair
            if (handType == HandType.ThreeOfAKind && cardsGroupsCountsList.Any(c => c == 2))
            {
                handType = HandType.FullHouse;
            }
            else if (handType == HandType.OnePair && cardsGroupsCountsList.Count(c => c == 2) == 2)
            {
                handType = HandType.TwoPair;
            }

            //Take Jokers into account
            if (part2 && jokerCount > 0)
            {
                handType = handType switch
                {
                    HandType.HighCard => HandType.OnePair,
                    HandType.OnePair => HandType.ThreeOfAKind,
                    HandType.TwoPair => jokerCount switch
                    {
                        1 => HandType.FullHouse,
                        2 => HandType.FourOfAKind,
                        _ => HandType.TwoPair,
                    },
                    HandType.ThreeOfAKind => HandType.FourOfAKind,
                    HandType.FullHouse => HandType.FiveOfAKind,
                    HandType.FourOfAKind => HandType.FiveOfAKind,
                    _ => handType
                };
            }

            return Math.Pow(NbCardLabel, 5) * (int)handType;
        }

        private static double GetCardValue(char card, int position, bool part2){
            
            double cardValue = Math.Pow(NbCardLabel, 4 - position);

            cardValue *= card switch {
                'A' => 13,
                'K' => 12,
                'Q' => 11,
                'J' => part2 ? 1 : 10,
                'T' => part2 ? 10 : 9,
                _   => int.Parse(card.ToString()) - (part2 ? 0 : 1)
            };

            return cardValue;
        }
    }
}