using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;






namespace MoC2
{
    class Program
    {
        static void TextProcessing()
        {
            string RawText = File.ReadAllText("raw_text.txt", Encoding.UTF8);
            string Text = RawText.ToLower();
            Text = Text.Replace("\n", ""); Text = Text.Replace("\r", "");
            Text = Text.Replace("ґ", "г"); Text = Text.Replace(" ", "");
            Text = String.Join("", Text.Split('/', '.', ',', '-', '(', ')', '!', '?', '`', '\'', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'));
            //Console.WriteLine(Text);
            if (Text.Length % 2 == 1)
                Text = Text.Remove(Text.Length - 1, 1);
            File.WriteAllText("pr_text.txt", Text);
        }

     

        static double[] MonogramsFrequency(string Text, char[] alphabet)
        {
            int m = alphabet.Length;
            double[] frq = new double[m];
            for (int t = 0; t < Text.Length; t++)
            {
                for (int i = 0; i < m; i++)
                {
                    if (Text[t] == alphabet[i])
                    {
                        frq[i]++;
                    }
                }
            }
            for (int i = 0; i < m; i ++)
            {
                frq[i] = frq[i] / (double)Text.Length;
                //Console.WriteLine(alphabet[i] + " :  " + frq[i]);
            }
            return frq;
        }

     
        static double[] BigramsFrequency(string Text, char[] alphabet)
        {
            int m = alphabet.Length;
            double[] frq = new double[m * m];
            for (int t = 0; t < Text.Length; t += 2)
            {
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if ((Text[t] == alphabet[i]) && (Text[t + 1] == alphabet[j]))
                        {
                            frq[m * i + j]++;
                        }
                    }
                }
            }
            for (int i = 0; i < frq.Length; i++)
            {
                frq[i] = (frq[i] * 2) / (double)Text.Length;
                //Console.WriteLine(alphabet[i/32] + " " +  alphabet[i%32] + " :  " + frq[i]);
            }
            return frq;
        }


        static int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            return a | b;
        }




