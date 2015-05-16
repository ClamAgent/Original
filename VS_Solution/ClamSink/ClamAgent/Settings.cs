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
   Settings.cs

 COPYRIGHT:
   Copyright (c) Zoltán Gömöri. 2008, 2009, 2010.
   All rights reserved.
   
 NOTES:
   The original version of this source code, the compiled binaries, and
   the documentation be found at:
     http://www.clamagent.org

 CREATED:
   2008.11.13
   
 LAST MODIFIED:
   2010.05.25
   
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
   v1.0 Build 5 - Added change tracking to the ClamAgentWP
                  (required by the SinkMsg)
   v1.0 Build 6 - Added the attachment stream size setting and flush to the ClamAgentWP
                  (required by the SinkMsg)
   v1.0 Build 8 - Changed the scanning engine from ClamWin to ClamD
   
TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Configuration;

namespace ClamSink
{
    public class Settings
    {
        public Settings()
        {
            String assemblyLocation = Assembly.GetExecutingAssembly().Location;
            this._config = ConfigurationManager.OpenExeConfiguration(assemblyLocation);
        }
        Configuration _config;
/*
        public string WorkingDirectory
        {
            get
            {
                return (this._config.AppSettings.Settings["WorkingDirectory"].Value);
            }
        }
*/
        public string PrependSubject
        {
            get
            {
                return (this._config.AppSettings.Settings["PrependSubject"].Value);
            }
        }
/*
        public string ClamScanPath
        {
            // Default value should be: "c:\\program files\\ClamWin\\bin\\clamscan.exe"
            get
            {
                return (this._config.AppSettings.Settings["ClamScanPath"].Value);
            }
        }
*/
        public string QuarantineFolder
        {
            get
            {
                return (this._config.AppSettings.Settings["QuarantineFolder"].Value);
            }
        }
/*
        public string ClamWindDBPath
        {
            // Default value should be (for W2K3): "C:\\Documents and Settings\\All Users\\.clamwin\\db"
            get
            {
                return (this._config.AppSettings.Settings["ClamWindDBPath"].Value);
            }
        }
*/
        public string LogPath
        {
            get
            {
                return (this._config.AppSettings.Settings["LogPath"].Value);
            }
        }

        public string LogFile
        {
            get
            {
                return (this._config.AppSettings.Settings["LogFile"].Value);
            }
        }

        public bool LogSplitDays
        {
            get
            {
                return (Boolean.Parse(this._config.AppSettings.Settings["LogSplitDays"].Value));
            }
        }

        public string DebugPath
        {
            get
            {
                return (this._config.AppSettings.Settings["DebugPath"].Value);
            }
        }

        public bool DebugAllMessage
        {
            get
            {
                return (Boolean.Parse(this._config.AppSettings.Settings["DebugAllMessage"].Value));
            }
        }

        public string ClamD_Address
        {
            get
            {
                return (this._config.AppSettings.Settings["ClamD_Address"].Value);
            }
        }
        public int ClamD_Port
        {
            get
            {
                return (Int32.Parse(this._config.AppSettings.Settings["ClamD_Port"].Value));
            }
        }
    }
}
