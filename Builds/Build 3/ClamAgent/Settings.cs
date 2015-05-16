using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Configuration;

namespace ClamAgent
{
    public class Settings
    {
        public Settings()
        {
            String assemblyLocation = Assembly.GetExecutingAssembly().Location;
            this._config = ConfigurationManager.OpenExeConfiguration(assemblyLocation);
        }
        Configuration _config;
        public string WorkingDirectory
        {
            get
            {
                return (this._config.AppSettings.Settings["WorkingDirectory"].Value);
            }
        }

        public string PrependSubject
        {
            get
            {
                return (this._config.AppSettings.Settings["PrependSubject"].Value);
            }
        }

        // [global::System.Configuration.DefaultSettingValueAttribute("c:\\program files\\ClamWin\\bin\\clamscan.exe")]
        public string ClamScanPath
        {
            get
            {
                return (this._config.AppSettings.Settings["ClamScanPath"].Value);
            }
        }

        // [global::System.Configuration.DefaultSettingValueAttribute("C:\\quarantine")]
        public string QuarantineFolder
        {
            get
            {
                return (this._config.AppSettings.Settings["QuarantineFolder"].Value);
            }
        }

        // [global::System.Configuration.DefaultSettingValueAttribute("C:\\Documents and Settings\\All Users\\.clamwin\\db")]
        public string ClamWindDBPath
        {
            get
            {
                return (this._config.AppSettings.Settings["ClamWindDBPath"].Value);
            }
        }

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

        // [global::System.Configuration.DefaultSettingValueAttribute("True")]
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
    }
}
