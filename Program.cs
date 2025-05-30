// BlackJack CLI
// Copyright (C) 2025 Porzeraklon
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

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

            // Creating setting variables
            bool showSums = true; // option to show sums of hands
            bool showHiLo = true; // option to show card counting tracker

            // Checking for user settings
            Card.gameSettings(args, ref showSums, ref showHiLo);

            // Displaying title card
            Card.TitleCard();
            Console.ReadKey();

            // Creating starting balance
            double balance = 1000;

            // Creating deck pattern (4 suits 13 cards each)
            List<string> deckPattern = Card.FillDeckPattern();
            

            // Generating and shuffling shoe
            int numOfDecks = 6;
            List<string> deck = new List<string>();
            for (int i = 0; i < numOfDecks; i++)
                deck.AddRange(deckPattern);
            Card.Shuffle(deck);

            // Generating image of card back
            List<string> cardBack = Card.DrawCardBack();
            
            // Creating lists for cards and their graphic equivalents
            List<string> playerCards = new List<string>();
            List<string> dealerCards = new List<string>();
            List<string> drawablePlayer = new List<string>();
            List<string> drawableDealer = new List<string>();

            // Creating variables tracing deck state
            int nextInDeck = 0, nextPlayer = 0, nextDealer = 0, cardsDealt = 0, runningCount = 0, trueCountRounded = 0;
            double trueCount = 0;

            // Creating variables tracing blackjacks and double down
            bool blackjackPlayer = false, blackjackDealer = false, doubleDown = false, doubleDownPossible = false;
            string action = "";

            // Main game loop
            while (balance > 0)
            {
                // Clearing console for better immersion
                Console.Clear();

                // Checking for deck penetration and shuffling
                if (nextInDeck >= (int)(0.75 * numOfDecks * 52))
                {
                    Console.WriteLine("Shuffling cards please wait.");
                    Thread.Sleep(3000);
                    Card.Shuffle(deck);
                    nextInDeck = 0;
                    cardsDealt = 0;
                    runningCount = 0;
                    trueCount = 0;
                    Console.WriteLine("Cards have been shuffled, game will continue");
                    Thread.Sleep(2000);
                    Console.Clear();
                }

                // Creating variable for player's bet
                double bet = 0;

                // Loop for placing bet
                while (true)
                {
                    Console.WriteLine($"Balance: {balance} $");
                    Console.WriteLine("To exit game press Ctrl+C");
                    Console.Write("Place your bet: ");

                    // Reading player's bet
                    string input = Console.ReadLine() ?? "";


                    // Checking for input errors
                    if (!double.TryParse(input, out bet))
                    {
                        Console.WriteLine("It's not a valid number, try again.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    if (bet <= 0)
                    {
                        Console.WriteLine("Bet must be greater than zero. Please enter the valid amount");
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

                    // If bet is correct, subtracting it from balance and breaking loop
                    balance -= bet;
                    break;
                }

                // Resetting all lists
                dealerCards.Clear();
                playerCards.Clear();
                drawableDealer.Clear();
                drawablePlayer.Clear();

                for (int i = 0; i < 7; i++)
                {
                    drawableDealer.Add("");
                    drawablePlayer.Add("");
                }

                // Dealing starting cards
                int sumPlayer = 0, sumDealer = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0) playerCards.Add(deck[nextInDeck]);
                    if (i == 1) dealerCards.Add(deck[nextInDeck + i]);
                    if (i == 2) playerCards.Add(deck[nextInDeck + i]);
                    if (i == 3) dealerCards.Add(deck[nextInDeck + i]);
                }

                // Setting tracking variables correctly
                nextInDeck += 4;
                nextPlayer = 2;
                nextDealer = 2;
                bool dealerHidden = true;
                cardsDealt += 4;
                if ((bet * 2) / 2 >= balance) doubleDownPossible = true;

                // Changing running count
                runningCount = Card.CountPlayerCards(runningCount, playerCards, nextPlayer);
                runningCount = Card.CountDealerCards(runningCount, dealerCards, dealerHidden, nextDealer);

                // Changing true count
                trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);
                trueCountRounded = (int)trueCount;

                // Displaying starting cards, and calculating value of hands
                Console.Clear();

                if (showHiLo)
                {
                    Console.WriteLine($"Current Hi-Lo true count: {trueCountRounded}");
                    Console.WriteLine($"Current Hi-Lo running count: {runningCount}");
                    Console.WriteLine();
                }

                Console.WriteLine($"Bet: {bet}");
                Console.WriteLine();

                sumDealer = Card.SumCards(dealerCards);
                Console.WriteLine("Dealer's cards:");
                Card.MergeCards(drawableDealer, Card.GenerateCard(dealerCards[0]));
                Card.MergeCards(drawableDealer, cardBack);
                Card.DrawCards(drawableDealer);

                Console.WriteLine();

                sumPlayer = Card.SumCards(playerCards);
                Console.WriteLine("Player's cards:");
                Card.MergeCards(drawablePlayer, Card.GenerateCard(playerCards[0]));
                Card.MergeCards(drawablePlayer, Card.GenerateCard(playerCards[1]));
                Card.DrawCards(drawablePlayer);
                if (showSums) Console.WriteLine(sumPlayer);


                // Checking for player's blackjack
                if (sumPlayer == 21)
                {
                    Console.WriteLine($"Player got Blackjack!");
                    blackjackPlayer = true;
                    Thread.Sleep(1500);
                }

                // Main hit/stand loop
                while (sumPlayer <= 21)
                {
                    // Displaying different message depending on ability to double down
                    if (!blackjackPlayer && doubleDownPossible)
                    {
                        Console.WriteLine("What do you do? (hit / stand / double down)");
                        action = Console.ReadLine() ?? "";
                    }
                    else if (!blackjackPlayer)
                    {
                        Console.WriteLine("What do you do? (hit / stand)");
                        action = Console.ReadLine() ?? "";
                    }

                    // Double down procedure
                    if ((action == "double down" || action == "double" || action == "dd") && doubleDownPossible)
                    {
                        // Setting double down variable to true, increasing bet, and subtracting it from balance
                        doubleDown = true;
                        balance -= bet;
                        bet *= 2;

                        // Clearing console, giving player his card and drawing all hands
                        Console.Clear();

                        playerCards.Add(deck[nextInDeck]);
                        nextInDeck++;
                        Card.MergeCards(drawablePlayer, Card.GenerateCard(playerCards[nextPlayer]));
                        nextPlayer++;
                        sumPlayer = Card.SumCards(playerCards);

                        runningCount = Card.CountPlayerCards(runningCount, playerCards, nextPlayer);
                        trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);

                        Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);

                        // Checking for exceeding 21 revealing Dealer's second card and breaking main hit/stand loop
                        if (sumPlayer > 21)
                        {
                            Card.RevealDealer(dealerHidden, drawableDealer, dealerCards, sumDealer);
                            dealerHidden = false;

                            runningCount = Card.CountDealerCards(runningCount, dealerCards, dealerHidden, nextDealer);
                            trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);

                            Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);
                            Console.WriteLine($"You lose {bet}.");
                            break;
                        }
                    }

                    // Hit procedure
                    if (action == "hit" || action == "h")
                    {
                        // Clearing console, setting double down possibility to false and giving player his card and drawing all hands
                        Console.Clear();
                        doubleDownPossible = false;

                        playerCards.Add(deck[nextInDeck]);
                        nextInDeck++;
                        Card.MergeCards(drawablePlayer, Card.GenerateCard(playerCards[nextPlayer]));
                        nextPlayer++;
                        sumPlayer = Card.SumCards(playerCards);

                        runningCount = Card.CountPlayerCards(runningCount, playerCards, nextPlayer);
                        trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);

                        Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);

                        // Checking for exceeding 21 revealing Dealer's second card and breaking main hit/stand loop
                        if (sumPlayer > 21)
                        {
                            Card.RevealDealer(dealerHidden, drawableDealer, dealerCards, sumDealer);
                            dealerHidden = false;

                            runningCount = Card.CountDealerCards(runningCount, dealerCards, dealerHidden, nextDealer);
                            trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);

                            Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);
                            Console.WriteLine($"You lose {bet}.");
                            break;
                        }
                    }

                    // Stand, blackjack or double down procedure
                    else if (action == "stand" || action == "s" || blackjackPlayer || doubleDown)
                    {
                        // Clearing console and revealing Dealer's second card
                        Card.RevealDealer(dealerHidden, drawableDealer, dealerCards, sumDealer);
                        dealerHidden = false;
                        
                        // Counting cards and updating true count
                        runningCount = Card.CountDealerCards(runningCount, dealerCards, dealerHidden, nextDealer);
                        trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);

                        // Drawing all hands
                        Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);

                        // Dealer's drawing loop
                        while (sumDealer < 17)
                        {
                            // Sleep for better immersion, and clearing console before adding another card to Dealer's hand and drawing all hands
                            Thread.Sleep(1000);
                            Console.Clear();
                            dealerCards.Add(deck[nextInDeck]);
                            Card.MergeCards(drawableDealer, Card.GenerateCard(dealerCards[nextDealer]));
                            nextInDeck++;
                            nextDealer++;
                            sumDealer = Card.SumCards(dealerCards);

                            runningCount = Card.CountDealerCards(runningCount, dealerCards, dealerHidden, nextDealer);
                            trueCount = runningCount / ((numOfDecks * 52 - cardsDealt) / 52.0);

                            Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);
                        }

                        //Checking for Dealer's blackjack
                        if (sumDealer == 21 && dealerCards.Count == 2)
                        {
                            Console.WriteLine("Dealer got Blackjack!");
                            blackjackDealer = true;
                            Thread.Sleep(1500);
                        }

                        // Checking outcome of the game
                        if (blackjackDealer && blackjackPlayer)
                        {
                            Console.WriteLine("Push. Bet is returned.");
                            balance += bet;
                            blackjackDealer = false;
                            blackjackPlayer = false;
                        }
                        else if (blackjackDealer && !blackjackPlayer)
                        {
                            Console.WriteLine($"You lose {bet}.");
                            blackjackDealer = false;
                        }
                        else if (blackjackPlayer && !blackjackDealer)
                        {
                            Console.WriteLine($"You win {bet * 2.5}");
                            balance += bet * 2.5;
                            blackjackPlayer = false;
                        }

                        else if (sumDealer > 21 || sumPlayer > sumDealer)
                        {
                            Console.WriteLine($"You win {bet * 2}!");
                            balance += bet * 2;
                        }

                        else if
                            (sumPlayer < sumDealer) Console.WriteLine($"You lose {bet}.");

                        else
                        {
                            Console.WriteLine("Push. Bet is returned");
                            balance += bet;
                        }

                        Thread.Sleep(1000);
                        doubleDown = false;
                        break;
                    }

                    // Handling invalid action
                    else
                    {
                        if ((action == "double down" || action == "double" || action == "dd") && balance < bet * 2)
                            Console.WriteLine("You can't afford to double down.");
                        else
                            Console.WriteLine("Incorrect action. Type 'hit', 'stand', or 'double down'.");

                        // Clearing console and writing all hands
                        Thread.Sleep(1000);
                        Console.Clear();
                        Card.DrawHands(bet, drawableDealer, drawablePlayer, sumDealer, sumPlayer, dealerHidden, showSums, showHiLo, trueCountRounded, runningCount);
                    }
                }

                // Checking whether the player can continue
                if (balance > 0)
                {
                    Console.WriteLine("Cards will be dealt again in a moment.");
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("You can't afford to keep playing. You will be escorted out.");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}