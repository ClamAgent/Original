using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace ClamAgent
{
    public class ClamWin
    {
        public ClamWin() {}
        public ClamWin(Settings settings)
        {
            this._clamScanPath = settings.ClamScanPath;
            this._quarantineFolder = settings.QuarantineFolder;
            this._clamWindDBPath = settings.ClamWindDBPath;
            this._BuildArguments();
        }
        private string _clamScanPath = "";
        public string ClamScanPath
        {
            set
            {
                this._clamScanPath = value;
            }
        }
        private string _quarantineFolder = "";
        public string QuarantineFolder
        {
            set
            {
                this._quarantineFolder = value;
                this._BuildArguments();
            }
        }
        private string _scanfolder = "";
        public string ScanFolder
        {
            set
            {
                this._scanfolder = value;
                this._BuildArguments();
            }
        }
        private string _clamWindDBPath = "";
        public string ClamWindDBPath
        {
            set
            {
                this._clamWindDBPath = value;
                this._BuildArguments();
            }
        }
        private int _timeout = int.MaxValue;
        public int Timeout
        {
            set
            {
                this._timeout = value;
            }
        }
        private string _arguments = "";
        private void _BuildArguments()
        {
            this._arguments = "\"" + this._scanfolder + "\" " +
                "--move=\"" + this._quarantineFolder + "\" " +
                "--max-recursion=10 " +
                "--database=\"" + this._clamWindDBPath + "\"";
        }
        public string Execute()
        {
            string retvalue = "";
            ProcessStartInfo cw_psi = new ProcessStartInfo(this._clamScanPath);
            cw_psi.RedirectStandardError = true;
            cw_psi.Arguments = this._arguments;
            cw_psi.UseShellExecute = false;
            cw_psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process cw_process = Process.Start(cw_psi);
            cw_process.WaitForExit(this._timeout);
            retvalue = cw_process.HasExited ? cw_process.StandardError.ReadToEnd() : null;
            return retvalue;
        }
    }
}
