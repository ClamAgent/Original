/* 
 THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
 EITHER EXPRESSED OR IMPLIED,  INCLUDING BUT NOT LIMITED TO THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

 This code is free for both personal and commercial use, but you are
 expressly forbidden from selling.

 **************************************************************************

 FILE NAME:
   SaveMail.cs

 DESCRIPTION:
   This is an Exchange 2007 Transport Agent 
   
 COPYRIGHT:
   Copyright (c) Zoltán Gömöri. 2008.
   All rights reserved.
   
 NOTES:
   The original version of this source code and the article that
   includes this source code can be found at:
     http://www.gomori.hu

 CREATED:
   2008.11.13
   
 LAST MODIFIED:
   2008.12.01
   
 VERSION:
   v1.0 Build 1 - Initial version
   v1.0 Build 2 - Change Access to the configuration file
                  Improoved file name handling
                  Logging
                  Error handling
   
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

namespace ClamAgent
{
    public sealed class ClamAgentFactory : RoutingAgentFactory
    {
        public override RoutingAgent CreateAgent(SmtpServer server)
        {
            return new ClamAgent(new Settings(), new ClamWin(new Settings()));
        }
    }
    public class ClamAgent : RoutingAgent
    {
        public ClamAgent(Settings settings, ClamWin clamwin)
        {
            this._settings = settings;
            this._clamwin = clamwin;
            this._log = new Logging(settings);
            this.OnSubmittedMessage += new SubmittedMessageEventHandler(ClamAgent_OnSubmittedMessage);
        }
        private Settings  _settings;
        private ClamWin _clamwin;
        private Logging _log;
        void ClamAgent_OnSubmittedMessage(SubmittedMessageEventSource source, QueuedMessageEventArgs e)
        {
            int i;
            bool IsInfected = false;
            bool IsDebugError = false;
            string AttGuid;
            string AttDir;
            string AttFullPath;
            string AttName;
            string WorkDir = "";
            string ScanResult;
            byte[] ScanResultBytes;
            string MsgGuid = Guid.NewGuid().ToString();
            Stream AttStream;
            ASCIIEncoding encoding = new ASCIIEncoding();


            // Check the working directory
            try
            {
                WorkDir = Path.GetFullPath(this._settings.WorkingDirectory);
            }
            catch (Exception err)
            {
                // handle the error, log it
                this._log.Write(err.Message);
            }
            if (WorkDir != "")
            {
                // Log start of the virus scan
                this._log.Write(MsgGuid + "," + e.MailItem.FromAddress.ToString() + "," + GetRecipientList(e.MailItem) + ",Starting the virus scan");
                if (e.MailItem.Message.Attachments.Count == 0)
                {
                    this._log.Write(MsgGuid + "," + "Nothing to do");
                }
                for (i = 0; i < e.MailItem.Message.Attachments.Count; i++)
                {
                    // Generate attachment Guid
                    AttGuid = Guid.NewGuid().ToString();
                    this._log.Write(MsgGuid + "," + AttGuid + "," + e.MailItem.Message.Attachments[i].FileName + ",Starting the virus scan");
                    try
                    {
                        // Build the Attachment's work directory
                        AttDir = Path.Combine(WorkDir, AttGuid);
                        // Create the Directory
                        Directory.CreateDirectory(AttDir);
                        this._log.Write(MsgGuid + "," + AttGuid + "," + AttDir + ",Directory created");
                        // Get the attachment filename from the e-mail
                        AttName = e.MailItem.Message.Attachments[i].FileName;
                        // Replace the Invalid characters in the filename
                        AttName = ExtendedPath.EscapeFile(AttName);
                        if (AttName == String.Empty)
                        {
                            AttName = "temp.bin";
                        }
                        // Generate the full path of the attachment file
                        AttFullPath = Path.Combine(AttDir, AttName);
                        // Save the Attachment file
                        ((ExStream)e.MailItem.Message.Attachments[i].GetContentReadStream()).SaveToFile(AttFullPath);
                        this._log.Write(MsgGuid + "," + AttGuid + "," + AttFullPath + ",File Saved");
                        // SaveToFile(e.MailItem.Message.Attachments[i], AttFullPath, 8192);
                        // Virus scan
                        this._clamwin.ScanFolder = AttDir;
                        ScanResult = this._clamwin.Execute();
                        // If the saved attachment file disapeared, we need to replace the attachment
                        // with the ClamWin's log
                        if (!File.Exists(AttFullPath))
                        {
                            this._log.Write(MsgGuid + "," + AttGuid + ",Virus found");
                            // add a .txt extension to the original attachment's filename
                            e.MailItem.Message.Attachments[i].FileName += ".txt";
                            // e.MailItem.Message.Attachments[i].ContentType = 
                            // Get the content stream
                            AttStream = e.MailItem.Message.Attachments[i].GetContentWriteStream();
                            AttStream.Position = 0;
                            ScanResultBytes = encoding.GetBytes(ScanResult);
                            // Overwrite it
                            AttStream.Write(ScanResultBytes, 0, ScanResultBytes.Length);
                            AttStream.SetLength(ScanResultBytes.Length);
                            // set that the message was infected
                            IsInfected = true;
                        }
                        else
                        {
                            this._log.Write(MsgGuid + "," + AttGuid + ",Virus not found");
                        }
                        // delete the directory and the file
                        Directory.Delete(AttDir, true);
                    }
                    catch (Exception err)
                    {
                        // Error logging
                        this._log.Write(err.Message);
                        IsDebugError = true;
                    }
                }
                if (IsDebugError)
                {
                    this._log.SaveMail(MsgGuid, e.MailItem);
                }
                this._log.Write(MsgGuid + ",Virus scan finished");
            }
            // Prepend the subject with the infection string
            if (IsInfected && this._settings.PrependSubject != String.Empty)
            {
                e.MailItem.Message.Subject = this._settings.PrependSubject + e.MailItem.Message.Subject;
            }
        }
        void SaveToFile(Attachment att, string filename, int bufflen)
        {
            int i;
            FileStream AttFile = File.Create(filename);
            Stream AttStream = att.GetContentReadStream();
            byte[] buffer = new byte[bufflen];
            for (i = bufflen; i == bufflen; )
            {
                i = AttStream.Read(buffer, 0, i);
                AttFile.Write(buffer, 0, i);
            }
            AttFile.Close();
            AttStream.Close();
        }
        void SaveToFile(MailItem mail, string filename, int bufflen)
        {
            int i;
            FileStream MsgFile = File.Create(filename);
            Stream MsgStream = mail.GetMimeReadStream();
            byte[] buffer = new byte[bufflen];
            for (i = bufflen; i == bufflen; )
            {
                i = MsgStream.Read(buffer, 0, i);
                MsgFile.Write(buffer, 0, i);
            }
            MsgFile.Close();
            MsgStream.Close();
        }
        private string GetRecipientList(MailItem mail)
        {
            string retvalue = "";
            foreach (EnvelopeRecipient recip in mail.Recipients)
            {
                retvalue += recip.Address.ToString() + ";";
            }
            return retvalue;
        }
    }
}
