using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace ClamAgent
{
    public class IniFile
    {
        public IniFile()
        {
            this.Type = IniProcessType.First;
            LoadIni(this.GetIniName());
        }
        public IniFile(string FileName)
        {
            this.Type = IniProcessType.First;
            LoadIni(FileName);
        }
        public IniFile(IniProcessType type)
        {
            this.Type = type;
            LoadIni(this.GetIniName());
        }
        public IniFile(string FileName, IniProcessType type)
        {
            this.Type = type;
            LoadIni(FileName);
        }
        private string GetIniName()
        {
            string AssemblyName = Assembly.GetExecutingAssembly().Location;
            return AssemblyName.Substring(0, AssemblyName.LastIndexOf('.')) + ".ini";
        }
        public Dictionary<string, List<string>> this[string Section]
        {
            get
            {
                string CurrSection = Section.ToLower().Trim();
                return this.IniData.ContainsKey(CurrSection) ? IniData[CurrSection] : null;
            }
        }

        private class SectionKeyPair
        {
            public SectionKeyPair() { }
            public SectionKeyPair(string section, string key)
            {
                this.Section = section.ToLower().Trim();
                this.Key = key.ToLower().Trim();
            }
            public string Section;
            public string Key;
            public bool IsValid = false;
        }
        private SectionKeyPair ValidateKey(string Section, string Key)
        {
            SectionKeyPair retvalue = new SectionKeyPair(Section,Key);
            if (this.IniData.ContainsKey(retvalue.Section))
                if (this.IniData[retvalue.Section].ContainsKey(retvalue.Key))
                    retvalue.IsValid = true;
            return retvalue;
        }
        private int GenValueIndex(SectionKeyPair sk)
        {
            int index = -1;
            if (sk.IsValid)
            {
                switch (this.Type)
                {
                    case IniProcessType.First:
                        index = 0;
                        break;
                    case IniProcessType.Last:
                        index = this.IniData[sk.Section][sk.Key].Count - 1;
                        break;
                }
            }
            return index;
        }
        public string GetValueStr(string Section, string Key, string Default)
        {
            string retvalue = this.GetValueStr(Section, Key);
            return retvalue != null ? retvalue : Default;
        }
        public string GetValueStr(string Section, string Key)
        {
            int index;
            SectionKeyPair seckey = ValidateKey(Section, Key);
            index = GenValueIndex(seckey);
            return (index >= 0) ? this.IniData[seckey.Section][seckey.Key][index] : null;
        }
        public string[] GetMultiValueStr(string Section, string Key)
        {
            SectionKeyPair seckey = ValidateKey(Section, Key);
            return (seckey.IsValid) ? this.IniData[seckey.Section][seckey.Key].ToArray() : new string[0];
        }
        public int GetValueInt(string Section, string Key)
        {
            return this.GetValueInt(Section, Key, int.MinValue);
        }
        public int GetValueInt(string Section, string Key, int Default)
        {
            int retvalue = Default;
            string strvalue;
            int index;
            SectionKeyPair seckey = ValidateKey(Section, Key);
            index = GenValueIndex(seckey);
            if (index >= 0)
            {
                strvalue = this.IniData[seckey.Section][seckey.Key][index];
                try
                {
                    retvalue = Convert.ToInt32(strvalue);
                }
                catch (Exception err)
                {
                    Log.Write(EventLogMessages.WARNING_INI_CONVERSION, new string[] { strvalue,"Int32",seckey.Section,seckey.Key,err.Message });
                }
            }
            return retvalue;
        }
        public bool GetValueBool(string Section, string Key)
        {
            return this.GetValueBool(Section, Key, false);
        }
        public bool GetValueBool(string Section, string Key, bool Default)
        {
            int index;
            SectionKeyPair seckey = ValidateKey(Section, Key);
            index = GenValueIndex(seckey);
            if (index >= 0)
            {
                switch (this.IniData[seckey.Section][seckey.Key][index].ToLower())
                {
                    case "0":
                        return false;
                    case "off":
                        return false;
                    case "no":
                        return false;
                    case "1":
                        return true;
                    case "on":
                        return true;
                    case "yes":
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return Default;
            }
        }
        private IniProcessType Type;
        private string[] Lines;
        private Dictionary<string, Dictionary<string, List<string>>> IniData;
        private void LoadIni(string FileName)
        {
            int i;
            string Section = String.Empty;
            string CurrLine;
            string CurrKey;
            string CurrValue;
            string[] CurrLineArr;
            IniData = new Dictionary<string, Dictionary<string, List<string>>>();
            if (File.Exists(FileName))
            {
                try
                {
                    Lines = File.ReadAllLines(FileName);
                }
                catch (Exception err)
                {
                    Log.Write(EventLogMessages.ERROR_INI_READFILE, new string[] { FileName, err.Message });
                    Lines = new string[0];
                }
                for (i = 0; i < Lines.Length; i++)
                {
                    CurrLine = Lines[i].Trim();
                    if (CurrLine != "")
                    {
                        if (CurrLine[0] != ';')
                        {
                            if (CurrLine[0] == '[' && CurrLine[CurrLine.Length - 1] == ']')
                            {
                                Section = CurrLine.Trim(new char[] { '[', ']' }).ToLower();
                                if (!IniData.ContainsKey(Section))
                                {
                                    IniData[Section] = new Dictionary<string, List<string>>();
                                }
                            }
                            else
                            {
                                if (Section != String.Empty && CurrLine.Trim() != String.Empty)
                                {
                                    CurrLineArr = CurrLine.Split(new char[] { '=' }, 2);
                                    if (CurrLineArr.Length == 2)
                                    {
                                        CurrKey = CurrLineArr[0].Trim().ToLower();
                                        CurrValue = CurrLineArr[1].Trim(new char[] { ' ', '\"' });
                                        if (CurrValue != "")
                                        {
                                            if (!IniData[Section].ContainsKey(CurrKey))
                                            {
                                                IniData[Section].Add(CurrKey, new List<string>());
                                            }
                                            IniData[Section][CurrKey].Add(CurrValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Log.Write(EventLogMessages.WARNING_INI_NOT_FOUND, new string[] {FileName});
            }
        }
    }
    public enum IniProcessType
    {
        First,
        Last
    }
}
