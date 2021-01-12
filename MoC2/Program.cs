using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;






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
            Text = String.Join("", Text.Split('/', '.', ',', '-', '—', '«', '»', '(', ')', '[', ']', '{', '}', '!', '?', '`', '\'',
               '#', '*', '№', ':', ';', '…', ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'));
            Text = Regex.Replace(Text, @"\s", "");
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
            for (int i = 0; i < m; i++)
            {
                frq[i] = frq[i] / (double)Text.Length;
            }
            return frq;
        }

        static int[] MonogramsCount(string Text, char[] alphabet)
        {
            int m = alphabet.Length;
            int[] frq = new int[m];
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
            }
            return frq;
        }

        static int[] BigramsCount(string Text, char[] alphabet)
        {
            int m = alphabet.Length;
            int[] frq = new int[m * m];
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
            return frq;
        }

        static int[] GetFreq(double[] frequency, int quantity)
        {
            List<double> FrqList = frequency.ToList<double>();
            int[] FrqGramms = new int[quantity];
            for (int i = 0; i < quantity; i++)
            {
                FrqGramms[i] = FrqList.IndexOf(FrqList.Max());
                FrqList[FrqGramms[i]] = 0;
            }
            return FrqGramms;
        }

        static int[] GetProh(double[] frequency, int quantity)
        {
            List<double> PrhList = frequency.ToList<double>();
            int[] PrhGramms = new int[quantity];
            for (int i = 0; i < quantity; i++)
            {
                PrhGramms[i] = PrhList.IndexOf(PrhList.Min());
                PrhList[PrhGramms[i]] = 1;
            }
            return PrhGramms;

        }


        static double Entropy(string Text, char[] alphabet, int l)
        {
            int L = Text.Length;
            var pow = Math.Pow(alphabet.Length, l);
            int m = (int)pow;
            double entr = 0;
            double[] frequency = new double[m];

            switch (l)
            {
                case 1:
                    frequency = MonogramsFrequency(Text, alphabet);
                    break;

                case 2:
                    frequency = BigramsFrequency(Text, alphabet);
                    break;

                default:
                    Console.WriteLine("Error");
                    break;
            }

            for (int i = 0; i < m; i++)
            {
                if (frequency[i] != 0)
                    entr -= (frequency[i] * Math.Log(frequency[i], 2));
            }
            entr = entr / (double)l;
            return entr;
        }

        static double CompIdx(string Text, char[] alphabet, int l)
        {
            int L = Text.Length;
            var pow = Math.Pow(alphabet.Length, l);
            int m = (int)pow;
            double CIDX = 0;
            int[] count = new int[m];

            switch (l)
            {
                case 1:
                    count = MonogramsCount(Text, alphabet);
                    break;

                case 2:
                    count = BigramsCount(Text, alphabet);
                    break;

                default:
                    Console.WriteLine("Error");
                    break;
            }

            for (int i = 0; i < m; i++)
            {
                CIDX += (count[i] * (count[i] - 1));
            }
            CIDX = CIDX / (double)(L * (L - 1));
            return CIDX;
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
                    for (int t = 0; t < PlainText.Length; t += 2)
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

            switch (l)
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
                    for (int t = 0; t < PlainText.Length; t += 2)
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
                    for (int t = 0; t < PlainText.Length; t += 2)
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

        static string Dist4(string PlainText, char[] alphabet, int l)  // формула
        {
            int m = alphabet.Length;
            string ResultString = "";
            int TempNumber, rng1, rng2;
            Random rng = new Random();


            switch (l)
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
                    for (int t = 0; t < PlainText.Length; t += 2)
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



        static string[] DistortTexts(int L, int N, int DistortionMethod, int d, int l)
        {
            string MainText = File.ReadAllText("pr_text.txt");
            char[] Letter = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й',
                'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };


            string[] DistdText = new string[N];
            string TempString;

            int r;  // key lenght for Dist1
            //int l = 1;  // l-gramm
            //int d = 105;  // for text partitioning
            if (l == 1)
                r = 5;
            else
                r = 10;



            switch (DistortionMethod)
            {
                case 1:
                    for (int i = 0; i < N; i++)
                    {
                        TempString = MainText.Substring((i * (L / d)), L);
                        DistdText[i] = Dist1(TempString, Letter, r, l);
                    }
                    break;

                case 2:
                    for (int i = 0; i < N; i++)
                    {
                        TempString = MainText.Substring((i * (L / d)), L);
                        DistdText[i] = Dist2(TempString, Letter, l);
                    }
                    break;

                case 3:
                    for (int i = 0; i < N; i++)
                    {
                        TempString = MainText.Substring((i * (L / d)), L);
                        DistdText[i] = Dist3(TempString, Letter, l);
                    }
                    break;

                case 4:
                    for (int i = 0; i < N; i++)
                    {
                        TempString = MainText.Substring((i * (L / d)), L);
                        DistdText[i] = Dist4(TempString, Letter, l);
                    }
                    break;

                default:
                    Console.WriteLine("Generation method is unknown");
                    break;
            }
            return DistdText;
        }


        


        static int Crit2_0(int l, double[] TrueFreq, string Text, char[] alphabet, int FrequentQuantity)
        {
            //int FrequentQuantity = 1;                                       // quantity of frequent l-gramms
            var FrequentGramms = GetFreq(TrueFreq, FrequentQuantity);
            int r = (int)Math.Pow(alphabet.Length, l);
            int m = alphabet.Length;
            int result = 0;
            int[] GrammsCounter = new int[r];

            switch (l)
            {
                case 1:
                    for (int t = 0; t < Text.Length; t++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            if ((Text[t] == alphabet[i]) && (GrammsCounter[i] == 0))
                            {
                                GrammsCounter[i]++;
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < FrequentGramms.Length; i++)
                        if (GrammsCounter[FrequentGramms[i]] == 0)
                        {
                            result++;
                            break;
                        }
                    break;

                case 2:
                    for (int t = 0; t < Text.Length; t += 2)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            for (int j = 0; j < m; j++)
                            {
                                if ((Text[t] == alphabet[i]) && (Text[t + 1] == alphabet[j]) && (GrammsCounter[m * i + j] == 0))
                                {
                                    GrammsCounter[m * i + j]++;
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < FrequentGramms.Length; i++)
                        if (GrammsCounter[FrequentGramms[i]] == 0)
                        {
                            result++;
                            break;
                        }
                    break;
            }

            if (result == 0)
                //Console.WriteLine("Accept the hypotesis H0");
                return 0;
            else
                //Console.WriteLine("Accept the hypotesis H1");            
                return 1;
        }

        static int Crit2_1(int l, double[] TrueFreq, string Text, char[] alphabet, int FrequentQuantity, int HypotesisLimit)
        {
            //int FrequentQuantity = 5;                                       // quantity of frequent l-gramms
            //int HypotesisLimit = (FrequentQuantity * 2) / 3;
            var FrequentGramms = GetFreq(TrueFreq, FrequentQuantity);
            int r = (int)Math.Pow(alphabet.Length, l);
            int m = alphabet.Length;
            int result = 0;
            int[] GrammsCounter = new int[r];

            switch (l)
            {
                case 1:
                    for (int t = 0; t < Text.Length; t++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            if ((Text[t] == alphabet[i]) && (GrammsCounter[i] == 0))
                            {
                                GrammsCounter[i]++;
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < FrequentGramms.Length; i++)
                        result += GrammsCounter[FrequentGramms[i]];
                    break;

                case 2:
                    for (int t = 0; t < Text.Length; t += 2)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            for (int j = 0; j < m; j++)
                            {
                                if ((Text[t] == alphabet[i]) && (Text[t + 1] == alphabet[j]) && (GrammsCounter[m * i + j] == 0))
                                {
                                    GrammsCounter[m * i + j]++;
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < FrequentGramms.Length; i++)
                        result += GrammsCounter[FrequentGramms[i]];
                    break;
            }

            if (result < HypotesisLimit)
                //Console.WriteLine("Accept the hypotesis H1");
                return 1;
            else
                //Console.WriteLine("Accept the hypotesis H0");
                return 0;
        }

        static int Crit2_2(int l, double[] TrueFreq, string Text, char[] alphabet, int FrequentQuantity)
        {
            //int FrequentQuantity = 5;                                       // quantity of frequent l-gramms
            var FrequentGramms = GetFreq(TrueFreq, FrequentQuantity);
            double[] TextFreq;
            int result = 0;
            
            switch (l)
            {
                case 1:
                    TextFreq = MonogramsFrequency(Text, alphabet);
                    break;

                case 2:
                    TextFreq = BigramsFrequency(Text, alphabet);
                    break;

                default:
                    TextFreq = new double[1];
                    break;
            }

            for (int i = 0; i < FrequentGramms.Length; i++)
                if (TextFreq[FrequentGramms[i]] < TrueFreq[FrequentGramms[i]])
                {
                    result++;
                    break;
                }
            if (result == 0)
                //Console.WriteLine("Accept the hypotesis H0");
                return 0;
            else
                //Console.WriteLine("Accept the hypotesis H1");
                return 1;
        }

        static int Crit2_3(int l, double[] TrueFreq, string Text, char[] alphabet, int FrequentQuantity)
        {
            //int FrequentQuantity = 5;                                       // quantity of frequent l-gramms
            var FrequentGramms = GetFreq(TrueFreq, FrequentQuantity);
            double[] TextFreq;
            double TrueFreqSum = 0, TextFreqSum = 0;
            
            switch (l)
            {
                case 1:
                    TextFreq = MonogramsFrequency(Text, alphabet);
                    break;

                case 2:
                    TextFreq = BigramsFrequency(Text, alphabet);
                    break;

                default:
                    TextFreq = new double[1];
                    break;
            }

            for (int i = 0; i < FrequentGramms.Length; i++)
            {
                TrueFreqSum += TrueFreq[FrequentGramms[i]];
                TextFreqSum += TextFreq[FrequentGramms[i]];
            }
            if (TextFreqSum < TrueFreqSum)
                //Console.WriteLine("Accept the hypotesis H1");
                return 1;
            else
                //Console.WriteLine("Accept the hypotesis H0");
                return 0;
        }

        static int Crit4_0(int l, string TrueText, string Text, char[] alphabet, double HypotesisLimit)
        {
            double TrueCIDX, TextCIDX;
            //double HypotesisLimit = 0;
            double result;

            TrueCIDX = CompIdx(TrueText, alphabet, l);
            TextCIDX = CompIdx(Text, alphabet, l);

            result = Math.Abs(TrueCIDX - TrueCIDX);

            
            if (result > HypotesisLimit)
                //Console.WriteLine("Accept the hypotesis H1");
                return 1;
            else
                //Console.WriteLine("Accept the hypotesis H0");
                return 0;

        }

        static int Crit5_0(int l, double[] TrueFreq, string Text, char[] alphabet)
        {
            int ProhibitetQuantity = 50;                                       // quantity of frequent l-gramms
            int HypotesisLimit = ProhibitetQuantity /2;
            var ProhibitetGramms = GetProh(TrueFreq, ProhibitetQuantity);
            int r = (int)Math.Pow(alphabet.Length, l);
            int m = alphabet.Length;
            int result = 0;
            int[] GrammsCounter = new int[r];

            switch (l)
            {
                case 1:
                    for (int t = 0; t < Text.Length; t++)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            if ((Text[t] == alphabet[i]) && (GrammsCounter[i] == 0))
                            {
                                GrammsCounter[i]++;
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < ProhibitetGramms.Length; i++)
                        result += GrammsCounter[ProhibitetGramms[i]];
                    break;

                case 2:
                    for (int t = 0; t < Text.Length; t += 2)
                    {
                        for (int i = 0; i < m; i++)
                        {
                            for (int j = 0; j < m; j++)
                            {
                                if ((Text[t] == alphabet[i]) && (Text[t + 1] == alphabet[j]) && (GrammsCounter[m * i + j] == 0))
                                {
                                    GrammsCounter[m * i + j]++;
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < ProhibitetGramms.Length; i++)
                        result += GrammsCounter[ProhibitetGramms[i]];
                    break;
            }
            int resultEmpty = ProhibitetQuantity - result;
            if (resultEmpty <= HypotesisLimit)
                //Console.WriteLine("Accept the hypotesis H1");
                return 1;
            else
                //Console.WriteLine("Accept the hypotesis H0");
                return 0;
        }






        static void Main(string[] args)
        {
            //TextProcessing();
            string MainText = File.ReadAllText("pr_text.txt");
            char[] Letter = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й',
                'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
            //Console.WriteLine(MainText.Length);
            var TrueMonoFrq = MonogramsFrequency(MainText, Letter);
            var TrueBiFrq = BigramsFrequency(MainText, Letter);

            int[] L = new int[] { 10, 100, 1000, 10000 };
            int[] N = new int[] { 10000, 10000, 10000, 1000 };
            int[] d = new int[] { 1, 3, 105, 15 };
            //int[] mfrq = new int[] { 2, 3, 3, 3 };
            //int[] mlim = new int[] { 1, 3, 3, 3 };
            double[] mlim = new double[] { 0.1, 0.02, 0.02, 0.02 };
            //int[] bfrq = new int[] { 3, 10, 10, 10 };
            //int[] blim = new int[] { 1, 4, 4, 4 };
            double[] blim = new double[] { 0.1, 0.02, 0.02, 0.02 };
            int dist_method;
            int HCounter_1;
            int HCounter_2;
            string MainTextPart;
           

            
            using (StreamWriter sw = File.CreateText("4_0.txt"))
            {



                for (dist_method = 1; dist_method <= 4; dist_method++)
                {
                    sw.WriteLine("Distortion method - " + dist_method);
                    sw.WriteLine();
                    for (int t = 0; t < 4; t++)
                    {
                        sw.WriteLine("L = " + L[t] + "  N = " + N[t] + " l = " + 1);
                        //Console.WriteLine("Distortioning texts.");
                        string[] distTexts = DistortTexts(L[t], N[t], dist_method, d[t], 1);
                        //Console.WriteLine("Distortion complete.");
                        //Console.WriteLine("Launching criteria check.");
                        MainTextPart = MainText.Substring(2 * L[t], L[t]);
                        HCounter_1 = 0;
                        HCounter_2 = 0;


                        for (int i = 0; i < N[t]; i++)
                        {
                            HCounter_1 += Crit4_0(1, TrueMonoFrq, distTexts[i], Letter, mlim[t]);
                            HCounter_2 += Crit4_0(2, TrueBiFrq, distTexts[i], Letter, blim[t]);
                            
                        }

                        //Console.WriteLine("Criteria check complete.");




                        sw.WriteLine("mono: " + (N[t] - HCounter_1) / (float)N[t]);
                        sw.WriteLine(" bi : " + (N[t] - HCounter_2) / (float)N[t]);
                        sw.WriteLine();
                        sw.WriteLine();

                    }

                    sw.WriteLine("--------------------------------------------------------------------------------------------");
                    sw.WriteLine();
                    sw.WriteLine();
                    Console.WriteLine("Distortion method - " + dist_method + " l = " + 1);



                    for (int t = 0; t < 4; t++)
                    {
                        sw.WriteLine("L = " + L[t] + "  N = " + N[t] + " l = " + 2);
                        //Console.WriteLine("Distortioning texts.");
                        string[] distTexts = DistortTexts(L[t], N[t], dist_method, d[t], 2);
                        //Console.WriteLine("Distortion complete.");
                        //Console.WriteLine("Launching criteria check.");
                        MainTextPart = MainText.Substring(2 * L[t], L[t]);
                        HCounter_1 = 0;
                        HCounter_2 = 0;


                        for (int i = 0; i < N[t]; i++)
                        {
                            HCounter_1 += Crit4_0(1, TrueMonoFrq, distTexts[i], Letter, mlim[t]);
                            HCounter_2 += Crit4_0(2, TrueBiFrq, distTexts[i], Letter, blim[t]);
                        }

                        //Console.WriteLine("Criteria check complete.");




                        sw.WriteLine("mono: " + (N[t] - HCounter_1) / (float)N[t]);
                        sw.WriteLine(" bi : " + (N[t] - HCounter_2) / (float)N[t]);
                        sw.WriteLine();
                        sw.WriteLine();
                    }
                    sw.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////");
                    Console.WriteLine("Distortion method - " + dist_method + " l = " + 2);
                }

            }

                
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
