using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            // caSettings settings = new caSettings();
            // Console.WriteLine(settings.WorkingDirectory);
            // settings.Reload();
            Console.WriteLine(ConfigurationManager.AppSettings["WorkingDirectory"]);
            // settings.WorkingDirectory = "c:\\test";
            // settings.Save();
        }
    }
}
