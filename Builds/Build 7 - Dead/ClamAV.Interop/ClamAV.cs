using System;
using System.Collections.Generic;
using System.Text;

namespace ClamAV.Interop
{
    public class ClamAV
    {
    }
}

// Error: Expected ; after member  in cl_limits but found int

public partial class NativeConstants
{

    /// CL_COUNT_PRECISION -> 4096
    public const int CL_COUNT_PRECISION = 4096;

    /// CL_CLEAN -> 0
    public const int CL_CLEAN = 0;

    /// CL_VIRUS -> 1
    public const int CL_VIRUS = 1;

    /// CL_SUCCESS -> CL_CLEAN
    public const int CL_SUCCESS = NativeConstants.CL_CLEAN;

    /// CL_BREAK -> 2
    public const int CL_BREAK = 2;

    /// CL_EMAXREC -> -100
    public const int CL_EMAXREC = -100;

    /// CL_EMAXSIZE -> -101
    public const int CL_EMAXSIZE = -101;

    /// CL_EMAXFILES -> -102
    public const int CL_EMAXFILES = -102;

    /// CL_ERAR -> -103
    public const int CL_ERAR = -103;

    /// CL_EZIP -> -104
    public const int CL_EZIP = -104;

    /// CL_EGZIP -> -105
    public const int CL_EGZIP = -105;

    /// CL_EBZIP -> -106
    public const int CL_EBZIP = -106;

    /// CL_EOLE2 -> -107
    public const int CL_EOLE2 = -107;

    /// CL_EMSCOMP -> -108
    public const int CL_EMSCOMP = -108;

    /// CL_EMSCAB -> -109
    public const int CL_EMSCAB = -109;

    /// CL_EACCES -> -110
    public const int CL_EACCES = -110;

    /// CL_ENULLARG -> -111
    public const int CL_ENULLARG = -111;

    /// CL_ETMPFILE -> -112
    public const int CL_ETMPFILE = -112;

    /// CL_EMEM -> -114
    public const int CL_EMEM = -114;

    /// CL_EOPEN -> -115
    public const int CL_EOPEN = -115;

    /// CL_EMALFDB -> -116
    public const int CL_EMALFDB = -116;

    /// CL_EPATSHORT -> -117
    public const int CL_EPATSHORT = -117;

    /// CL_ETMPDIR -> -118
    public const int CL_ETMPDIR = -118;

    /// CL_ECVD -> -119
    public const int CL_ECVD = -119;

    /// CL_ECVDEXTR -> -120
    public const int CL_ECVDEXTR = -120;

    /// CL_EMD5 -> -121
    public const int CL_EMD5 = -121;

    /// CL_EDSIG -> -122
    public const int CL_EDSIG = -122;

    /// CL_EIO -> -123
    public const int CL_EIO = -123;

    /// CL_EFORMAT -> -124
    public const int CL_EFORMAT = -124;

    /// CL_ESUPPORT -> -125
    public const int CL_ESUPPORT = -125;

    /// CL_EARJ -> -127
    public const int CL_EARJ = -127;

    /// CL_EUSERABORT -> -500
    public const int CL_EUSERABORT = -500;

    /// CL_DB_PHISHING -> 0x2
    public const int CL_DB_PHISHING = 2;

    /// CL_DB_ACONLY -> 0x4
    public const int CL_DB_ACONLY = 4;

    /// CL_DB_PHISHING_URLS -> 0x8
    public const int CL_DB_PHISHING_URLS = 8;

    /// CL_DB_PUA -> 0x10
    public const int CL_DB_PUA = 16;

    /// CL_DB_CVDNOTMP -> 0x20
    public const int CL_DB_CVDNOTMP = 32;

    /// CL_DB_OFFICIAL -> 0x40
    public const int CL_DB_OFFICIAL = 64;

    /// CL_DB_PUA_MODE -> 0x80
    public const int CL_DB_PUA_MODE = 128;

    /// CL_DB_PUA_INCLUDE -> 0x100
    public const int CL_DB_PUA_INCLUDE = 256;

    /// CL_DB_PUA_EXCLUDE -> 0x200
    public const int CL_DB_PUA_EXCLUDE = 512;

    /// CL_DB_STDOPT -> (CL_DB_PHISHING | CL_DB_PHISHING_URLS)
    public const int CL_DB_STDOPT = (NativeConstants.CL_DB_PHISHING | NativeConstants.CL_DB_PHISHING_URLS);

    /// CL_SCAN_RAW -> 0x0
    public const int CL_SCAN_RAW = 0;

    /// CL_SCAN_ARCHIVE -> 0x1
    public const int CL_SCAN_ARCHIVE = 1;

    /// CL_SCAN_MAIL -> 0x2
    public const int CL_SCAN_MAIL = 2;

