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
                        frq[i]++;
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
                    for (int j = 0; j < m; j++)
                        if ((Text[t] == alphabet[i]) && (Text[t + 1] == alphabet[j]))
                            frq[32 * i + j]++;
            }
            for (int i = 0; i < frq.Length; i++)
            {
                frq[i] = (frq[i] * 2) / (double)Text.Length;
                //Console.WriteLine(alphabet[i/32] + " " +  alphabet[i%32] + " :  " + frq[i]);
            }
            return frq;
        }



        static string[] GenerateTexts(int L, int N, int GenerationMethod)
        {
            string MainText = File.ReadAllText("pr_text.txt");
            char[] Letter = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й',
                'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };




            string[] GendText = new string[N];
            switch (GenerationMethod)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    Console.WriteLine("Generation method is unknown");
                    break;
            }
            return GendText;
        }



        static void Main(string[] args)
        {

            string MainText = File.ReadAllText("pr_text.txt");
            char[] Letter = new char[] {'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й',
                'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };

            TextProcessing();
            //var t = MonogramsFrequency(MainText, Letter);
            var t = BigramsFrequency(MainText, Letter);
            double z = 0;
            for (int i = 0; i < t.Length ; i++)
                z += t[i];
            Console.WriteLine(z);



            Console.ReadKey();
        }
    }
}
