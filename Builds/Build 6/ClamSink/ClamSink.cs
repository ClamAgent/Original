/* 
 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
 EITHER EXPRESSED OR IMPLIED,  INCLUDING BUT NOT LIMITED TO THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

 This code is free for both personal and commercial use, but you are
 expressly forbidden from selling.

 **************************************************************************
 PROJECT NAME:
   ClamSink

 DESCRIPTION:
   This is an Exchange 2003/IIS SMTP Event Sink for virus scanning
   
 FILE NAME:
   ClamSink.cs

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

using System.Runtime.InteropServices;
using Microsoft.Exchange.Transport.EventInterop;
using Microsoft.Exchange.Transport.EventWrappers;

namespace ClamSink
{
    [Guid("3418F36D-BC94-48eb-BFBE-2CB6C88C0D5C")]
    [ComVisible(true)]
    public class ClamSink : IMailTransportSubmission, IEventIsCacheable, IPersistPropertyBag
    {
        #region IMailTransportSubmission Members

        public void OnMessageSubmission(MailMsg pIMailMsg, IMailTransportNotify pINotify, IntPtr pvNotifyContext)
        {
            Message Msg = new Message(pIMailMsg);
            this._cawp.Scan(new SinkMsg(Msg));
        }

        #endregion

        #region IPersistPropertyBag Members

        public void GetClassID(out Guid pClassID)
        {
            pClassID = new Guid("3418F36D-BC94-48eb-BFBE-2CB6C88C0D5C");
        }

        public void InitNew() { }

        /// <summary>
        /// IPersistPropertyBag Load function. Executed on the DLL Load
        /// </summary>
        /// <param name="pPropBag"></param>
        /// <param name="pErrorLog"></param>
        public void Load(IPropertyBag pPropBag, IErrorLog pErrorLog)
        {
            Settings settings = new Settings();
            this._cawp = new ClamAgentWP(settings, new ClamWin(settings));
        }

        public void Save(IPropertyBag pPropBag, int fClearDirty, int fSaveAllProperties) { }

        #endregion

        #region IEventIsCacheable Members

        public void IsCacheable() { }

        #endregion

        private ClamAgentWP _cawp;
    }
}
