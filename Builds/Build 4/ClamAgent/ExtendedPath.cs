/* 
 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
 EITHER EXPRESSED OR IMPLIED,  INCLUDING BUT NOT LIMITED TO THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

 This code is free for both personal and commercial use, but you are
 expressly forbidden from selling.

 **************************************************************************
 PROJECT NAME:
   ClamAgent

 DESCRIPTION:
   This is an Exchange 2007 Transport Agent for virus scanning
   
 FILE NAME:
   ExtendedPath.cs

 COPYRIGHT:
   Copyright (c) Zoltán Gömöri. 2008.
   All rights reserved.
   
 NOTES:
   The original version of this source code, the compiled binaries, and
   the documentation be found at:
     http://www.clamagent.org

 CREATED:
   2008.11.13
   
 LAST MODIFIED:
   2008.12.09
   
 VERSION:
   v1.0 Build 1 - Initial version
   v1.0 Build 2 - Change Access to the configuration file
                  Improoved file name handling
                  Logging
                  Error handling
   v1.0 Build 3 - Change the type of the configuration file
   v1.0 Build 4 - Added the option to save all crossing mail
                  Reorganized to be able to create a fork for
                  Exchange 2003/IIS SMTP
   
TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClamAgent
{
    /// <summary>
    /// Extension to the System.Path class
    /// Can be used to replace the invalid characters in the file and path names to unicode escape sequence
    /// </summary>
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
