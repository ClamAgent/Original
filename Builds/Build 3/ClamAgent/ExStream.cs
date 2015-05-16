using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClamAgent
{
    public static class ExStream
    {
        public static void SaveToFile(Stream stream, string filename)
        {
            SaveToFile(stream, filename, 8192);
        }
        public static void SaveToFile(Stream stream, string filename, int bufflen)
        {
            int i;
            FileStream SaveFile = File.Create(filename);
            byte[] buffer = new byte[bufflen];
            for (i = bufflen; i == bufflen; )
            {
                i = stream.Read(buffer, 0, i);
                SaveFile.Write(buffer, 0, i);
            }
            SaveFile.Close();
            stream.Close();
        }
    }
}
