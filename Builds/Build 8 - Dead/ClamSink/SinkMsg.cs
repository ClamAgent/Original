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
   SinkMsg.cs

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

using ClamBase;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Exchange.Transport.EventInterop;
using Microsoft.Exchange.Transport.EventWrappers;
using System.Text.RegularExpressions;
using ClamAgent.Wrappers;


namespace ClamSink
{
    class SinkMsg : MessageBase
    {
        private Message _basemsg;
        private CDO.Message _basecdomsg;
        List<BodyPartBase> _attachments;
        public SinkMsg(Message msg)
        {
            int i;
            // Save the original message
            this._basemsg = msg;
            // Convert the original transport message to CDO Message
            this._basecdomsg = this.MsgToCDOMsg(msg);
            // Convert the attachments
            this._attachments = new List<BodyPartBase>();
            for (i = 1; i < this._basecdomsg.Attachments.Count + 1; i++)
            {
                this._attachments.Add(new CdoAttachment(this._basecdomsg.Attachments[i]));
            }
        }
        public override void SaveToFile(string filename)
        {
            this._basecdomsg.GetStream().SaveToFile(filename, ADODB.SaveOptionsEnum.adSaveCreateOverWrite);
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
                return this._basemsg.SenderAddressSMTP;
            }
        }

        public override string[] EnvelopeRecipientAddresses
        {
            get
            {
                List<string> retarr = new List<string>();
                foreach (Recip MsgRecip in this._basemsg.Recips)
                {
                    if (MsgRecip.SMTPAddress != null)
                    {
                        retarr.Add(MsgRecip.SMTPAddress);
                    }
                }
                return retarr.ToArray();
            }
        }

        public override string Subject
        {
            get
            {
                return this._basecdomsg.Subject;
            }
            set
            {
                this._basecdomsg.Subject = value;
            }
        }

        public override void Commit()
        {
            uint MsgBytesWritten;
            byte[] MsgContent = CDOMsgToByteArr(this._basecdomsg);
            // Write back the message content
            this._basemsg.WriteContent(0, (uint)MsgContent.Length, out MsgBytesWritten, MsgContent);
            // Set the new content size
            this._basemsg.SetContentSize(MsgBytesWritten);
        }
        /// <summary>
        /// Converts the Exchange wrapper's Message object to a CDO.Message object
        /// </summary>
        /// <param name="Msg">Message object</param>
        /// <returns>CDO.Message object</returns>
        private CDO.Message MsgToCDOMsg(Message Msg)
        {
            // Create a CDO.Message object
            CDO.Message cdoMsg = new CDO.MessageClass();
            // Create the underlaying ADODB.Stream for the CDO.Message
            ADODB.Stream MsgStream = new ADODB.Stream();
            // Set the stream to binary
            MsgStream.Type = ADODB.StreamTypeEnum.adTypeBinary;
            // Open the stream
            MsgStream.Open(Missing.Value,
                ADODB.ConnectModeEnum.adModeUnknown,
                ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified,
                "", "");
            // Write the contents of the Message to the stream
            MsgStream.Write(Msg.ReadContent(0, Msg.GetContentSize()));
            MsgStream.Position = 0;
            // bind the stream to the CDO.Message object
            cdoMsg.DataSource.OpenObject(MsgStream, "_Stream");
            return cdoMsg;
        }
        /// <summary>
        /// Convert a CDO.Message's MIME stream to a byte array
        /// </summary>
        /// <param name="CdoMsg">The CDO.Message object</param>
        /// <returns>Byte array containing the MIME stream</returns>
        private byte[] CDOMsgToByteArr(CDO.Message CdoMsg)
        {
            // Get the ADODB stream from the CDO.Message
            ADODB.Stream MsgStream = CdoMsg.GetStream();
            // Switch the stream to binary
            MsgStream.Type = ADODB.StreamTypeEnum.adTypeBinary;
            // Read the stream to a byte array and return it
            return (byte[])(MsgStream.Read((int)ADODB.StreamReadEnum.adReadAll));
        }
    }
    public class CdoAttachment : BodyPartBase
    {
        CDO.IBodyPart _baseatt;
        public CdoAttachment(CDO.IBodyPart att)
        {
            this._baseatt = att;
        }
        public override void SaveToFile(string filename)
        {
            this._baseatt.GetDecodedContentStream().SaveToFile(filename, ADODB.SaveOptionsEnum.adSaveCreateOverWrite);
        }

        public override string FileName
        {
            get
            {
                return this._baseatt.FileName;
            }
            set
            {
                string EncodedValue = this.EncodeMsgHeaderField(value);
                this._baseatt.Fields["urn:schemas:mailheader:content-disposition"].Value =
                    Regex.Replace(
                        (string)this._baseatt.Fields["urn:schemas:mailheader:content-disposition"].Value,
                        "filename=\".*\"",
                        "filename=\"" + EncodedValue + "\"",
                        RegexOptions.IgnoreCase);
                this._baseatt.Fields["urn:schemas:mailheader:content-type"].Value =
                    Regex.Replace(
                        (string)this._baseatt.Fields["urn:schemas:mailheader:content-type"].Value,
                        "name=\".*\"",
                        "name=\"" + EncodedValue + "\"",
                        RegexOptions.IgnoreCase);
                this._baseatt.Fields["urn:schemas:mailheader:content-description"].Value = EncodedValue;
                this._baseatt.Fields.Update();
            }
        }

        public override string ContentType
        {
            get
            {
                return (string)this._baseatt.Fields["urn:schemas:mailheader:content-type"].Value;
            }
            set
            {
                this._baseatt.Fields["urn:schemas:mailheader:content-type"].Value = value;
                this._baseatt.Fields.Update();
            }
        }

        public override System.IO.Stream GetContentReadStream()
        {
            return new AdoStreamWrapper(this._baseatt.GetDecodedContentStream());
        }

        public override System.IO.Stream GetContentWriteStream()
        {
            return new AdoStreamWrapper(this._baseatt.GetDecodedContentStream());
        }
        private string DecodeMsgHeaderField(string field)
        {
            CDO.Message Msg = new CDO.Message();
            Msg.Fields["urn:schemas:mailheader:subject"].Value = field;
            Msg.Fields.Update();
            return Msg.Subject;
        }
        private string EncodeMsgHeaderField(string field)
        {
            CDO.Message Msg = new CDO.Message();
            Msg.Subject = field;
            return (string)Msg.Fields["urn:schemas:mailheader:subject"].Value;
        }
    }
}
