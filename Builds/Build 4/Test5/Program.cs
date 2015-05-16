using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using System.Reflection;

namespace Test5
{
    class Program
    {
        static void Main(string[] args)
        {
            ADODB.Stream teststream = new ADODB.Stream();
            teststream.Open(Missing.Value,ADODB.ConnectModeEnum.adModeUnknown,ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified,null,null);
            teststream.LoadFromFile("c:\\Data\\Test\\test.txt");
            // teststream.Mode = ADODB.ConnectModeEnum.adModeRead;
            teststream.Type = ADODB.StreamTypeEnum.adTypeBinary;
            object testobj = teststream.Read(20);
            Console.WriteLine(testobj);
        }
    }
}
