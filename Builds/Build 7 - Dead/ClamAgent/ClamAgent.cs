/* 
 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
 EITHER EXPRESSED OR IMPLIED,  INCLUDING BUT NOT LIMITED TO THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

 This code is free for both personal and commercial use, but you are
 expressly forbidden from selling.

 **************************************************************************
 PROJECT NAME:
   ClamAgent

 DESCRIPTION:
   This is an Exchange 2007 Transport Agent for virus scanning
   
 FILE NAME:
   ClamAgent.cs

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
using System.IO;
using System.Configuration;

using Microsoft.Exchange.Data.Transport.Routing;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Email;
using Microsoft.Exchange.Data.Mime;

using ClamBase;

namespace ClamAgent
{
    public sealed class ClamAgentFactory : RoutingAgentFactory
    {
        public override RoutingAgent CreateAgent(SmtpServer server)
        {
            return new ClamAgent(new Settings(), new ClamAVNet(new Settings()));
        }
    }
    public class ClamAgent : RoutingAgent
    {
        public ClamAgent(Settings settings, ClamAVNet clamav)
        {
            this._cawp = new ClamAgentWP(settings, clamav);
            this.OnSubmittedMessage += new SubmittedMessageEventHandler(ClamAgent_OnSubmittedMessage);
        }
        private ClamAgentWP _cawp;
        /// <summary>
        /// ClamAgent - Main event handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void ClamAgent_OnSubmittedMessage(SubmittedMessageEventSource source, QueuedMessageEventArgs e)
        {
            this._cawp.Scan(new TransportMsg(e.MailItem));
        }
    }
}
