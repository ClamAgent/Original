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
   ClamWin.cs

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
   2008.12.16
   
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

TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace ClamAgent
{
    /// <summary>
    /// clamscan.exe's wrapper. This interfaces the virus scanning engine itself
    /// </summary>
    public class ClamWin
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="settings">Settings object represent the app.settings file</param>
        public ClamWin(Settings settings)
        {
            // Save the app.settings parameters to private variables
            this._clamScanPath = settings.ClamScanPath;
            this._quarantineFolder = settings.QuarantineFolder;
            this._clamWindDBPath = settings.ClamWindDBPath;
            // Build the clamscan's command line from the parameters
            this._BuildArguments();
        }
        /// <summary>
        /// Path of the clamscan.exe
        /// </summary>
        private string _clamScanPath = "";
        /// <summary>
        /// Path of the clamscan.exe
        /// </summary>
        public string ClamScanPath
        {
            set
            {
                this._clamScanPath = value;
            }
        }
        /// <summary>
        /// Location of the Quarantine
        /// </summary>
        private string _quarantineFolder = "";
        /// <summary>
        /// Location of the Quarantine
        /// </summary>
        public string QuarantineFolder
        {
            set
            {
                this._quarantineFolder = value;
                this._BuildArguments();
            }
        }
        /// <summary>
        /// The folder to scan
        /// </summary>
        private string _scanfolder = "";
        /// <summary>
        /// The folder to scan
        /// </summary>
        public string ScanFolder
        {
            set
            {
                this._scanfolder = value;
                this._BuildArguments();
            }
        }
        /// <summary>
        /// Path of the ClamAV's virus database
        /// </summary>
        private string _clamWindDBPath = "";
        /// <summary>
        /// Path of the ClamAV's virus database
        /// </summary>
        public string ClamWindDBPath
        {
            set
            {
                this._clamWindDBPath = value;
                this._BuildArguments();
            }
        }
        /// <summary>
        /// Timeout of the scanning - it set to infinite
        /// (In later version it can be configured through the app.settings)
        /// </summary>
        private int _timeout = int.MaxValue;
        /// <summary>
        /// Timeout of the scanning
        /// </summary>
        public int Timeout
        {
            set
            {
                this._timeout = value;
            }
        }
        /// <summary>
        /// Placehonder of the clamscan's argument list
        /// </summary>
        private string _arguments = "";
        /// <summary>
        /// Build the argument list from the parameters
        /// </summary>
        private void _BuildArguments()
        {
            this._arguments = "\"" + this._scanfolder + "\" " +
                "--move=\"" + this._quarantineFolder + "\" " +
                "--max-recursion=10 " +
                "--database=\"" + this._clamWindDBPath + "\"";
        }
        /// <summary>
        /// Execution of the virus scanning
        /// </summary>
        /// <returns>The standard output and error strings</returns>
        public string Execute()
        {
            string retvalue = "";
            // Create a ProcessStartInfo object for the the clamscan.exe
            ProcessStartInfo cw_psi = new ProcessStartInfo(this._clamScanPath);
            // Set to redirect the standard input and the standard error.
            // We will give that back for later use
            cw_psi.RedirectStandardError = true;
            cw_psi.RedirectStandardOutput = true;
            // Set the command line arguments
            cw_psi.Arguments = this._arguments;
            // Run without shell
            cw_psi.UseShellExecute = false;
            // Run it hidden
            cw_psi.WindowStyle = ProcessWindowStyle.Hidden;
            // Start the execution - the real virus scanning happens here
            Process cw_process = Process.Start(cw_psi);
            // Wait until the execution finished
            cw_process.WaitForExit(this._timeout);
            // Give back the standard error and standard output strings
            retvalue = cw_process.HasExited ? cw_process.StandardError.ReadToEnd() + cw_process.StandardOutput.ReadToEnd() : null;
            return retvalue;
        }
    }
}
