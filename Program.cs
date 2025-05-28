using System.ComponentModel;
using Blackjack.Cards;
namespace Blackjack
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Ensuring correct encoding is being used
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Displaying title card
            Card.TitleCard();
            Console.ReadKey();

            // Creating starting balance
            double balance = 1000;

            // Creating deck pattern (4 colors 13 cards each)
            List<string> deck_pattern = Card.FillDeckPattern();
            

            // Generating and shuffling shoe
            int num_of_decks = 6;
            List<string> deck = new List<string>();
            for (int i = 0; i < num_of_decks; i++) deck.AddRange(deck_pattern);
            Card.Shuffle(deck);

            // Genearating image of card back
            List<string> card_back = Card.DrawCardBack();
            
            // Creating lists for cards and their graphic equivalents
            List<string> player_cards = new List<string>();
            List<string> dealer_cards = new List<string>();
            List<string> drawable_player_cards = new List<string>();
            List<string> drawable_dealer_cards = new List<string>();

            // Creating varribles tracing deck state
            int next_in_deck = 0, next_player = 0, next_dealer = 0;

            // Creating varribles tracing blackjacks and double down
            bool blackjack_player =false, blackjack_dealer = false, double_down = false;
            string action = "";

            // Main game loop
            while (balance > 0)
            {
                // Clearing console for better immersion
                Console.Clear();

                // Creating varrible for player's bet
                double bet = 0;

                // Loop for placing bet
                while (true)
                {
                    Console.WriteLine($"Balance: {balance} $");
                    Console.WriteLine("To exit game press Ctrl + C");
                    Console.Write("Place your bet: ");

                    // Reading player's bet
                    string input = Console.ReadLine() ?? "";


                    // Checking for imput errors
                    if (!double.TryParse(input, out bet))
                    {
                        Console.WriteLine("It's not a number, try again.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    if (bet <= 0)
                    {
                        Console.WriteLine("Bet must be greater then zero.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    if (bet > balance)
                    {
                        Console.WriteLine("Your balance is too low for that bet.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    // If bet is correct, subtracking it from balance and breaking loop
                    balance -= bet;
                    break;
                }

                // Reseting all lists
                dealer_cards.Clear();
                player_cards.Clear();
                drawable_dealer_cards.Clear();
                drawable_player_cards.Clear();

                for (int i = 0; i < 7; i++)
                {
                    drawable_dealer_cards.Add("");
                    drawable_player_cards.Add("");
                }

                // Checking for deck penetration
                if (next_in_deck >= (int)(0.75 * num_of_decks * 52)) Card.Shuffle(deck);

                // Dealing starting cards
                int sum_player = 0, sum_dealer = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0) player_cards.Add(deck[next_in_deck]);
                    if (i == 1) dealer_cards.Add(deck[next_in_deck + i]);
                    if (i == 2) player_cards.Add(deck[next_in_deck + i]);
                    if (i == 3) dealer_cards.Add(deck[next_in_deck + i]);
                }

                // Setting tracking varribles correctly
                next_in_deck += 4;
                next_player = 2;
                next_dealer = 2;

                // Displaying starting cards, and calculating value of hands
                Console.Clear();
                Console.WriteLine($"Bet: {bet}");
                Console.WriteLine();

                sum_dealer = Card.SumCards(dealer_cards);
                Console.WriteLine("Dealer's crads:");
                Card.MergeCards(drawable_dealer_cards, Card.GenerateCard(dealer_cards[0]));
                Card.MergeCards(drawable_dealer_cards, card_back);
                Card.DrawCards(drawable_dealer_cards);

                Console.WriteLine();

                sum_player = Card.SumCards(player_cards);
                Console.WriteLine("Player's cards:");
                Card.MergeCards(drawable_player_cards, Card.GenerateCard(player_cards[0]));
                Card.MergeCards(drawable_player_cards, Card.GenerateCard(player_cards[1]));
                Card.DrawCards(drawable_player_cards);
                Console.WriteLine(sum_player);

                // Checking for player's blackjack
                if (sum_player == 21)
                {
                    Console.WriteLine($"Player got Blajckjack!");
                    blackjack_player = true;
                    Thread.Sleep(1500);
                }

                // Main hit/pass loop
                while (sum_player <= 21)
                {
                    // Displaying diffrent message depending on ability to double down
                    if (!blackjack_player && balance >= bet * 2)
                    {
                        Console.WriteLine("What do you do? (hit/pass/double down)");
                        action = Console.ReadLine() ?? "";
                    }
                    else if (!blackjack_player)
                    {
                        Console.WriteLine("What do you do? (hit/pass)");
                        action = Console.ReadLine() ?? "";
                    }

                    // Double down procedure
                    if ((action == "double down" || action == "double" || action == "dd") && balance >= bet * 2)
                    {
                        // Setting double down varrible to true, increasing bet, and subtracing it from balance
                        double_down = true;
                        balance -= bet;
                        bet *= 2;

                        // Clearing console, giving player his card and drawing all hands
                        Console.Clear();

                        player_cards.Add(deck[next_in_deck]);
                        next_in_deck++;
                        Card.MergeCards(drawable_player_cards, Card.GenerateCard(player_cards[next_player]));
                        next_player++;
                        sum_player = Card.SumCards(player_cards);

                        Console.WriteLine($"Bet: {bet}");
                        Console.WriteLine();

                        Console.WriteLine("Dealer's cards:");
                        Card.DrawCards(drawable_dealer_cards);

                        Console.WriteLine();

                        Console.WriteLine("Player's cards:");
                        Card.DrawCards(drawable_player_cards);
                        Console.WriteLine(sum_player);

                        // Checking for exeeding 21 and breaking main hit/pass loop if so
                        if (sum_player > 21)
                        {
                            Console.WriteLine($"You lose {bet}.");
                            break;
                        }
                    }

                    // Hit procedure
                    if (action == "hit" || action == "h")
                    {
                        // Clearing console, giving player his card and drawing all hands
                        Console.Clear();

                        player_cards.Add(deck[next_in_deck]);
                        next_in_deck++;
                        Card.MergeCards(drawable_player_cards, Card.GenerateCard(player_cards[next_player]));
                        next_player++;
                        sum_player = Card.SumCards(player_cards);

                        Console.WriteLine($"Bet: {bet}");
                        Console.WriteLine();

                        Console.WriteLine("Dealer's cards:");
                        Card.DrawCards(drawable_dealer_cards);

                        Console.WriteLine();

                        Console.WriteLine("Player's cards:");
                        Card.DrawCards(drawable_player_cards);
                        Console.WriteLine(sum_player);

                        // Checking for exeeding 21 and breaking main hit/pass loop if so
                        if (sum_player > 21)
                        {
                            Console.WriteLine($"You lose {bet}.");
                            break;
                        }
                    }

                    // Pass, blackjack or double down procedure
                    else if (action == "pass" || action == "p" || blackjack_player || double_down)
                    {
                        // Clearing console and revealing Dealer's second card
                        Thread.Sleep(1000);
                        Console.Clear();
                        drawable_dealer_cards.Clear();
                        for (int i = 0; i < 7; i++) drawable_dealer_cards.Add("");
                        Card.MergeCards(drawable_dealer_cards, Card.GenerateCard(dealer_cards[0]));
                        Card.MergeCards(drawable_dealer_cards, Card.GenerateCard(dealer_cards[1]));
                        sum_dealer = Card.SumCards(dealer_cards);

                        // Drawing all hands
                        Console.WriteLine($"Bet: {bet}");
                        Console.WriteLine();

                        Console.WriteLine("Dealer's cards:");
                        Card.DrawCards(drawable_dealer_cards);
                        Console.WriteLine(sum_dealer);

                        Console.WriteLine();

                        Console.WriteLine("Player's cards:");
                        Card.DrawCards(drawable_player_cards);
                        Console.WriteLine(sum_player);

                        // Dealer's drawing loop
                        while (sum_dealer < 17)
                        {
                            // Sleep for better immersion, and clearing console before adding another card to Dealer's hand and drawing all hands
                            Thread.Sleep(1000);
                            Console.Clear();
                            dealer_cards.Add(deck[next_in_deck]);
                            Card.MergeCards(drawable_dealer_cards, Card.GenerateCard(dealer_cards[next_dealer]));
                            next_in_deck++;
                            next_dealer++;
                            sum_dealer = Card.SumCards(dealer_cards);

                            Console.WriteLine($"Bet: {bet}");
                            Console.WriteLine();

                            Console.WriteLine("Dealer's ards:");
                            Card.DrawCards(drawable_dealer_cards);
                            Console.WriteLine(sum_dealer);

                            Console.WriteLine();

                            Console.WriteLine("Player's cards:");
                            Card.DrawCards(drawable_player_cards);
                            Console.WriteLine(sum_player);
                        }

                        //Checing for Dealer's blackjack
                        if (sum_dealer == 21 && dealer_cards.Count == 2)
                        {
                            Console.WriteLine("Dealer got Blackjack!");
                            blackjack_dealer = true;
                            Thread.Sleep(1500);
                        }

                        // Checking outcome of the game
                        if (blackjack_dealer && blackjack_player)
                        {
                            Console.WriteLine("Push. Bet is returned.");
                            balance += bet;
                            blackjack_dealer = false;
                            blackjack_player = false;
                        }
                        else if (blackjack_dealer && !blackjack_player)
                        {
                            Console.WriteLine($"You lose {bet}.");
                            blackjack_dealer = false;
                        }
                        else if (blackjack_player && !blackjack_dealer)
                        {
                            Console.WriteLine($"You win {bet * 2.5}");
                            balance += bet * 2.5;
                            blackjack_player = false;
                        }

                        else if (sum_dealer > 21 || sum_player > sum_dealer)
                        {
                            Console.WriteLine($"You win {bet * 2}!");
                            balance += bet * 2;
                        }

                        else if (sum_player < sum_dealer) Console.WriteLine($"You lose {bet}.");

                        else
                        {
                            Console.WriteLine("Push. Bet is returned");
                            balance += bet;
                        }

                        Thread.Sleep(1000);
                        double_down = false;
                        break;
                    }

                    // Handling invalid action
                    else
                    {
                        if ((action == "double down" || action == "double" || action == "dd") && balance < bet * 2)
                            Console.WriteLine("You can't afford to double down");
                        else
                            Console.WriteLine("Incorrect action. Type 'hit', 'pass' or 'double down'.");

                        // Clearing console and wrting all hands
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.WriteLine($"Bet: {bet}");
                        Console.WriteLine();
                        Console.WriteLine("Dealer's cards:");
                        Card.DrawCards(drawable_dealer_cards);
                        Console.WriteLine();
                        Console.WriteLine("Player's cards:");
                        Card.DrawCards(drawable_player_cards);
                        Console.WriteLine(sum_player);
                    }
                }
                
                // Checking for possibility of next game
                if (balance > 0)
                {
                    Console.WriteLine("Cards will be dealt again in a moment.");
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("You can't afford to keep playing, You will be escored out.");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}