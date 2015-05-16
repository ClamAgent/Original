using System;
using System.Collections.Generic;
using System.Text;

namespace Test3
{
    class Program
    {
        static void Main(string[] args)
        {
            UnicodeEncoding unicode = new UnicodeEncoding();
            byte[] ascii = unicode.GetBytes("Ő");
            int i;
            for (i = 0; i < ascii.Length; i++)
            {

                Console.WriteLine(ascii[i].ToString("X"));
            }
        }
    }
}
