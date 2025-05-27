namespace Blackjack.Karty
{
    public static class Karta
    {
        // Wzorzec talii kart
        public static List<string> WypelnijWzorzec()
        {
            return new List<string>
                {
                    "♥02", "♥03", "♥04", "♥05", "♥06", "♥07", "♥08", "♥09", "♥10", "♥JP", "♥QN", "♥KG", "♥AS",
                    "♦02", "♦03", "♦04", "♦05", "♦06", "♦07", "♦08", "♦09", "♦10", "♦JP", "♦QN", "♦KG", "♦AS",
                    "♣02", "♣03", "♣04", "♣05", "♣06", "♣07", "♣08", "♣09", "♣10", "♣JP", "♣QN", "♣KG", "♣AS",
                    "♠02", "♠03", "♠04", "♠05", "♠06", "♠07", "♠08", "♠09", "♠10", "♠JP", "♠QN", "♠KG", "♠AS"
                };

        }
        // Rewers ASCII
        public static List<string> RysujRewers()
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

        // Tasowanie talii przy użyciu algorytmu Fisher-Yates
        public static void Tasuj(List<string> a)
        {
            Random rng = new Random();
            int n = a.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (a[i], a[j]) = (a[j], a[i]);
            }
        }

        // Generowanie graficznej reprezentacji jednej karty
        public static List<string> GenerujKarte(string karta)
        {
            char symbol = karta[0];
            List<string> lista_kart = new List<string>();
            string wartosc = "??";
            if (karta[1] == '0')
            {
                wartosc = " " + karta[2];
                lista_kart.Add("┌───────┐");
                lista_kart.Add($"│{wartosc}     │");
                lista_kart.Add("│       │");
                lista_kart.Add($"│   {symbol}   │");
                lista_kart.Add("│       │");
                lista_kart.Add($"│     {wartosc[1]}{wartosc[0]}│");
                lista_kart.Add("└───────┘");
            }

            else
            {
                switch (karta[2])
                {
                    case '0': wartosc = "10"; break;
                    case 'P': wartosc = "JP"; break;
                    case 'N': wartosc = "QN"; break;
                    case 'G': wartosc = "KG"; break;
                    case 'S': wartosc = "AS"; break;
                }
                lista_kart.Add("┌───────┐");
                lista_kart.Add($"│{wartosc}     │");
                lista_kart.Add("│       │");
                lista_kart.Add($"│   {symbol}   │");
                lista_kart.Add("│       │");
                lista_kart.Add($"│     {wartosc}│");
                lista_kart.Add("└───────┘");
            }
            return lista_kart;
        }

        // Doklejenie kolejnej karty do istniejącej grafiki
        public static List<string> PolaczKarty(List<string> glowna, List<string> dodawana)
        {
            for (int i = 0; i < glowna.Count; i++)
            {
                glowna[i] += dodawana[i];
            }
            return glowna;
        }

        // Rysowanie listy kart w konsoli
        public static void RysujKarty(List<string> lista_kart)
        {
            foreach (string linijka in lista_kart)
            {
                Console.WriteLine(linijka);
            }
        }
        
        // Zliczanie wartości punktowej zestawu kart
        public static int SumujKarty(List<string> lista_kart)
        {
            int suma = 0;
            foreach (string karta in lista_kart)
            {
                switch (karta[2])
                {
                    case '2': suma += 2; break;
                    case '3': suma += 3; break;
                    case '4': suma += 4; break;
                    case '5': suma += 5; break;
                    case '6': suma += 6; break;
                    case '7': suma += 7; break;
                    case '8': suma += 8; break;
                    case '9': suma += 9; break;
                    case '0': suma += 10; break;
                    case 'P': suma += 10; break;
                    case 'N': suma += 10; break;
                    case 'G': suma += 10; break;
                }
            }
            foreach (string karta in lista_kart)
            {
                if (karta[2] == 'S')
                {
                    if (suma <= 10) suma += 11;
                    else suma += 1;
                }
            }
            return suma;
        }
    }
}