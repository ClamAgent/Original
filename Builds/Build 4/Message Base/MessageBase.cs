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
   MessageBase.cs

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
   2008.12.09
   
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

 TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace ClamBase
{
    /// <summary>
    /// Abstract base class for the transport e-mail messages
    /// </summary>
    public abstract class MessageBase
    {
        public abstract void SaveToFile(string filename);
        // public abstract BodyPartBase[] BodyParts { get; set; }
        public abstract List<BodyPartBase> Attachments { get; }
        public abstract string EnvelopeFromAddress { get; }
        public abstract string[] EnvelopeRecipientAddresses { get; }
        public abstract string Subject { get; set; }
        // public abstract string HtmlBody { get; set; }
        // public abstract string TextBody { get; set; }
        public abstract void Commit();
    }
    public abstract class BodyPartBase
    {
        public abstract void SaveToFile(string filename);
        public abstract string FileName { get; set; }
        public abstract string ContentType { get; set; }
        public abstract Stream GetContentReadStream();
        public abstract Stream GetContentWriteStream();
    }
    /*
    public abstract class FieldsBase
    {
    }
    */
}
