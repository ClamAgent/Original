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
            get { throw new NotImplementedException(); }
        }

        public override bool CanSeek
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanWrite
        {
            get { throw new NotImplementedException(); }
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