    /// CL_SCAN_OLE2 -> 0x4
    public const int CL_SCAN_OLE2 = 4;

    /// CL_SCAN_BLOCKENCRYPTED -> 0x8
    public const int CL_SCAN_BLOCKENCRYPTED = 8;

    /// CL_SCAN_HTML -> 0x10
    public const int CL_SCAN_HTML = 16;

    /// CL_SCAN_PE -> 0x20
    public const int CL_SCAN_PE = 32;

    /// CL_SCAN_BLOCKBROKEN -> 0x40
    public const int CL_SCAN_BLOCKBROKEN = 64;

    /// CL_SCAN_MAILURL -> 0x80
    public const int CL_SCAN_MAILURL = 128;

    /// CL_SCAN_BLOCKMAX -> 0x100
    public const int CL_SCAN_BLOCKMAX = 256;

    /// CL_SCAN_ALGORITHMIC -> 0x200
    public const int CL_SCAN_ALGORITHMIC = 512;

    /// CL_SCAN_PHISHING_BLOCKSSL -> 0x800
    public const int CL_SCAN_PHISHING_BLOCKSSL = 2048;

    /// CL_SCAN_PHISHING_BLOCKCLOAK -> 0x1000
    public const int CL_SCAN_PHISHING_BLOCKCLOAK = 4096;

    /// CL_SCAN_ELF -> 0x2000
    public const int CL_SCAN_ELF = 8192;

    /// CL_SCAN_PDF -> 0x4000
    public const int CL_SCAN_PDF = 16384;

    /// CL_SCAN_STRUCTURED -> 0x8000
    public const int CL_SCAN_STRUCTURED = 32768;

    /// CL_SCAN_STRUCTURED_SSN_NORMAL -> 0x10000
    public const int CL_SCAN_STRUCTURED_SSN_NORMAL = 65536;

    /// CL_SCAN_STRUCTURED_SSN_STRIPPED -> 0x20000
    public const int CL_SCAN_STRUCTURED_SSN_STRIPPED = 131072;

    /// CL_SCAN_PARTIAL_MESSAGE -> 0x40000
    public const int CL_SCAN_PARTIAL_MESSAGE = 262144;

    /// CL_SCAN_HEURISTIC_PRECEDENCE -> 0x80000
    public const int CL_SCAN_HEURISTIC_PRECEDENCE = 524288;

    /// CL_SCAN_STDOPT -> (CL_SCAN_ARCHIVE | CL_SCAN_MAIL | CL_SCAN_OLE2 | CL_SCAN_PDF | CL_SCAN_HTML | CL_SCAN_PE | CL_SCAN_ALGORITHMIC | CL_SCAN_ELF)
    public const int CL_SCAN_STDOPT = (NativeConstants.CL_SCAN_ARCHIVE
                | (NativeConstants.CL_SCAN_MAIL
                | (NativeConstants.CL_SCAN_OLE2
                | (NativeConstants.CL_SCAN_PDF
                | (NativeConstants.CL_SCAN_HTML
                | (NativeConstants.CL_SCAN_PE
                | (NativeConstants.CL_SCAN_ALGORITHMIC | NativeConstants.CL_SCAN_ELF)))))));

    /// CL_RAW -> CL_SCAN_RAW
    public const int CL_RAW = NativeConstants.CL_SCAN_RAW;

    /// CL_ARCHIVE -> CL_SCAN_ARCHIVE
    public const int CL_ARCHIVE = NativeConstants.CL_SCAN_ARCHIVE;

    /// CL_MAIL -> CL_SCAN_MAIL
    public const int CL_MAIL = NativeConstants.CL_SCAN_MAIL;

    /// CL_OLE2 -> CL_SCAN_OLE2
    public const int CL_OLE2 = NativeConstants.CL_SCAN_OLE2;

    /// CL_ENCRYPTED -> CL_SCAN_BLOCKENCRYPTED
    public const int CL_ENCRYPTED = NativeConstants.CL_SCAN_BLOCKENCRYPTED;

    /// cl_node -> cl_engine
    /// Error generating expression: Value cl_engine is not resolved
    public const string cl_node = "cl_engine";

    /// cl_perror -> cl_strerror
    /// Error generating expression: Value cl_strerror is not resolved
    public const string cl_perror = "cl_strerror";
}

