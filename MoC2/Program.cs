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
            char[] RestrictedChars = new char[] {'/', '.', ',', '-', '(', ')', '!', '?', '`'};
            Text = Text.Replace("\n", ""); Text = Text.Replace("\r", "");
            Text = Text.Replace("ґ", "г"); Text = Text.Replace(" ", "");

            Text = Text.Trim(RestrictedChars);

            Console.WriteLine(Text);
            File.WriteAllText("pr_text.txt", Text);
        }

        static void Main(string[] args)
        {
            TextProcessing();

            Console.ReadKey();
        }
    }
}
