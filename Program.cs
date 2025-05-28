using System.ComponentModel;
using Blackjack.Karty;
namespace Blackjack
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Karta.TitleCard();
            Console.ReadKey();

            double saldo = 1000;
            if (args.Length > 0) double.TryParse(args[0], out saldo);

            // Tworzenie wzorca talii (cztery kolory po 13 kart)
            List<string> wzorzec = Karta.WypelnijWzorzec();
            

            // Generowanie oraz tasowanie shoe (domyślnie 6 talii)
            int ilosc_talii = 6;
            if (args.Length > 1) int.TryParse(args[1], out ilosc_talii);
            List<string> talia = new List<string>();
            for (int i = 0; i < ilosc_talii; i++) talia.AddRange(wzorzec);
            Karta.Tasuj(talia);

            // Graficzna reprezentacja zakrytej karty
            List<string> rewers = Karta.RysujRewers();
            
            // Listy na karty i ich graficzne odpowiedniki
            List<string> karty_gracz = new List<string>();
            List<string> karty_dealer = new List<string>();
            List<string> rysowane_karty_gracz = new List<string>();
            List<string> rysowane_karty_dealer = new List<string>();

            // Zmienne śledzące stan talii
            int nastepna_w_talii = 0, nastepna_gracza = 0, nastepna_dealer = 0;

            //Zmienne śledzące blackjack oraz double down
            bool blackjack_gracz =false, blackjack_dealer = false, double_down = false;
            string akcja = "";


            while (saldo > 0)
            {
                Console.Clear();
                double stawka = 0;
                while (true)
                {
                    Console.WriteLine($"Twoje saldo: {saldo} zł");
                    Console.WriteLine("Aby wyjdz napisz naciśnij Ctrl + C");
                    Console.Write("Aby grać podaj stawkę: ");
                    string input = Console.ReadLine() ?? "";

                    if (!double.TryParse(input, out stawka))
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

                // Resetowanie wszystkich list
                karty_dealer.Clear();
                karty_gracz.Clear();
                rysowane_karty_dealer.Clear();
                rysowane_karty_gracz.Clear();

                for (int i = 0; i < 7; i++)
                {
                    rysowane_karty_dealer.Add("");
                    rysowane_karty_gracz.Add("");
                }

                // Sprawdzenie stanu zużycia talii, oraz przetasowanie
                if (nastepna_w_talii >= (int)(0.75 * ilosc_talii * 52)) Karta.Tasuj(talia);

                // Rozdanie początkowych kart
                int suma_gracz = 0, suma_dealer = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0) karty_gracz.Add(talia[nastepna_w_talii]);
                    if (i == 1) karty_dealer.Add(talia[nastepna_w_talii + i]);
                    if (i == 2) karty_gracz.Add(talia[nastepna_w_talii + i]);
                    if (i == 3) karty_dealer.Add(talia[nastepna_w_talii + i]);
                }

                nastepna_w_talii += 4;
                nastepna_gracza = 2;
                nastepna_dealer = 2;

                // Wyświetlenie kart na początku gry
                Console.Clear();
                Console.WriteLine($"Stawka: {stawka}");
                Console.WriteLine();

                suma_dealer = Karta.SumujKarty(karty_dealer);
                Console.WriteLine("Karty dealer:");
                Karta.PolaczKarty(rysowane_karty_dealer, Karta.GenerujKarte(karty_dealer[0]));
                Karta.PolaczKarty(rysowane_karty_dealer, rewers);
                Karta.RysujKarty(rysowane_karty_dealer);

                Console.WriteLine();

                suma_gracz = Karta.SumujKarty(karty_gracz);
                Console.WriteLine("Karty gracz:");
                Karta.PolaczKarty(rysowane_karty_gracz, Karta.GenerujKarte(karty_gracz[0]));
                Karta.PolaczKarty(rysowane_karty_gracz, Karta.GenerujKarte(karty_gracz[1]));
                Karta.RysujKarty(rysowane_karty_gracz);
                Console.WriteLine(suma_gracz);

                // Rozpatrywanie blackjack gracza
                if (suma_gracz == 21)
                {
                    Console.WriteLine($"Blajckjack gracza!");
                    blackjack_gracz = true;
                    Thread.Sleep(1500);
                }

                // Główna pętla gry gracza
                while (suma_gracz <= 21)
                {
                    if (!blackjack_gracz && saldo >= stawka * 2)
                    {
                        Console.WriteLine("Co robisz? (hit/pass/double down)");
                        akcja = Console.ReadLine() ?? "";
                    }
                    else if (!blackjack_gracz)
                    {
                        Console.WriteLine("Co robisz? (hit/pass)");
                        akcja = Console.ReadLine() ?? "";
                    }

                    if ((akcja == "double down" || akcja == "double" || akcja == "dd") && saldo >= stawka * 2)
                    {
                        double_down = true;
                        saldo -= stawka;
                        stawka *= 2;
                        Console.Clear();

                        karty_gracz.Add(talia[nastepna_w_talii]);
                        nastepna_w_talii++;
                        Karta.PolaczKarty(rysowane_karty_gracz, Karta.GenerujKarte(karty_gracz[nastepna_gracza]));
                        nastepna_gracza++;
                        suma_gracz = Karta.SumujKarty(karty_gracz);

                        Console.WriteLine($"Stawka: {stawka}");
                        Console.WriteLine();

                        Console.WriteLine("Karty dealer:");
                        Karta.RysujKarty(rysowane_karty_dealer);

                        Console.WriteLine();

                        Console.WriteLine("Karty gracz:");
                        Karta.RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);

                        if (suma_gracz > 21)
                        {
                            Console.WriteLine($"Przegrywasz {stawka}.");
                            break;
                        }
                    }

                    if (akcja == "hit" || akcja == "h")
                    {
                        Console.Clear();

                        karty_gracz.Add(talia[nastepna_w_talii]);
                        nastepna_w_talii++;
                        Karta.PolaczKarty(rysowane_karty_gracz, Karta.GenerujKarte(karty_gracz[nastepna_gracza]));
                        nastepna_gracza++;
                        suma_gracz = Karta.SumujKarty(karty_gracz);

                        Console.WriteLine($"Stawka: {stawka}");
                        Console.WriteLine();

                        Console.WriteLine("Karty dealer:");
                        Karta.RysujKarty(rysowane_karty_dealer);

                        Console.WriteLine();

                        Console.WriteLine("Karty gracz:");
                        Karta.RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);

                        if (suma_gracz > 21)
                        {
                            Console.WriteLine($"Przegrywasz {stawka}.");
                            break;
                        }
                    }

                    else if (akcja == "pass" || akcja == "p" || blackjack_gracz || double_down)
                    {
                        // Odsłonięcie zakrytej karty Dealera
                        Thread.Sleep(1000);
                        Console.Clear();
                        rysowane_karty_dealer.Clear();
                        for (int i = 0; i < 7; i++) rysowane_karty_dealer.Add("");
                        Karta.PolaczKarty(rysowane_karty_dealer, Karta.GenerujKarte(karty_dealer[0]));
                        Karta.PolaczKarty(rysowane_karty_dealer, Karta.GenerujKarte(karty_dealer[1]));
                        suma_dealer = Karta.SumujKarty(karty_dealer);

                        Console.WriteLine($"Stawka: {stawka}");
                        Console.WriteLine();

                        Console.WriteLine("Karty dealer:");
                        Karta.RysujKarty(rysowane_karty_dealer);
                        Console.WriteLine(suma_dealer);

                        Console.WriteLine();

                        Console.WriteLine("Karty gracz:");
                        Karta.RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);

                        // Dobieranie kart przez Dealera jeżeli ma < 17
                        while (suma_dealer < 17)
                        {
                            Thread.Sleep(1000);
                            Console.Clear();
                            karty_dealer.Add(talia[nastepna_w_talii]);
                            Karta.PolaczKarty(rysowane_karty_dealer, Karta.GenerujKarte(karty_dealer[nastepna_dealer]));
                            nastepna_w_talii++;
                            nastepna_dealer++;
                            suma_dealer = Karta.SumujKarty(karty_dealer);

                            Console.WriteLine($"Stawka: {stawka}");
                            Console.WriteLine();

                            Console.WriteLine("Karty dealer:");
                            Karta.RysujKarty(rysowane_karty_dealer);
                            Console.WriteLine(suma_dealer);

                            Console.WriteLine();

                            Console.WriteLine("Karty gracz:");
                            Karta.RysujKarty(rysowane_karty_gracz);
                            Console.WriteLine(suma_gracz);
                        }

                        // Rozstrzygnięcie gry
                        if (suma_dealer == 21 && karty_dealer.Count == 2)
                        {
                            Console.WriteLine("Blackjack Dealer!");
                            blackjack_dealer = true;
                            Thread.Sleep(1500);
                        }

                        if (blackjack_dealer && blackjack_gracz)
                        {
                            Console.WriteLine("Remis – push. Stawka zostaje zwrócona.");
                            saldo += stawka;
                            blackjack_dealer = false;
                            blackjack_gracz = false;
                        }
                        else if (blackjack_dealer && !blackjack_gracz)
                        {
                            Console.WriteLine($"Przegrywasz {stawka}.");
                            blackjack_dealer = false;
                        }
                        else if (blackjack_gracz && !blackjack_dealer)
                        {
                            Console.WriteLine($"Wygrywasz {stawka * 2.5}");
                            saldo += stawka * 2.5;
                            blackjack_gracz = false;
                        }

                        else if (suma_dealer > 21 || suma_gracz > suma_dealer)
                        {
                            Console.WriteLine($"Wygrywasz {stawka * 2}!");
                            saldo += stawka * 2;
                        }

                        else if (suma_gracz < suma_dealer) Console.WriteLine($"Przegrywasz {stawka}.");

                        else
                        {
                            Console.WriteLine("Remis – push. Stawka zostaje zwrócona.");
                            saldo += stawka;
                        }

                        Thread.Sleep(1000);
                        double_down = false;
                        break;
                    }

                    else
                    {
                        if ((akcja == "double down" || akcja == "double" || akcja == "dd") && saldo < stawka * 2)
                            Console.WriteLine("Nie stać cię na double down!");
                        else 
                            Console.WriteLine("Nie rozpoznano polecenia. Wpisz 'hit', 'pass' lub 'double down'.");
                        // Obsługa niepoprawnej komendy
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.WriteLine($"Stawka: {stawka}");
                        Console.WriteLine();
                        Console.WriteLine("Karty dealer:");
                        Karta.RysujKarty(rysowane_karty_dealer);
                        Console.WriteLine();
                        Console.WriteLine("Karty gracz:");
                        Karta.RysujKarty(rysowane_karty_gracz);
                        Console.WriteLine(suma_gracz);
                    }
                }
                
                // Sprawdzenie możliwości daleszej gry
                if (saldo > 0)
                {
                    Console.WriteLine("Za chwilę karty zostaną rozdane ponownie.");
                    Thread.Sleep(2000);
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