/// Return Type: int
///desc: int
///bytes: int
public delegate int cl_engine_callback(int desc, int bytes);

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct cl_engine
{

    /// unsigned int
    public uint refcount;

    /// unsigned short
    public ushort sdb;

    /// unsigned int
    public uint dboptions;

    /// void**
    public System.IntPtr root;

    /// void*
    public System.IntPtr md5_hdb;

    /// void*
    public System.IntPtr md5_mdb;

    /// void*
    public System.IntPtr md5_fp;

    /// void*
    public System.IntPtr zip_mlist;

    /// void*
    public System.IntPtr rar_mlist;

    /// void*
    public System.IntPtr whitelist_matcher;

    /// void*
    public System.IntPtr domainlist_matcher;

    /// void*
    public System.IntPtr phishcheck;

    /// void*
    public System.IntPtr dconf;

    /// void*
    public System.IntPtr ftypes;

    /// void*
    public System.IntPtr ignored;

    /// char*
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
    public string pua_cats;

    /// cl_engine_callback
    public cl_engine_callback AnonymousMember1;
}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct cl_stat
{

    /// char*
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
    public string dir;

    /// unsigned int
    public uint entries;

    /// stat*
    public System.IntPtr stattab;

    /// char**
    public System.IntPtr statdname;
}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct cl_cvd
{

    /// char*
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
    public string time;

    /// unsigned int
    public uint version;

    /// unsigned int
    public uint sigs;

    /// unsigned int
    public uint fl;

    /// char*
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
    public string md5;

    /// char*
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
    public string dsig;

    /// char*
    [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
    public string builder;

    /// unsigned int
    public uint stime;
}

public partial class NativeMethods
{

    /// Return Type: int
    ///desc: int
    ///virname: char**
    ///param2: unsigned int
    ///scanned: int*
    ///engine: cl_engine*
    ///limits: cl_limits*
    ///options: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_scandesc")]
    public static extern int cl_scandesc(int desc, ref System.IntPtr virname, uint param2, ref int scanned, ref cl_engine engine, System.IntPtr limits, uint options);


    /// Return Type: int
    ///filename: char*
    ///virname: char**
    ///param2: unsigned int
    ///scanned: int*
    ///engine: cl_engine*
    ///limits: cl_limits*
    ///options: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_scanfile")]
    public static extern int cl_scanfile([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string filename, ref System.IntPtr virname, uint param2, ref int scanned, ref cl_engine engine, System.IntPtr limits, uint options);


    /// Return Type: int
    ///path: char*
    ///engine: cl_engine**
    ///signo: unsigned int*
    ///options: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_load")]
    public static extern int cl_load([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string path, ref System.IntPtr engine, ref uint signo, uint options);


    /// Return Type: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_retdbdir")]
    public static extern System.IntPtr cl_retdbdir();


    /// Return Type: int
    ///engine: cl_engine*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_build")]
    public static extern int cl_build(ref cl_engine engine);


    /// Return Type: cl_engine*
    ///engine: cl_engine*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_dup")]
    public static extern System.IntPtr cl_dup(ref cl_engine engine);


    /// Return Type: void
    ///engine: cl_engine*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_free")]
    public static extern void cl_free(ref cl_engine engine);


    /// Return Type: cl_cvd*
    ///file: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_cvdhead")]
    public static extern System.IntPtr cl_cvdhead([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string file);


    /// Return Type: cl_cvd*
    ///head: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_cvdparse")]
    public static extern System.IntPtr cl_cvdparse([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string head);


    /// Return Type: int
    ///file: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_cvdverify")]
    public static extern int cl_cvdverify([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string file);


    /// Return Type: void
    ///cvd: cl_cvd*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_cvdfree")]
    public static extern void cl_cvdfree(ref cl_cvd cvd);


    /// Return Type: int
    ///dirname: char*
    ///dbstat: cl_stat*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_statinidir")]
    public static extern int cl_statinidir([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string dirname, ref cl_stat dbstat);


    /// Return Type: int
    ///dbstat: cl_stat*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_statchkdir")]
    public static extern int cl_statchkdir(ref cl_stat dbstat);


    /// Return Type: int
    ///dbstat: cl_stat*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_statfree")]
    public static extern int cl_statfree(ref cl_stat dbstat);


    /// Return Type: void
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_debug")]
    public static extern void cl_debug();


    /// Return Type: unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_retflevel")]
    public static extern uint cl_retflevel();


    /// Return Type: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_retver")]
    public static extern System.IntPtr cl_retver();


    /// Return Type: void
    ///dir: char*
    ///leavetemps: short
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_settempdir")]
    public static extern void cl_settempdir([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string dir, short leavetemps);


    /// Return Type: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cli_gettempdir")]
    public static extern System.IntPtr cli_gettempdir();


    /// Return Type: int
    ///dirname: char*
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cli_rmdirs")]
    public static extern int cli_rmdirs([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string dirname);


    /// Return Type: char*
    ///clerror: int
    [System.Runtime.InteropServices.DllImportAttribute("<Unknown>", EntryPoint = "cl_strerror")]
    public static extern System.IntPtr cl_strerror(int clerror);

}
