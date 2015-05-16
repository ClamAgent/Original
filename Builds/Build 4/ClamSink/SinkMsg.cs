using System;
using System.Collections.Generic;
using System.Text;

using ClamBase;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Exchange.Transport.EventInterop;
using Microsoft.Exchange.Transport.EventWrappers;


namespace ClamSink
{
    class SinkMsg : MessageBase
    {
        private Message _basemsg;
        private CDO.Message _basecdomsg;
        List<BodyPartBase> _attachments;
        public SinkMsg(Message Msg)
        {
            this._basemsg = Msg;
            this._basecdomsg = this.MsgToCDOMsg(Msg);
        }
        public override void SaveToFile(string filename)
        {
            throw new NotImplementedException();
        }

        public override List<BodyPartBase> Attachments
        {
            get { throw new NotImplementedException(); }
        }

        public override string EnvelopeFromAddress
        {
            get { throw new NotImplementedException(); }
        }

        public override string[] EnvelopeRecipientAddresses
        {
            get { throw new NotImplementedException(); }
        }

        public override string Subject
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
    }

    class SinkAttachment : BodyPartBase
    {

        public override void SaveToFile(string filename)
        {
            throw new NotImplementedException();
        }

        public override string FileName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string ContentType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override System.IO.Stream GetContentReadStream()
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream GetContentWriteStream()
        {
            throw new NotImplementedException();
        }
    }
}
