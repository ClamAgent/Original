using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ClamAgent
{
    public class caSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue(@"c:\work")]
        public string WorkingDirectory
        {
            get
            {
                return ((string)this["WorkingDirectory"]);
            }
        }
    }
}
