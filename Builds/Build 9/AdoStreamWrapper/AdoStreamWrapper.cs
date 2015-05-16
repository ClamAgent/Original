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
   Settings.cs

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
   2010.07.16
   
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
   v1.0 Build 9 - Implemented the CanRead, CanSeek, CanWrite indicators
                  The BinaryReader uses the indicators. I couldn't determine the seeking
                  capability easily so the seeking will be disabled (not needed anyhow)
   
TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace ClamAgent.Wrappers
{
    public class AdoStreamWrapper: Stream
    {
        ADODB.Stream _basestream;
        public AdoStreamWrapper(ADODB.Stream basestream)
        {
            this._basestream = basestream;
            // Set the strea to binary
            this._basestream.Type = ADODB.StreamTypeEnum.adTypeBinary;
        }
        public override bool CanRead
        {
            get
            {
                // throw new NotImplementedException();
                // return (this._basestream.Mode & ADODB.ConnectModeEnum.adModeRead) != 0;
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                // throw new NotImplementedException();
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            { 
                // throw new NotImplementedException();
                // return (this._basestream.Mode & ADODB.ConnectModeEnum.adModeWrite) != 0;
                return true;
            }
        }

        public override void Flush()
        {
            this._basestream.Flush();
        }

        public override long Length
        {
            get
            {
                return this._basestream.Size;
            }
        }

        public override long Position
        {
            get
            {
                return this._basestream.Position;
            }
            set
            {
                this._basestream.Position = (int)value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int i;
            byte[] readbuffer;
            readbuffer = (byte[])this._basestream.Read(count);
            for (i = 0; i < readbuffer.Length; i++)
            {
                buffer[offset + i] = readbuffer[i];
            }
            return readbuffer.Length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this._basestream.Position = (int)offset;
            return this._basestream.Position;
        }

        public override void SetLength(long value)
        {
            this._basestream.Position = (int)value;
            this._basestream.SetEOS();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] WriteBuffer = new byte[count];
            int i;
            for(i=0;i<count;i++)
            {
                WriteBuffer[i] = buffer[offset + i];
            }
            this._basestream.Write(WriteBuffer);
        }

        public override void Close()
        {
            this._basestream.Close();
            base.Close();
        }
    }
}