        static string Dist1(string PlainText, char[] alphabet, int r, int l)  // Віженер
        {
            int m = alphabet.Length;
            string ResultString = "";
            int TempNumber = 0;
            int[] key = new int[r];
            Random rng = new Random();

            switch (l)
            {
                case 1:
                    for (int i = 0; i < r; i++)
                        key[i] = rng.Next(m);
                    for (int t = 0; t < PlainText.Length; t++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            if (PlainText[t] == alphabet[i])
                            {
                                TempNumber = i;
                                break;
                            }
                        }
                        TempNumber += key[t % r];
                        TempNumber = TempNumber % m;
                        ResultString += alphabet[TempNumber];
                    }
                    return ResultString;

                case 2:
                    for (int i = 0; i < r; i++)
                        key[i] = rng.Next(m * m);
                    for (int t = 0; t < PlainText.Length; t+= 2)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            for (int j = 0; j < m; j++)
                            {
                                if ((PlainText[t] == alphabet[i]) && (PlainText[t + 1] == alphabet[j]))
                                {
                                    TempNumber = m * i + j;
                                    break;
                                }
                            }
                            break;
                        }
                        TempNumber += key[t % r];
                        TempNumber = TempNumber % (m * m);
                        ResultString += alphabet[TempNumber / m];
                        ResultString += alphabet[TempNumber % m];
                    }
                    return ResultString;

                default:
                    return "Error";
            }           
        }

        static string Dist2(string PlainText, char[] alphabet, int l)  // афінна підстановка
        {
            int m = alphabet.Length;
            string ResultString = "";
            int TempNumber = 0;
            Random rng = new Random();
            int a, b;

            switch(l)
            {
                case 1:
                    b = rng.Next(m);
                    do
                        a = rng.Next(m);
                    while (GCD(a, m) != 1);
                    for (int t = 0; t < PlainText.Length; t++)
                    {
                        for (int i = 0; i < m; i++)
                            if (PlainText[t] == alphabet[i])
                            {
                                TempNumber = i;
                                break;
                            }
                        TempNumber = a * TempNumber + b;
                        TempNumber = TempNumber % m;
                        ResultString += alphabet[TempNumber];
                    }
                    return ResultString;

                case 2:
                    b = rng.Next(m * m);
                    do
                        a = rng.Next(m * m);
                    while (GCD(a, (m * m)) != 1);
                    for (int t = 0; t < PlainText.Length; t+= 2)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            for (int j = 0; j < m; j++)
                            {
                                if ((PlainText[t] == alphabet[i]) && (PlainText[t + 1] == alphabet[j]))
                                {
                                    TempNumber = m * i + j;
                                    break;
                                }
                            }
                            break;
                        }
                        TempNumber = a * TempNumber + b;
                        TempNumber = TempNumber % (m * m);
                        ResultString += alphabet[TempNumber / m];
                        ResultString += alphabet[TempNumber % m];
                    }
                    return ResultString;

                default:
                    return "Error";

            }
        }

        static string Dist3(string PlainText, char[] alphabet, int l)  // рівномірно розподілена послідовність
        {
            int m = alphabet.Length;
            string ResultString = "";
            Random rng = new Random();
            int TempNumber;

            switch (l)
            {
                case 1:
                    for (int t = 0; t < PlainText.Length; t++)
                    {
                        TempNumber = rng.Next(m);
                        ResultString += alphabet[TempNumber];
                    }
                    return ResultString;

                case 2:
                    for (int t = 0; t < PlainText.Length; t+= 2)
                    {
                        TempNumber = rng.Next(m * m);
                        ResultString += alphabet[TempNumber / m];
                        ResultString += alphabet[TempNumber % m];
                    }
                    return ResultString;

                default:
                    return "Error";
            }
        }

        static string Dist4(string PlainText, char[] alphabet, int l)
        {
            int m = alphabet.Length;
            string ResultString = "";
            int TempNumber, rng1, rng2;
            Random rng = new Random();
            

            switch(l)
            {
                case 1:
                    rng1 = rng.Next(m);
                    rng2 = rng.Next(m);
                    for (int t = 0; t < PlainText.Length; t++)
                    {
                        TempNumber = (rng1 + rng2) % m;
                        ResultString += alphabet[TempNumber];
                        rng1 = rng2;
                        rng2 = rng.Next(m);
                    }
                    return ResultString;

                case 2:
                    rng1 = rng.Next(m * m);
                    rng2 = rng.Next(m * m);
                    for (int t = 0; t < PlainText.Length; t+= 2)
                    {
                        TempNumber = (rng1 + rng2) % (m * m);
                        ResultString += alphabet[TempNumber / m];
                        ResultString += alphabet[TempNumber % m];
                        rng1 = rng2;
                        rng2 = rng.Next(m);
                    }
                    return ResultString;
                    
                default:
                    return "Error";
            }
        }



        static string[] DistortTexts(int L, int N, int DistortionMethod)
        {
            string MainText = File.ReadAllText("pr_text.txt");
            char[] Letter = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й',
                'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
            
            
            string[] DistdText = new string[N];
            string TempString;

            int r = 5;  // key lenght for Dist1
            int l = 1;  // l-gram
            

            switch (DistortionMethod)
            {
                case 1:
                    for(int i = 0; i < N; N++)
                    {
                        TempString = MainText.Substring((i * L), L);
                        DistdText[i] = Dist1(TempString, Letter, r, l);
                    }
                    break;

                case 2:
                    for (int i = 0; i < N; N++)
                    {
                        TempString = MainText.Substring((i * L), L);
                        DistdText[i] = Dist2(TempString, Letter, l);
                    }
                    break;

                case 3:
                    for (int i = 0; i < N; N++)
                    {
                        TempString = MainText.Substring((i * L), L);
                        DistdText[i] = Dist3(TempString, Letter, l);
                    }
                    break;

                case 4:
                    for (int i = 0; i < N; N++)
                    {
                        TempString = MainText.Substring((i * L), L);
                        DistdText[i] = Dist4(TempString, Letter, l);
                    }
                    break;

                default:
                    Console.WriteLine("Generation method is unknown");
                    break;
            }
            return DistdText;
        }



        static void Main(string[] args)
        {

            string MainText = File.ReadAllText("pr_text.txt");
            char[] Letter = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й',
                'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };

            
            //TextProcessing();
            var t = Dist2(MainText, Letter, 1);            
            Console.WriteLine(t);

            var d = Dist4(MainText, Letter, 2);
            Console.WriteLine(d);
           

            Console.ReadKey();
        }
    }
}
