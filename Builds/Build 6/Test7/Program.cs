using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Test7
{
    class Program
    {
        static void Main(string[] args)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string ScanResult = "Test text: ez akkor most jó?";
            CdoMsg msg = new CdoMsg(LoadMsgFile("c:\\Data\\Test\\Test1.eml"));

            // add a .txt extension to the original attachment's filename
            msg.Attachments[0].FileName += ".txt";
            // Get the content stream
            Stream AttStream = msg.Attachments[0].GetContentWriteStream();
            byte[] ScanResultBytes = encoding.GetBytes(ScanResult);
            // Overwrite it
            AttStream.Write(ScanResultBytes, 0, ScanResultBytes.Length);
            AttStream.SetLength(ScanResultBytes.Length);
            AttStream.Flush();
            AttStream.Close();

            msg.Attachments[0].ContentType = "text/plain";
            msg.Commit();
            msg.SaveToFile("c:\\data\\test\\test7_result.eml");
        }
        public static CDO.Message LoadMsgFile(string FileName)
        {
            CDO.Message Msg = new CDO.MessageClass();
            ADODB.Stream MsgStream = new ADODB.Stream();
            MsgStream.Type = ADODB.StreamTypeEnum.adTypeBinary;
            MsgStream.Open((object)System.Type.Missing,
                ADODB.ConnectModeEnum.adModeUnknown,
                ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified,
                "", "");
            MsgStream.LoadFromFile(FileName);
            Msg.DataSource.OpenObject(MsgStream, "_Stream");
            return Msg;
        }
    }
}
