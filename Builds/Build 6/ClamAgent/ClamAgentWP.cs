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
   ClamAgentWP.cs

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
using System.IO;

namespace ClamAgent
{
    public class ClamAgentWP
    {
        // settings loaded from the app.config file
        Settings _settings;
        // clamscan.exe wrapper
        ClamWin _clamwin;
        // file logging facility
        Logging _log;
        public ClamAgentWP(Settings settings, ClamWin clamwin)
        {
            this._settings = settings;
            this._clamwin = clamwin;
            this._log = new Logging(settings);
        }
        public void Scan(MessageBase msg)
        {
            // MessageBase Msg = new TransportMsg(e.MailItem);
            int i;
            // Indicator of the infected message
            bool IsInfected = false;
            // Indicator if anything changed in the message
            bool IsMsgChanged = false;
            // It will be true if an unexpected exception happened during the execution
            // in this case, the modifie message will be saved to the debug directory
            bool IsDebugError = false;
            // Guid generated for the actual attachment
            string AttGuid;
            // Directory name where the actual attachment will be saved.
            // The value is the WorkingDirectory from the app.settings file + the AttGuid
            string AttDir;
            // Full file name of the saved attachment AttDir + the FileName property of the attachment object
            string AttFullPath;
            // The attachment's FileName property. The invalid characters converted to escaped version
            string AttName;
            // The working directory
            string WorkDir = "";
            // Result of the virus scanning. Combination of the clamscan's Standard Output and Standard Error
            string ScanResult;
            // Scanresult string converted to byte array (it must be done because we have to write it back
            // to a binary stream
            byte[] ScanResultBytes;
            // Guid generated for the actual message object
            string MsgGuid = Guid.NewGuid().ToString();
            // The actual attachment's content stream
            Stream AttStream;
            ASCIIEncoding encoding = new ASCIIEncoding();

            // Check the working directory
            try
            {
                // check if the WorkingDirectory determined by the app.settings is a legal path string
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
                this._log.Write(MsgGuid + "," + msg.EnvelopeFromAddress + "," + String.Join(";", msg.EnvelopeRecipientAddresses) + ",Starting the virus scan");
                // If the message has no attachments, log that nothing to do
                if (msg.Attachments.Count == 0)
                {
                    this._log.Write(MsgGuid + "," + "Nothing to do");
                }
                // Iterate through all of the attachments
                for (i = 0; i < msg.Attachments.Count; i++)
                {
                    // Generate attachment Guid
                    AttGuid = Guid.NewGuid().ToString();
                    this._log.Write(MsgGuid + "," + AttGuid + "," + msg.Attachments[i].FileName + ",Starting the virus scan");
                    try
                    {
                        // Build the Attachment's work directory
                        AttDir = Path.Combine(WorkDir, AttGuid);
                        // Create the Directory
                        Directory.CreateDirectory(AttDir);
                        this._log.Write(MsgGuid + "," + AttGuid + "," + AttDir + ",Directory created");
                        // Get the attachment filename from the e-mail
                        AttName = msg.Attachments[i].FileName;
                        // Replace the Invalid characters in the filename
                        AttName = ExtendedPath.EscapeFile(AttName);
                        if (AttName == String.Empty)
                        {
                            AttName = "temp.bin";
                        }
                        // Generate the full path of the attachment file
                        AttFullPath = Path.Combine(AttDir, AttName);
                        // Save the Attachment file
                        msg.Attachments[i].SaveToFile(AttFullPath);
                        this._log.Write(MsgGuid + "," + AttGuid + "," + AttFullPath + ",File Saved");
                        // Virus scan
                        this._clamwin.ScanFolder = AttDir;
                        ScanResult = this._clamwin.Execute();
                        // If the saved attachment file disapeared, we need to replace the attachment
                        // with the ClamWin's log
                        if (!File.Exists(AttFullPath))
                        {
                            this._log.Write(MsgGuid + "," + AttGuid + ",Virus found");
                            // add a .txt extension to the original attachment's filename
                            msg.Attachments[i].FileName += ".txt";
                            // Set the content type
                            msg.Attachments[i].ContentType = "text/plain";
                            // Get the content stream
                            AttStream = msg.Attachments[i].GetContentWriteStream();
                            ScanResultBytes = encoding.GetBytes(ScanResult);
                            // Overwrite it
                            AttStream.Write(ScanResultBytes, 0, ScanResultBytes.Length);
                            // set the stream length (it must be put in a try/catch block because the
                            // cdo message requires it, but the transport message doesn't support it)
                            try
                            {
                                AttStream.SetLength(ScanResultBytes.Length);
                            }
                            catch (Exception err) {}
                            // Flush the stream
                            AttStream.Flush();
                            // Close the Stream
                            AttStream.Close();

                            // set that the message was infected
                            IsInfected = true;
                            // set that we changed information in the message
                            IsMsgChanged = true;
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
                if (IsDebugError || this._settings.DebugAllMessage)
                {
                    msg.SaveToFile(Path.Combine(this._settings.DebugPath, MsgGuid + ".eml"));
                }
                this._log.Write(MsgGuid + ",Virus scan finished");
            }
            // Prepend the subject with the infection string
            if (IsInfected && this._settings.PrependSubject != String.Empty)
            {
                msg.Subject = this._settings.PrependSubject + msg.Subject;
                IsMsgChanged = true;
            }
            if (IsMsgChanged)
            {
                msg.Commit();
            }
        }
    }
}
