using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClamAgent
{
    public abstract class ExStream : Stream
    {
        public void SaveToFile(string filename)
        {
            SaveToFile(filename, 8192);
        }
        public void SaveToFile(string filename, int bufflen)
        {
            int i;
            FileStream SaveFile = File.Create(filename);
            byte[] buffer = new byte[bufflen];
            for (i = bufflen; i == bufflen; )
            {
                i = this.Read(buffer, 0, i);
                SaveFile.Write(buffer, 0, i);
            }
            SaveFile.Close();
            this.Close();
        }
    }
}
