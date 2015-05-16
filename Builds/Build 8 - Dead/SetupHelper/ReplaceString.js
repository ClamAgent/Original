var args = WScript.Arguments
var FSO = new ActiveXObject("Scripting.FileSystemObject");
var src = FSO.OpenTextFile(args(0));
var dst = FSO.CreateTextFile(args(0) + ".tmp");
var tmpline;
var re = new RegExp("%" + args(1) + "%","ig");
while(!src.AtEndOfStream)
{
    tmpline = src.ReadLine();
    tmpline = tmpline.replace(re, args(2));
    dst.WriteLine(tmpline);
}
src.Close();
dst.Close();
FSO.DeleteFile(args(0));
FSO.MoveFile(args(0) + ".tmp", args(0));