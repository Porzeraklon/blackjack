namespace Blackjack.Cards
{
    public static class Card
    {
        // Title card
        public static void TitleCard()
        {
            Console.Clear();
            List<string> title = new List<string>
            {
                "  ____  _            _        _            _       _____ _      _____",
                " |  _ \\| |          | |      | |          | |     / ____| |    |_   _|",
                " | |_) | | __ _  ___| | __   | | __ _  ___| | __ | |    | |      | |  ",
                " |  _ <| |/ _` |/ __| |/ /   | |/ _` |/ __| |/ / | |    | |      | |  ",
                " | |_) | | (_| | (__|   < |__| | (_| | (__|   <  | |____| |____ _| |_ ",
                " |____/|_|\\__,_|\\___|_|\\_\\____/ \\__,_|\\___|_|\\_\\  \\_____|______|_____|",
                "",
                "                          ┌───────┐┌───────┐",
                "                          │AS     ││10     │",
                "                          │       ││       │",
                "                          │   ♥   ││   ♥   │",
                "                          │       ││       │",
                "                          │     AS││     10│",
                "                          └───────┘└───────┘",
                "                        Press any key to start"
            };

            foreach (string line in title)
            {
                Console.WriteLine(line);
            }
        }

        // Deck pattern
        public static List<string> FillDeckPattern()
        {
            return new List<string>
                {
                    "♥02", "♥03", "♥04", "♥05", "♥06", "♥07", "♥08", "♥09", "♥10", "♥JP", "♥QN", "♥KG", "♥AS",
                    "♦02", "♦03", "♦04", "♦05", "♦06", "♦07", "♦08", "♦09", "♦10", "♦JP", "♦QN", "♦KG", "♦AS",
                    "♣02", "♣03", "♣04", "♣05", "♣06", "♣07", "♣08", "♣09", "♣10", "♣JP", "♣QN", "♣KG", "♣AS",
                    "♠02", "♠03", "♠04", "♠05", "♠06", "♠07", "♠08", "♠09", "♠10", "♠JP", "♠QN", "♠KG", "♠AS"
                };

        }
        // Card back ASCII
        public static List<string> DrawCardBack()
        {
            return new List<string>
            {
                "┌───────┐",
                "│╬╬╬╬╬╬╬│",
                "│╬░░░░░╬│",
                "│╬░░░░░╬│",
                "│╬░░░░░╬│",
                "│╬╬╬╬╬╬╬│",
                "└───────┘"
            };
        }

        // Shuffle using Fisher-Yates alg
        public static void Shuffle(List<string> deck)
        {
            Random rng = new Random();
            int n = deck.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (deck[i], deck[j]) = (deck[j], deck[i]);
            }
        }

        // Generating visual of one card
        public static List<string> GenerateCard(string card)
        {
            char symbol = card[0];
            List<string> card_list = new List<string>();
            string value = "??";
            if (card[1] == '0')
            {
                value = " " + card[2];
                card_list.Add("┌───────┐");
                card_list.Add($"│{value}     │");
                card_list.Add("│       │");
                card_list.Add($"│   {symbol}   │");
                card_list.Add("│       │");
                card_list.Add($"│     {value[1]}{value[0]}│");
                card_list.Add("└───────┘");
            }

            else
            {
                switch (card[2])
                {
                    case '0': value = "10"; break;
                    case 'P': value = "JP"; break;
                    case 'N': value = "QN"; break;
                    case 'G': value = "KG"; break;
                    case 'S': value = "AS"; break;
                }
                card_list.Add("┌───────┐");
                card_list.Add($"│{value}     │");
                card_list.Add("│       │");
                card_list.Add($"│   {symbol}   │");
                card_list.Add("│       │");
                card_list.Add($"│     {value}│");
                card_list.Add("└───────┘");
            }
            return card_list;
        }

        // Merging new card with current card set
        public static List<string> MergeCards(List<string> mainCard, List<string> newCard)
        {
            for (int i = 0; i < mainCard.Count; i++)
            {
                mainCard[i] += newCard[i];
            }
            return mainCard;
        }

        // Drawing cards in CLI
        public static void DrawCards(List<string> card_list)
        {
            foreach (string line in card_list)
            {
                Console.WriteLine(line);
            }
        }
        
        // Card set worth calc
        public static int SumCards(List<string> card_list)
        {
            int sum = 0;
            foreach (string card in card_list)
            {
                switch (card[2])
                {
                    case '2': sum += 2; break;
                    case '3': sum += 3; break;
                    case '4': sum += 4; break;
                    case '5': sum += 5; break;
                    case '6': sum += 6; break;
                    case '7': sum += 7; break;
                    case '8': sum += 8; break;
                    case '9': sum += 9; break;
                    case '0': sum += 10; break;
                    case 'P': sum += 10; break;
                    case 'N': sum += 10; break;
                    case 'G': sum += 10; break;
                }
            }
            foreach (string card in card_list)
            {
                if (card[2] == 'S')
                {
                    if (sum <= 10) sum += 11;
                    else sum += 1;
                }
            }
            return sum;
        }
    }
}