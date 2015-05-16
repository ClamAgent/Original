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
   2008.11.20
   
 VERSION:
   v1.0 Build 1 - Initial version
   
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
            return new ClamAgent(new ClamAgentSettings(), new ClamWin(new ClamWinConfig()));
        }
    }
    public class ClamAgent : RoutingAgent
    {
        public ClamAgent(ClamAgentSettings settings, ClamWin clamwin)
        {
            this._settings = settings;
            this._clamwin = clamwin;
            this.OnSubmittedMessage += new SubmittedMessageEventHandler(ClamAgent_OnSubmittedMessage);
        }
        private ClamAgentSettings  _settings;
        private ClamWin _clamwin;
        void ClamAgent_OnSubmittedMessage(SubmittedMessageEventSource source, QueuedMessageEventArgs e)
        {
            int i;
            bool IsInfected = false;
            string AttGuid;
            string AttDir;
            string AttFullPath;
            string AttName;
            string ScanResult;
            byte[] ScanResultBytes;
            Stream AttStream;
            ASCIIEncoding encoding = new ASCIIEncoding();
            for(i=0;i<e.MailItem.Message.Attachments.Count;i++)
            {
                // Guid generálás
                AttGuid = Guid.NewGuid().ToString();
                // Könyvtár generálás
                // ellenőrizni kell, hogy létre lehet-e hozni a könyvtárat
                AttDir = this._settings.WorkingDirectory + "\\" + AttGuid;
                Directory.CreateDirectory(AttDir);
                // Fájl mentés
                AttName = e.MailItem.Message.Attachments[i].FileName;
                // ellenőrizni kell, hogy az attachment fájl neve az valós fájl név-e
                AttFullPath = AttDir + "\\" + AttName;
                SaveToFile(e.MailItem.Message.Attachments[i], AttFullPath, 8192);
                // Vírus scan
                this._clamwin.ScanFolder = AttDir;
                ScanResult = this._clamwin.Execute();
                // Vírus scan eredmény olvasás
                // ha eltünt a fájl akkor a helyére .txt kiterjesztéssel bearkni a clamwin logját
                if (!File.Exists(AttFullPath))
                {
                    e.MailItem.Message.Attachments[i].FileName += ".txt";
                    // e.MailItem.Message.Attachments[i].ContentType = 
                    AttStream = e.MailItem.Message.Attachments[i].GetContentWriteStream();
                    AttStream.Position = 0;
                    ScanResultBytes = encoding.GetBytes(ScanResult);
                    AttStream.Write(ScanResultBytes, 0, ScanResultBytes.Length);
                    AttStream.SetLength(ScanResultBytes.Length);
                    IsInfected = true;
                }
                // törölni a könyvtárat és a file-t
                Directory.Delete(AttDir, true);
            }
            // valamint a subjectbe beírni, hogy infected
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
    }
    public class ClamAgentSettings
    {
        public ClamAgentSettings()
        {
            this.WorkingDirectory = ConfigurationManager.AppSettings["WorkingDirectory"];
            this.WorkingDirectory = ConfigurationManager.AppSettings["PrependSubject"];
        }
        public string WorkingDirectory;
        public string PrependSubject;
    }
}
