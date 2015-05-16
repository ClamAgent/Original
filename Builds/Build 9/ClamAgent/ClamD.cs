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
   ClamWD.cs

 COPYRIGHT:
   Copyright (c) Zoltán Gömöri. 2010.
   All rights reserved.
   
 NOTES:
   The original version of this source code, the compiled binaries, and
   the documentation be found at:
     http://www.clamagent.org

 CREATED:
   2010.05.20
   
 LAST MODIFIED:
   2010.07.16
   
 VERSION:
   v1.0 Build 8 - Initial version
   v1.0 Build 9 - Bug fix. Modified the clamd response's end of line handling (the original release classified everithing as virus

TO DO:

 **************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClamAgent
{
    public class ClamD
    {
        public ClamD(Settings settings)
        {
            this._server = settings.ClamD_Address;
            this._port = settings.ClamD_Port;
        }
        private string _server;
        private int _port;

        private ClamdResult _Send(string command)
        {
            return this._Send(command, null, 1024);
        }

        private ClamdResult _Send(string command, Stream stream)
        {
            return this._Send(command, stream, 1024);
        }
        private ClamdResult _Send(string command, Stream stream, int buffsize)
        {
            int BytesRead;
            byte[] buffer;
            int responselength;
            ClamdResult retvalue = new ClamdResult(); 
            try
            {
                // Open TCP Connection to ClamD
                TcpClient Clamd = new TcpClient(this._server, this._port);
                NetworkStream ClamdStream = Clamd.GetStream();
                // Convert command to byte array
                buffer = Encoding.ASCII.GetBytes("z" + command + "\0");
                // Send command
                ClamdStream.Write(buffer, 0, buffer.Length);
                // If the stream is not null, the command has additional data to send
                if (stream != null)
                {
                    // Create a binary reader on the stream
                    BinaryReader br = new BinaryReader(stream);
                    // Allocate the send buffer
                    buffer = new byte[buffsize + 4];
                    BytesRead = buffsize;
                    for (; BytesRead == buffsize; )
                    {
                        // Load a chunk of data to the buffer (the first four bytes stores the chunk size)
                        BytesRead = br.Read(buffer, 4, buffsize);
                        // Put the chunk size into the buffer in Big Endian order
                        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BytesRead)).CopyTo(buffer, 0);
                        // Send data
                        ClamdStream.Write(buffer, 0, BytesRead + 4);
                    }
                    // Load the closing chunk into the buffer (zero length chunk)
                    buffer = new byte[] { 0, 0, 0, 0 };
                    // Send data
                    ClamdStream.Write(buffer, 0, buffer.Length);
                }
                // Allocate a buffer for the return data
                buffer = new byte[buffsize];
                // Read the response
                responselength = ClamdStream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception err)
            {
                retvalue.Code = 3;
                retvalue.Description = err.Message;
                return retvalue;
            }
            // Convert the response data to string and return it
            retvalue.Description = Encoding.ASCII.GetString(buffer, 0, responselength);
            retvalue.Code = 0;
            return retvalue;
        }
        public ClamdResult ScanStream(Stream stream)
        {
            ClamdResult retvalue = _Send("INSTREAM", stream);
            // If the scanning communication procedure finished without error
            // we have to process the return string to find out if a virus found
            // or any trouble happend with the clamd
            if (retvalue.Code == 0)
            {
                retvalue.Description = retvalue.Description.Split('\x0')[0];
                if (retvalue.Description.StartsWith("stream: ", StringComparison.OrdinalIgnoreCase))
                {
                    retvalue.Code = (retvalue.Description == "stream: OK") ? 0 : 1;
                    retvalue.Description = retvalue.Description.Remove(0, 8);
                }
                else
                {
                    retvalue.Code = 2;
                }
            }
            return retvalue;
        }
        /*
        public void Scan(string FileName)
        {
            Console.WriteLine(this._Send("SCAN " + FileName));
        }
         */
    }
    public class ClamdResult
    {
        public ClamdResult()
        {
        }
        public int Code;
        public string Description;
    }
}
