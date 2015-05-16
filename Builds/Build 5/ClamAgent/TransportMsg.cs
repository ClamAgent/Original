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
   TransportMsg.cs

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
   2008.12.12
   
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

 TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using ClamBase;

using Microsoft.Exchange.Data.Transport.Routing;
using Microsoft.Exchange.Data.Transport;
using Microsoft.Exchange.Data.Transport.Email;
using Microsoft.Exchange.Data.Mime;

namespace ClamAgent
{
    /// <summary>
    /// Wrapper for the Microsoft.Exchange.Transport.MailItem
    /// </summary>
    public class TransportMsg : MessageBase
    {
        MailItem _basemsg;
        List<BodyPartBase> _attachments;

        public TransportMsg(MailItem msg)
        {
            int i;
            this._basemsg = msg;
            this._attachments = new List<BodyPartBase>();
            for (i = 0; i < msg.Message.Attachments.Count; i++)
            {
                this._attachments.Add(new TransportAttachment(msg.Message.Attachments[i]));
            }
        }
        public override void SaveToFile(string filename)
        {
            ExStream.SaveToFile(this._basemsg.GetMimeReadStream(), filename);
        }

        public override List<BodyPartBase> Attachments
        {
            get
            {
                return this._attachments;
            }
        }

        public override string EnvelopeFromAddress
        {
            get
            {
                return this._basemsg.FromAddress.ToString();
            }
        }

        public override string[] EnvelopeRecipientAddresses
        {
            get
            {
                int i;
                List<string> AddressList = new List<string>();
                for (i = 0; i < this._basemsg.Recipients.Count; i++)
                {
                    AddressList.Add(this._basemsg.Recipients[i].ToString());
                }
                return AddressList.ToArray();
            }
        }

        public override string Subject
        {
            get
            {
                return this._basemsg.Message.Subject;
            }
            set
            {
                this._basemsg.Message.Subject = value;
            }
        }
        /// <summary>
        /// Commit - Dummy function. The Transport message doesn't require to
        /// write back the changes
        /// </summary>
        public override void Commit()
        {
        }
    }

    public class TransportAttachment : BodyPartBase
    {
        Attachment _baseatt;
        public TransportAttachment(Attachment att)
        {
            this._baseatt = att;
        }
        public override void SaveToFile(string filename)
        {
            ExStream.SaveToFile(this._baseatt.GetContentReadStream(), filename);
        }

        public override string FileName
        {
            get
            {
                return this._baseatt.FileName;
            }
            set
            {
                this._baseatt.FileName = value;
            }
        }

        public override string ContentType
        {
            get
            {
                return this._baseatt.ContentType;
            }
            set
            {
                this._baseatt.ContentType = value;
            }
        }

        public override System.IO.Stream GetContentReadStream()
        {
            return this._baseatt.GetContentReadStream();
        }

        public override System.IO.Stream GetContentWriteStream()
        {
            return this._baseatt.GetContentWriteStream();
        }
    }
}
