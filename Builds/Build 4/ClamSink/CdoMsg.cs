using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using ClamBase;
using ClamAgent.Wrappers;

namespace ClamSink
{
    public class CdoMsg : MessageBase
    {
        private CDO.Message _basemsg;
        List<BodyPartBase> _attachments;

        public CdoMsg(CDO.Message msg)
        {
            int i;
            this._basemsg = msg;
            this._attachments = new List<BodyPartBase>();
            for (i = 1; i < msg.Attachments.Count + 1; i++)
            {
                this._attachments.Add(new CdoAttachment(msg.Attachments[i]));
            }
        }
        public override void SaveToFile(string filename)
        {
            this._basemsg.GetStream().SaveToFile(filename, ADODB.SaveOptionsEnum.adSaveCreateOverWrite);
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
                return Regex.Match(
                    (string)this._basemsg.EnvelopeFields["http://schemas.microsoft.com/cdo/smtpenvelope/senderemailaddress"].Value,
                    @"[\w\-.]*@[a-z0-9\-.]*",
                    RegexOptions.IgnoreCase).Value;
            }
        }

        public override string[] EnvelopeRecipientAddresses
        {
            get
            {
                MatchCollection RegExMatches = Regex.Matches(
                    (string)this._basemsg.EnvelopeFields["http://schemas.microsoft.com/cdo/smtpenvelope/recipientlist"].Value,
                    @"smtp:([\w\-.]*@[a-z0-9\-.]*)",
                    RegexOptions.IgnoreCase);
                string[] retarr = new string[RegExMatches.Count];
                int i;
                for (i = 0; i < RegExMatches.Count; i++)
                {
                    retarr[i] = RegExMatches[i].Groups[1].Value;
                }
                return retarr;
            }
        }

        public override string Subject
        {
            get
            {
                return this._basemsg.Subject;
            }
            set
            {
                this._basemsg.Subject = value;
            }
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
                /*
                Dictionary<string, string> ContentDisposition = new Dictionary<string, string>();
                int i;
                string[] SplitedHeader = ((string)this._baseatt.Fields["urn:schemas:mailheader:content-disposition"].Value).Split(';');
                string[] SplitedField;
                for (i = 0; i < SplitedHeader.Length; i++)
                {
                    SplitedField = this.QuotedSplit(SplitedHeader[i], '=');
                    if(
                }
                this._baseatt.Fields["FileName"].Value = value;
                this._baseatt.Fields.Update();
                */
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
        /*
        private string[] QuotedSplit(string src, char separator)
        {
            int i;
            int j=0;
            char[] srcarr = src.ToCharArray();
            bool InQuote = false;
            List<string> retarr = new List<string>();
            retarr.Add("");
            for(i = 0; i < srcarr.Length; i++)
            {
                if(srcarr[i] == separator)
                {
                    j++;
                    retarr.Add("");
                }
                else
                {
                    if(srcarr[i] == '\"')
                    {
                        InQuote=!InQuote;
                    }
                    else
                    {
                        retarr[j]+=srcarr[i];
                    }
                }
            }
            return retarr.ToArray();
        }
        */
    }
}
