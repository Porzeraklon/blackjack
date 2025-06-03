namespace Blackjack.BotPlayer
{
    public static class Bot
    {
        // Bot deciding to hit spand or double down based on player's cards, dealer's card and HI-LO count
        public static string Decide(List<string> playerCards, int sumPlayer, bool doubleDownPossible, int trueCountRounded, string dealerCard)
        {
            // Checking for soft hand
            if (playerCards.Count == 2 && (playerCards[0][2] == 'S' || playerCards[1][2] == 'S'))
            {
                // Soft 12, Soft 13 and Soft 14
                if ((sumPlayer == 12 || sumPlayer == 13 || sumPlayer == 14) && (dealerCard[2] != '5' && dealerCard[2] != '6')) return "hit";
                else if ((sumPlayer == 12 || sumPlayer == 13 || sumPlayer == 14) && (dealerCard[2] == '5' || dealerCard[2] == '6'))
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }

                // Soft 15 and Soft 16
                if ((sumPlayer == 15 || sumPlayer == 16) && (dealerCard[2] != '4' && dealerCard[2] != '5' && dealerCard[2] != '6')) return "hit";
                else if ((sumPlayer == 15 || sumPlayer == 16) && (dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6'))
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }

                // Soft 17
                if (sumPlayer == 17 && dealerCard[2] == '2' && trueCountRounded >= 1)
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }
                else if (sumPlayer == 17 && (dealerCard[2] == '3' || dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6'))
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }
                else if (sumPlayer == 17) return "hit";

                // Soft 18
                if (sumPlayer == 18 && (dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6'))
                {
                    if (doubleDownPossible) return "double down";
                    else return "stand";
                }
                else if (sumPlayer == 18 && (dealerCard[2] == '2' || dealerCard[2] == '3' || dealerCard[2] == '7' || dealerCard[2] == '8')) return "stand";
                else if (sumPlayer == 18) return "hit";

                // Soft 19
                if (sumPlayer == 19 && (dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6'))
                {
                    if (doubleDownPossible && trueCountRounded >= 1) return "double down";
                    else return "stand";
                }
                else if (sumPlayer == 19) return "stand";

                // Soft 20 or more
                if (sumPlayer >= 20) return "stand";
            }
            // Player has hard total
            else
            {
                // Hard 7 or less
                if (sumPlayer < 8) return "hit";

                // Hard 8
                if (sumPlayer == 8 && dealerCard[2] != '6') return "hit";
                else if (sumPlayer == 8 && dealerCard[2] == '6')
                {
                    if (trueCountRounded >= 2 && doubleDownPossible) return "double down";
                    else return "hit";
                }

                // Hard 9
                if (sumPlayer == 9 && (dealerCard[2] == '3' || dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6'))
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }
                else if (sumPlayer == 9)
                {
                    if (trueCountRounded >= 1 && dealerCard[2] == '2')
                    {
                        if (doubleDownPossible) return "double down";
                        else return "hit";
                    }
                    else if (trueCountRounded >= 3 && dealerCard[2] == '7')
                    {
                        if (doubleDownPossible) return "double down";
                        else return "hit";
                    }
                    else return "hit";
                }

                // Hard 10
                if (sumPlayer == 10 && (dealerCard[2] == '0' || dealerCard[2] == 'P' || dealerCard[2] == 'N' || dealerCard[2] == 'G' || dealerCard[2] == 'S'))
                {
                    if (trueCountRounded >= 4 && doubleDownPossible) return "double down";
                    else return "hit";
                }
                else if (sumPlayer == 10)
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }

                // Hard 11
                if (sumPlayer == 11 && dealerCard[2] == 'S')
                {
                    if (trueCountRounded >= 1 && doubleDownPossible) return "double down";
                    else return "hit";
                }
                else if (sumPlayer == 11)
                {
                    if (doubleDownPossible) return "double down";
                    else return "hit";
                }

                // Hard 12
                if (sumPlayer == 12 && (dealerCard[2] == '2' || dealerCard[2] == '3'))
                {
                    if (dealerCard[2] == '2' && trueCountRounded >= 3) return "stand";
                    else if (dealerCard[2] == '3' && trueCountRounded >= 2) return "stand";
                    else return "hit";
                }
                else if (sumPlayer == 12 && dealerCard[2] == '4')
                {
                    if (trueCountRounded < 0) return "hit";
                    else return "stand";
                }
                else if (sumPlayer == 12 && (dealerCard[2] == '5' || dealerCard[2] == '6')) return "stand";
                else if (sumPlayer == 12) return "hit";

                // Hard 13
                if (sumPlayer == 13 && dealerCard[2] == '2')
                {
                    if (trueCountRounded <= -1) return "hit";
                    else return "stand";
                }
                else if (sumPlayer == 13 && (dealerCard[2] == '3' || dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6')) return "stand";
                else if (sumPlayer == 13) return "hit";

                // Hard 14
                if (sumPlayer == 14 && (dealerCard[2] == '2' || dealerCard[2] == '3' || dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6')) return "stand";
                else if (sumPlayer == 14) return "hit";

                // Hard 15
                if (sumPlayer == 15 && (dealerCard[2] == '2' || dealerCard[2] == '3' || dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6')) return "stand";
                else if (sumPlayer == 15 && (dealerCard[2] == '0' || dealerCard[2] == 'P' || dealerCard[2] == 'N' || dealerCard[2] == 'G'))
                {
                    if (trueCountRounded >= 4) return "stand";
                    else return "hit";
                }
                else if (sumPlayer == 15) return "hit";

                // Hard 16
                if (sumPlayer == 16 && (dealerCard[2] == '2' || dealerCard[2] == '3' || dealerCard[2] == '4' || dealerCard[2] == '5' || dealerCard[2] == '6')) return "stand";
                else if (sumPlayer == 16 && (dealerCard[2] == '0' || dealerCard[2] == 'P' || dealerCard[2] == 'N' || dealerCard[2] == 'G'))
                {
                    if (trueCountRounded >= 0) return "stand";
                    else return "hit";
                }
                else if (sumPlayer == 16 && dealerCard[2] == '9')
                {
                    if (trueCountRounded >= 4) return "stand";
                    else return "hit";
                }
                else if (sumPlayer == 16) return "hit";

                // Hard 17 or more
                if (sumPlayer >= 17) return "stand";
            }
            return "error";
        }

        public static string Bet(double balance, int trueCountRounded)
        {
            switch (trueCountRounded)
            {
                case >= 5: return (0.18 * balance).ToString("F0");
                case >= 4: return (0.14 * balance).ToString("F0");
                case >= 3: return (0.10 * balance).ToString("F0");
                case >= 2: return (0.06 * balance).ToString("F0");
                case >= 1: return (0.03 * balance).ToString("F0");
                case <= 0: return (0.01 * balance).ToString("F0");
            }
            ;
        }
    }
}
