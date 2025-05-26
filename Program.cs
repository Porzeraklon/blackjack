using System.ComponentModel;

namespace Blackjack
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int saldo = 1000;
            if (args.Length == 1) int.TryParse(args[0], out saldo);
            while (saldo > 0)
            {
                Console.Clear();
                int stawka = 0;
                while (true)
                {
                    Console.WriteLine($"Twoje saldo: {saldo} zł");
                    Console.Write("Podaj stawkę (tylko liczby całkowite): ");
                    string input = Console.ReadLine() ?? "";

                    if (!int.TryParse(input, out stawka))
                    {
                        Console.WriteLine("To nie jest poprawna liczba. Spróbuj ponownie.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    if (stawka <= 0)
                    {
                        Console.WriteLine("Stawka musi być większa od zera.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    if (stawka > saldo)
                    {
                        Console.WriteLine("Nie masz wystarczającego salda na taką stawkę.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }
                    saldo -= stawka;
                    break; // Stawka poprawna
                }

                // Talia 52 kart (cztery kolory po 13 kart)
                string[] talia =
                {
                    "♥02", "♥03", "♥04", "♥05", "♥06", "♥07", "♥08", "♥09", "♥10", "♥JP", "♥QN", "♥KG", "♥AS",
                    "♦02", "♦03", "♦04", "♦05", "♦06", "♦07", "♦08", "♦09", "♦10", "♦JP", "♦QN", "♦KG", "♦AS",
                    "♣02", "♣03", "♣04", "♣05", "♣06", "♣07", "♣08", "♣09", "♣10", "♣JP", "♣QN", "♣KG", "♣AS",
                    "♠02", "♠03", "♠04", "♠05", "♠06", "♠07", "♠08", "♠09", "♠10", "♠JP", "♠QN", "♠KG", "♠AS"
                };

                // Graficzna reprezentacja zakrytej karty
                List<string> rewers = new List<string>
                {
                    "┌───────┐",
                    "│╬╬╬╬╬╬╬│",
                    "│╬░░░░░╬│",
                    "│╬░░░░░╬│",
                    "│╬░░░░░╬│",
                    "│╬╬╬╬╬╬╬│",
                    "└───────┘"
                };

                // Listy na karty i ich graficzne odpowiedniki
                List<string> karty_gracz = new List<string>();
                List<string> karty_dealer = new List<string>();
                List<string> rysowane_karty_gracz = new List<string>();
                List<string> rysowane_karty_dealer = new List<string>();

                for (int i = 0; i < 7; i++)
                {
                    rysowane_karty_dealer.Add("");
                    rysowane_karty_gracz.Add("");
                }

                Tasuj(talia);

                // Rozdanie początkowych kart
                int suma_gracz = 0, suma_dealer = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i < 2) karty_dealer.Add(talia[i]);
                    if (i == 2) karty_gracz.Add(talia[i]);
                }
                int nastepna_w_talii = 3, nastepna_gracza = 1, nastepna_dealer = 2;

                // Wyświetlenie kart na początku gry
                Console.WriteLine("Karty dealer:");
                PolaczKarty(rysowane_karty_dealer, GenerujKarte(karty_dealer[0]));
                PolaczKarty(rysowane_karty_dealer, rewers);
                RysujKarty(rysowane_karty_dealer);
                Console.WriteLine();

                Console.WriteLine("Karty gracz:");
                PolaczKarty(rysowane_karty_gracz, GenerujKarte(karty_gracz[0]));
                RysujKarty(rysowane_karty_gracz);

                // Główna pętla gry gracza
                while (suma_gracz <= 21)
                {
                    Console.WriteLine("Co robisz? (hit/pass)");
                    string akcja = Console.ReadLine() ?? "";

                    if (akcja == "hit" || akcja == "h")
                    {
                        Console.Clear();
                        karty_gracz.Add(talia[nastepna_w_talii]);
                        nastepna_w_talii++;
                        PolaczKarty(rysowane_karty_gracz, GenerujKarte(karty_gracz[nastepna_gracza]));
                        nastepna_gracza++;
                        suma_gracz = SumujKarty(karty_gracz);

                        Console.WriteLine("Karty dealer:");
                        RysujKarty(rysowane_karty_dealer);
                        Console.WriteLine();

                        Console.WriteLine("Karty gracz:");
                        RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);

                        if (suma_gracz > 21)
                        {
                            Console.WriteLine($"Przegrywasz {stawka}.");
                            break;
                        }
                    }
                    else if (akcja == "pass" || akcja == "p")
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                        rysowane_karty_dealer.Clear();
                        for (int i = 0; i < 7; i++) rysowane_karty_dealer.Add("");
                        PolaczKarty(rysowane_karty_dealer, GenerujKarte(karty_dealer[0]));
                        PolaczKarty(rysowane_karty_dealer, GenerujKarte(karty_dealer[1]));
                        suma_dealer = SumujKarty(karty_dealer);
                        Console.WriteLine("Karty dealer:");
                        RysujKarty(rysowane_karty_dealer);
                        Console.WriteLine(suma_dealer);

                        Console.WriteLine();

                        Console.WriteLine("Karty gracz:");
                        RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);


                        // Dealer dobiera karty jeśli ma mniej niż 17 punktów
                        while (suma_dealer < 17)
                        {
                            Thread.Sleep(1000);
                            Console.Clear();
                            karty_dealer.Add(talia[nastepna_w_talii]);
                            PolaczKarty(rysowane_karty_dealer, GenerujKarte(karty_dealer[nastepna_dealer]));
                            nastepna_w_talii++;
                            nastepna_dealer++;
                            suma_dealer = SumujKarty(karty_dealer);

                            Console.WriteLine("Karty dealer:");
                            RysujKarty(rysowane_karty_dealer);
                            Console.WriteLine(suma_dealer);

                            Console.WriteLine();

                            Console.WriteLine("Karty gracz:");
                            RysujKarty(rysowane_karty_gracz);
                            Console.WriteLine(suma_gracz);
                        }



                        // Rozstrzygnięcie gry
                        if (suma_dealer > 21 || suma_gracz > suma_dealer)
                        {
                            Console.WriteLine($"Wygrywasz {stawka * 2}!");
                            saldo += stawka * 2;
                        }
                        else if (suma_gracz < suma_dealer)
                            Console.WriteLine($"Przegrywasz {stawka}.");
                        else
                        {
                            Console.WriteLine("Remis – push.");
                            saldo += stawka;
                        }
                        break;
                    }
                    else
                    {
                        // Obsługa niepoprawnej komendy
                        Console.WriteLine("Nie rozpoznano polecenia. Wpisz 'hit' albo 'pass'.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.WriteLine("Karty dealer:");
                        RysujKarty(rysowane_karty_dealer);
                        Console.WriteLine();
                        Console.WriteLine("Karty gracz:");
                        RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);
                    }
                }

                // Tasowanie talii przy użyciu algorytmu Fisher-Yates
                static void Tasuj(string[] a)
                {
                    Random rng = new Random();
                    int n = a.Length;
                    for (int i = n - 1; i > 0; i--)
                    {
                        int j = rng.Next(i + 1);
                        (a[i], a[j]) = (a[j], a[i]);
                    }
                }

                // Generuje graficzną reprezentację jednej karty
                static List<string> GenerujKarte(string karta)
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

                // Dokleja kolejną kartę do istniejącej grafiki
                static List<string> PolaczKarty(List<string> glowna, List<string> dodawana)
                {
                    for (int i = 0; i < glowna.Count; i++)
                    {
                        glowna[i] += dodawana[i];
                    }
                    return glowna;
                }

                // Rysuje listę kart w konsoli
                static void RysujKarty(List<string> lista_kart)
                {
                    foreach (string linijka in lista_kart)
                    {
                        Console.WriteLine(linijka);
                    }
                }

                // Zlicza wartość punktową zestawu kart
                static int SumujKarty(List<string> lista_kart)
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
                if (saldo > 0)
                {
                    Console.WriteLine("Rozdać karty jeszcze raz? (tak/nie)");
                    string kontynuacja = Console.ReadLine() ?? "";
                    if (kontynuacja != "tak") break;
                    Thread.Sleep(500);
                }
                else
                {
                    Console.WriteLine("Nie stać cię na dalszą grę, proszę opuścić lokal.");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}