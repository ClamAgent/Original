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
   Logging.cs

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
using System.Threading;

namespace ClamSink
{
    /// <summary>
    /// File based, thread safe logging facility
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="settings">Settings object based on the app.settings</param>
        public Logging(Settings settings)
        {
            this._settings = settings;
            this._logmutex = new Mutex(false, "Global\\ClamAgent_logging_CF25C61-FFFE-4078-99ED-AE797A501F03");
        }
        /// <summary>
        /// Write to the logfile
        /// </summary>
        /// <param name="text">Text to write to the log</param>
        public void Write(string text)
        {
            // generate content and filename
            DateTime logdt = DateTime.Now;
            string logfilename = Path.Combine(this._settings.LogPath, this._settings.LogSplitDays ? "ca" + logdt.ToString("yyMMdd") + ".log" : this._settings.LogFile);
            string logtext = logdt.ToString("yyyy-MM-dd HH:mm:ss") + "," + text;
            // get mutex
            this._logmutex.WaitOne();
            // open file
            try
            {
                // write
                StreamWriter logfile = new StreamWriter(logfilename, true);
                logfile.WriteLine(logtext);
                // close file
                logfile.Close();
            }
            catch (Exception err) {}
            // release mutex
            this._logmutex.ReleaseMutex();
        }
        /// <summary>
        /// Settings object based on the app.settings
        /// </summary>
        private Settings _settings;
        /// <summary>
        /// Thread syncronization mutex for the log file writeing
        /// </summary>
        private Mutex _logmutex;
    }
}
