using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClamAgent
{
    public static class ExtendedPath
    {
        public static string EscapePath(string pathname)
        {
            return ExtendedPath.Escape(pathname, Path.GetInvalidPathChars());
        }
        public static string EscapeFile(string filename)
        {
            return ExtendedPath.Escape(filename, Path.GetInvalidFileNameChars());
        }
        private static string Escape(string src, char[] invalidchars)
        {
            string retvalue = src;
            int i;
            for (i = 0; i < invalidchars.Length; i++)
            {
                retvalue = retvalue.Replace(invalidchars[i].ToString(),"%" + ExtendedPath.CharToHex(invalidchars[i]));
            }
            return retvalue;
        }
        private static string CharToHex(char src)
        {
            string retvalue = "";
            UnicodeEncoding uni = new UnicodeEncoding();
            byte[] ascii = uni.GetBytes(src.ToString());
            int i;
            for (i = 0; i < ascii.Length; i++)
            {
                retvalue += ExtendedPath.TrailHex(ascii[i].ToString("X"), 2);
            }
            return retvalue;
        }
        private static string TrailHex(string hex, int length)
        {
            string zeros = "";
            int i;
            for (i = 0; i < length - hex.Length; i++)
            {
                zeros += "0";
            }
            return zeros + hex;
        }
    }
}
