using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace ClamAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            ClamD clam = new ClamD("10.1.1.201", 3310);
            Console.WriteLine(clam.ScanStream(File.OpenRead("c:\\DATA\\virus test\\eicar.com")).Description);
        }
    }
}
