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
