using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Test2
{
    public class caSettings : ApplicationSettingsBase
    {
        [ApplicationScopedSetting()]
        [DefaultSettingValue(@"c:\work")]
        public string WorkingDirectory
        {
            get
            {
                return ((string)this["WorkingDirectory"]);
            }
            set
            {
                this["WorkingDirectory"] = (string)value;
            }
        }
    }
}
