using System;
using System.Collections.Generic;
using System.Text;

// using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Test8
{
    class Program
    {
        static void Main(string[] args)
        {
            ClamD clam = new ClamD("10.1.1.201", 3310);
            // clam.ScanStream(new BinaryReader(File.OpenRead("c:\\data\\CRM_dijbekero.no")));
            // clam.Scan("c:\\inst\\Microsoft Sysinternals NewSID v4.10\\newsid.exe");
            // clam.Scan("c:\\inst\\eicar.com");
            Console.WriteLine(clam.ScanStream(File.OpenRead("c:\\DATA\\virus test\\eicar.com")));
//            Console.WriteLine(clam.ScanStream(File.OpenRead("c:\\inst\\eicar.com")));
//            Console.WriteLine(clam.ScanStream(File.OpenRead("c:\\data\\CRM_dijbekero.no")));
            /*
            string ClamConfigDir;
            string ClamDConfFile;
            int port;
            ClamConf clamdconf;
            ClamConfigDir = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\ClamAV", "ConfigDir", null);
            if (ClamConfigDir != null)
            {
                ClamDConfFile = Path.Combine(ClamConfigDir, "clamd.conf");
                clamdconf = new ClamConf(ClamDConfFile);
                port = Int32.Parse(clamdconf.Read("TCPSocket"));
            }
             */
        }
    }
    public class ClamD
    {
        public ClamD(string server, int port)
        {
            this._server = server;
            this._port = port;
        }
        private string _server;
        private int _port;
        
        private string _Send(string command)
        {
            return this._Send(command, null, 1024);
        }

        private string _Send(string command, Stream stream)
        {
            return this._Send(command, stream, 1024);
        }

        private string _Send(string command, Stream stream, int buffsize)
        {
            int BytesRead;
            byte[] buffer;
            int responselength;
            // Open TCP Connection to ClamD
            TcpClient Clamd = new TcpClient(this._server, this._port);
            NetworkStream ClamdStream = Clamd.GetStream();
            // Convert command to byte array
            buffer = Encoding.ASCII.GetBytes("z" + command +"\0");
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
            // Convert the response data to string and return it
            return Encoding.ASCII.GetString(buffer, 0, responselength);
        }

        public string ScanStream(Stream stream)
        {
            return _Send("INSTREAM", stream);
        }


        /*
        public string ScanStream(Stream scanstream)
        {
            int chunklength = 1024;
            int BytesRead;
            byte[] writebuff;
            byte[] readbuff;
            int responselength;
            BinaryReader br = new BinaryReader(scanstream);
            TcpClient clamd;
            clamd = new TcpClient(this._server, this._port);
            writebuff = Encoding.ASCII.GetBytes("zINSTREAM\0");
            NetworkStream stream = clamd.GetStream();
            stream.Write(writebuff, 0, writebuff.Length);
            writebuff = new byte[chunklength + 4];
            BytesRead = chunklength;
            for (; BytesRead == chunklength; )
            {
                BytesRead = br.Read(writebuff, 4, chunklength);
                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(BytesRead)).CopyTo(writebuff, 0);
                stream.Write(writebuff, 0, BytesRead + 4);
            }
            writebuff = new byte[] { 0, 0, 0, 0 };
            stream.Write(writebuff, 0, writebuff.Length);
            readbuff = new byte[1024];
            responselength = stream.Read(readbuff, 0, readbuff.Length);
            return Encoding.ASCII.GetString(readbuff, 0, responselength);
        }
         */
        public void Scan(string FileName)
        {
            Console.WriteLine(this._Send("SCAN " + FileName));
        }
        /*
        public void Close()
        {
            this._Send("END");
        }
        
        private string _Send(string data)
        {
            byte[] writebuff;
            byte[] readbuff;
            int responselength;
            writebuff = Encoding.ASCII.GetBytes("n" + data + "\n");
            NetworkStream stream = this._clamd.GetStream();
            stream.Write(writebuff, 0, writebuff.Length);
            readbuff = new byte[8192];
            responselength = stream.Read(readbuff, 0, readbuff.Length);
            return Encoding.ASCII.GetString(readbuff, 0, responselength);
        }
         */
    }
    /*
    public class ClamConf
    {
        private Dictionary<string,string> _conf;
        public ClamConf(string ConfPath)
        {
            string buff;
            string key;
            string value;
            int keylength;
            StreamReader confreader = File.OpenText(ConfPath);
            for (buff = confreader.ReadLine(); !confreader.EndOfStream; buff = confreader.ReadLine())
            {
                keylength = buff.IndexOfAny(new char[] { ' ', '\t' });
                key = buff.Substring(0, keylength).Trim();
                value = buff.Substring(keylength, buff.Length - keylength).Trim();
                if (value.Substring(0, 1) == "\"" && value.Substring(value.Length, 1) == "\"")
                {
                    value = value.Substring(1, value.Length - 1);
                }
                this._conf.Add(key, value);
            }
        }
        public string Read(string key)
        {
            if (this._conf.ContainsKey(key))
            {
                return this._conf[key];
            }
            return null;
        }
    }
     */
}

/*
static void Connect(String server, String message) 
{
  try 
  {
    // Create a TcpClient.
    // Note, for this client to work you need to have a TcpServer 
    // connected to the same address as specified by the server, port
    // combination.
    Int32 port = 13000;
    TcpClient client = new TcpClient(server, port);

    // Translate the passed message into ASCII and store it as a Byte array.
    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);         

    // Get a client stream for reading and writing.
   //  Stream stream = client.GetStream();

    NetworkStream stream = client.GetStream();

    // Send the message to the connected TcpServer. 
    stream.Write(data, 0, data.Length);

    Console.WriteLine("Sent: {0}", message);         

    // Receive the TcpServer.response.

    // Buffer to store the response bytes.
    data = new Byte[256];

    // String to store the response ASCII representation.
    String responseData = String.Empty;

    // Read the first batch of the TcpServer response bytes.
    Int32 bytes = stream.Read(data, 0, data.Length);
    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
    Console.WriteLine("Received: {0}", responseData);         

    // Close everything.
    stream.Close();         
    client.Close();         
  } 
  catch (ArgumentNullException e) 
  {
    Console.WriteLine("ArgumentNullException: {0}", e);
  } 
  catch (SocketException e) 
  {
    Console.WriteLine("SocketException: {0}", e);
  }

  Console.WriteLine("\n Press Enter to continue...");
  Console.Read();
}
*/