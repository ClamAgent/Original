using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            ClamWin cw = new ClamWin();
            cw.Executable = @"c:\program files\ClamWin\bin\clamscan.exe";
            cw.ScanFolder = @"c:\work";
            cw.Quarantine = @"C:\quarantine";
            cw.Database = @"C:\Documents and Settings\All Users\.clamwin\db";
            Console.WriteLine(cw.Execute());

// "c:\program files\ClamWin\bin\clamscan.exe" c:\work --move="C:\quarantine" --max-recursion=10 --database="C:\Documents and Settings\All Users\.clamwin\db"

        }
    }
    public class ClamWin
    {
        public ClamWin()
        {
        }
        private string _executable = "";
        public string Executable
        {
            set
            {
                this._executable = value;
            }
        }
        private string _quarantine = "";
        public string Quarantine
        {
            set
            {
                this._quarantine = value;
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
        private string _database = "";
        public string Database
        {
            set
            {
                this._database = value;
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
                "--move=\"" + this._quarantine + "\" " +
                "--max-recursion=10 " +
                "--database=\"" + this._database + "\"";
        }
        public string Execute()
        {
            string retvalue = "";
            ProcessStartInfo cw_psi = new ProcessStartInfo(this._executable);
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
