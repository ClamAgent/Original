// This codefile is the "header" file for the ClamAgent.mc.dll event messages resource dll
using System;

namespace ClamAgent
{
    // 00002XXXX - Warning
    // 00001XXXX - Error
    // 00004XXXX - Information
    // 00000 - 00FF Debug
    // 00100 - 01FF Application
    // 00200 - 02FF ini
    public enum EventLogMessages
    {
        INFORMATION_DEBUG_MSG = 0x00040000,
        WARNING_INI_NOT_FOUND = 0x00020220,
        WARNING_INI_CONVERSION = 0x00020221,
        ERROR_INI_READFILE = 0x00010210
    }
}