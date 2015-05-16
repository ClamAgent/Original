using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

using Microsoft.Exchange.Data.Transport.Routing;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Email;
using Microsoft.Exchange.Data.Mime;

namespace ClamAgent
{
    public class Logging
    {
        public Logging(Settings settings)
        {
            this._settings = settings;
            this._logmutex = new Mutex(false, "Global\\ClamAgent_logging_CF25C61-FFFE-4078-99ED-AE797A501F03");
        }
        public void Write(string text)
        {
            // generate content and filename
            DateTime logdt = new DateTime();
            string logfilename = Path.Combine(this._settings.LogPath, this._settings.LogSplitDays ? "ca" + logdt.ToString("yyMMdd") + ".log" : this._settings.LogFile);
            string logtext = logdt.ToString("yyyy-MM-dd HH:mm:ss") + text;
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
            catch (Exception err)
            {
            }
            // release mutex
            this._logmutex.ReleaseMutex();
        }
        public void SaveMail(string guid, MailItem mail)
        {
            ((ExStream)mail.GetMimeReadStream()).SaveToFile(Path.Combine(this._settings.DebugPath, guid + ".eml"));
        }
        private Settings _settings;
        private Mutex _logmutex;
    }
}
