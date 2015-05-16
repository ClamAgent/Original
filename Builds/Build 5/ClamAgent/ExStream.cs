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
   ExStream.cs

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
    /// Buffered write extension to the Stream object
    /// Can be used to save message or attachment stream
    /// </summary>
    public static class ExStream
    {
        /// <summary>
        /// Save a stream contents to a file
        /// </summary>
        /// <param name="stream">The stream to save</param>
        /// <param name="filename">Name of the resulting file</param>
        public static void SaveToFile(Stream stream, string filename)
        {
            SaveToFile(stream, filename, 8192);
        }
        /// <summary>
        /// Save a stream contents to a file
        /// </summary>
        /// <param name="stream">The stream to save</param>
        /// <param name="filename">Name of the resulting file</param>
        /// <param name="bufflen">Length of the buffer in bytes</param>
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
