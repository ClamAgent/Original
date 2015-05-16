using System;
using System.Collections.Generic;
using System.Text;
using ClamSink;
using ClamBase;

namespace Test6
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageBase Msg = new CdoMsg(LoadMsgFile("c:\\data\\test\\test1.eml"));
            Msg.Attachments[0].FileName = "árvíztűrő tükörfúrógép.txt";
            Msg.SaveToFile("c:\\data\\test\\test3.eml");
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
