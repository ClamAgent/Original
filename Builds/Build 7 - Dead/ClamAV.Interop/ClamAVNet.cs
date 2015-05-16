using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace ClamAV.Interop
{
    public class CL
    {
        public const int COUNT_PRECISION = 4096;
        // return codes
        public const int CLEAN = 0;             // no virus found
        public const int VIRUS = 1;             // virus(es) found
        public const int SUCCESS = CL.CLEAN;
        public const int BREAK = 2;
        public const int EMAXREC = -100;        // (internal) recursion limit exceeded
        public const int EMAXSIZE = -101;       // (internal) size limit exceeded
        public const int EMAXFILES = -102;      // (internal) files limit exceeded
        public const int ERAR = -103;           // rar handler error
        public const int EZIP = -104;           // zip handler error
        public const int EGZIP = -105;          // gzip handler error
        public const int EBZIP = -106;          // bzip2 handler error
        public const int EOLE2 = -107;          // OLE2 handler error
        public const int EMSCOMP = -108;        // MS Expand handler error
        public const int EMSCAB = -109;         // MS CAB module error
        public const int EACCES = -110;         // access denied
        public const int ENULLARG = -111;       // null argument
        public const int ETMPFILE = -112;       // tmpfile() failed
        public const int EMEM = -114;           // memory allocation error
        public const int EOPEN = -115;          // file open error
        public const int EMALFDB = -116;        // malformed database
        public const int EPATSHORT = -117;      // pattern too short
        public const int ETMPDIR = -118;        // mkdir() failed
        public const int ECVD = -119;           // not a CVD file (or broken)
        public const int ECVDEXTR = -120;       // CVD extraction failure
        public const int EMD5 = -121;           // MD5 verification error
        public const int EDSIG = -122;          // digital signature verification error
        public const int EIO = -123;            // general I/O error
        public const int EFORMAT = -124;        // (internal) bad format or broken file
        public const int ESUPPORT = -125;       // not supported data format
        public const int EARJ = -127;           // ARJ handler error
        // Callback
        public const int EUSERABORT = -500;     // interrupted by callback
    }
    public class CL_DB
    {
        public const int PHISHING      = 0x0002;
        public const int ACONLY        = 0x0004;    // WARNING: only for developers
        public const int PHISHING_URLS = 0x0008;
        public const int PUA           = 0x0010;
        public const int CVDNOTMP      = 0x0020;
        public const int OFFICIAL      = 0x0040;
        public const int PUA_MODE      = 0x0080;
        public const int PUA_INCLUDE   = 0x0100;
        public const int PUA_EXCLUDE   = 0x0200;
        // recommended db settings
        public const int STDOPT = (CL_DB.PHISHING | CL_DB.PHISHING_URLS);
    }
    public class CL_SCAN
    {
        public const int RAW			         = 0x0;
        public const int ARCHIVE			     = 0x1;
        public const int MAIL			         = 0x2;
        public const int OLE2			         = 0x4;
        public const int BLOCKENCRYPTED		     = 0x8;
        public const int HTML			         = 0x10;
        public const int PE			             = 0x20;
        public const int BLOCKBROKEN		     = 0x40;
        public const int MAILURL			     = 0x80;
        public const int BLOCKMAX		         = 0x100; // ignored
        public const int ALGORITHMIC		     = 0x200;
        public const int PHISHING_BLOCKSSL	     = 0x800; // ssl mismatches, not ssl by itself
        public const int PHISHING_BLOCKCLOAK     = 0x1000;
        public const int ELF			         = 0x2000;
        public const int PDF			         = 0x4000;
        public const int STRUCTURED		         = 0x8000;
        public const int STRUCTURED_SSN_NORMAL	 = 0x10000;
        public const int STRUCTURED_SSN_STRIPPED = 0x20000;
        public const int PARTIAL_MESSAGE         = 0x40000;
        public const int HEURISTIC_PRECEDENCE    = 0x80000;
        // recommended scan settings
        public const int CL_SCAN_STDOPT = CL_SCAN.ARCHIVE | CL_SCAN.MAIL | CL_SCAN.OLE2 | CL_SCAN.PDF | CL_SCAN.HTML | CL_SCAN.PE | CL_SCAN.ALGORITHMIC | CL_SCAN.ELF;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct cl_engine
    {
        public uint refcount; // reference counter
        public ushort sdb;
        public uint dboptions;

        // Roots table
        public IntPtr root;

        // B-M matcher for standard MD5 sigs
        public IntPtr md5_hdb;

        // B-M matcher for MD5 sigs for PE sections
        public IntPtr md5_mdb;

        // B-M matcher for whitelist db
        public IntPtr md5_fp;

        // Zip metadata
        public IntPtr zip_mlist;

        // RAR metadata
        public IntPtr rar_mlist;

        // Phishing .pdb and .wdb databases
        public IntPtr whitelist_matcher;
        public IntPtr domainlist_matcher;
        public IntPtr phishcheck;

        // Dynamic configuration
        public IntPtr dconf;

        // Filetype definitions
        public IntPtr ftypes;

        // Ignored signatures
        public System.IntPtr ignored;

        // PUA categories (to be included or excluded)
        [MarshalAs(UnmanagedType.LPStr)]
        public string pua_cats;

        // Callback for Scanning
        public cl_engine_callback EngineCallback;
    }
    public delegate int cl_engine_callback(int desc, int bytes);

    [StructLayout(LayoutKind.Sequential)]
    public struct cl_limits
    {
        public ulong maxscansize; // during the scanning of archives this size will never be exceeded
        public ulong maxfilesize; // compressed files will only be decompressed and scanned up to this size
        public uint maxreclevel; // maximum recursion level for archives
        public uint maxfiles; // maximum number of files to be scanned within a single archive
        public ushort archivememlim; // limit memory usage for some unpackers
        // This is for structured data detection.  You can set the minimum
        // number of occurences of an CC# or SSN before the system will
        // generate a notification.
        public uint min_cc_count;
        public uint min_ssn_count;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct cl_stat
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string dir;
        public uint entries;
        public IntPtr stattab;
        public IntPtr statdname;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct cl_cvd
    {
        [MarshalAs(UnmanagedType.LPStr)]    // field no.
        public string time;                 // 2
        public uint version;                // 3
        public uint sigs;                   // 4
        public uint fl;                     // 5
        [MarshalAs(UnmanagedType.LPStr)]
        public string md5;                  // 6
        [MarshalAs(UnmanagedType.LPStr)]
        public string dsig;                 // 7
        [MarshalAs(UnmanagedType.LPStr)]
        public string builder;              // 8
        public uint stime;                  // 9
    }

    public class cl_virus
    {
        public int Status;
        public string VirusName;
        public ulong Scanned;
    }

    public class ClamAVNet
    {
        // constructor
        public ClamAVNet(string tempdir, string database)
        {
            ClamAVNet.cl_settempdir(tempdir, 0);
            this._engine_ptr = new IntPtr();
            this._signo = new uint();
            this._db = database;
            this._db_loaded = false;

            this.LoadDatabase();
            this._limits = new cl_limits();

            this._limits.maxscansize = 0x06400000;
            this._limits.maxfilesize = 0x01900000;
            this._limits.maxreclevel = 16;
            this._limits.maxfiles = 0x00002710;
            this._limits.archivememlim = 0;
            this._limits.min_cc_count = 0;
            this._limits.min_ssn_count = 0;
        }
        // destructor
        ~ClamAVNet()
        {
            // Release memory used by the database
            if(this._db_loaded)
                ClamAVNet.cl_free(this._engine_ptr);
        }
        public cl_virus ScanFile(string filename)
        {
            cl_virus retvalue = new cl_virus();
            IntPtr virnameptr = new IntPtr();
            retvalue.Status = cl_scanfile(filename, ref virnameptr, ref retvalue.Scanned, this._engine_ptr, ref this._limits, CL_SCAN.CL_SCAN_STDOPT);
            retvalue.VirusName = Marshal.PtrToStringAnsi(virnameptr);
            return retvalue;
        }
        private int LoadDatabase()
        {
            int err;
            err = ClamAVNet.cl_load(this._db, ref this._engine_ptr, ref this._signo, 10);
            if (err == 0)
            {
                this._db_loaded = true;
                err = ClamAVNet.cl_build(this._engine_ptr);
            }
            return err;
        }
        private bool _db_loaded;
        private string _db;
        private IntPtr _engine_ptr;
        private uint _signo;
        private cl_limits _limits;

        // file scanning

        // cl_scandesc - File scanning based on file descriptor
        // extern int cl_scandesc(int desc, const char **virname, unsigned long int *scanned, const struct cl_engine *engine, const struct cl_limits *limits, unsigned int options);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_scandesc")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_scandesc")]
#endif
        private static extern int cl_scandesc(int desc, ref IntPtr virname, ref ulong scanned, ref cl_engine engine, ref cl_limits limits, uint options);

        // cl_scanfile - File scanning based on file name
        // extern int cl_scanfile(const char *filename, const char **virname, unsigned long int *scanned, const struct cl_engine *engine, const struct cl_limits *limits, unsigned int options);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_scanfile")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_scanfile")]
#endif
        // public static extern int cl_scanfile([In()] [MarshalAs(UnmanagedType.LPStr)] string filename, ref IntPtr virname, ref ulong scanned, ref cl_engine engine, ref cl_limits limits, uint options);
        private static extern int cl_scanfile([In()] [MarshalAs(UnmanagedType.LPStr)] string filename, ref IntPtr virname, ref ulong scanned, IntPtr engineptr, ref cl_limits limits, uint options);

        // database handling */
        // extern int cl_load(const char *path, struct cl_engine **engine, unsigned int *signo, unsigned int options);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_load")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_load")]
#endif
        private static extern int cl_load([In()] [MarshalAs(UnmanagedType.LPStr)] string path, ref IntPtr engine, ref uint signo, uint options);

        // extern const char *cl_retdbdir(void);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_retdbdir")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_retdbdir")]
#endif
        private static extern IntPtr cl_retdbdir();

        // engine handling
        // extern int cl_build(struct cl_engine *engine);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_build")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_build")]
#endif
        // private static extern int cl_build(ref cl_engine engine);
        private static extern int cl_build(IntPtr engineptr);

        // extern struct cl_engine *cl_dup(struct cl_engine *engine);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_dup")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_dup")]
#endif
        private static extern IntPtr cl_dup(ref cl_engine engine);

        // extern void cl_free(struct cl_engine *engine);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_free")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_free")]
#endif
//        private static extern void cl_free(ref cl_engine engine);
        private static extern void cl_free(IntPtr engineptr);
        // CVD
        // extern struct cl_cvd *cl_cvdhead(const char *file);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_cvdhead")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_cvdhead")]
#endif
        private static extern IntPtr cl_cvdhead([In()] [MarshalAs(UnmanagedType.LPStr)] string file);

        // extern struct cl_cvd *cl_cvdparse(const char *head);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_cvdparse")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_cvdparse")]
#endif
        private static extern System.IntPtr cl_cvdparse([In()] [MarshalAs(UnmanagedType.LPStr)] string head);

        // extern int cl_cvdverify(const char *file);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_cvdverify")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_cvdverify")]
#endif
        private static extern int cl_cvdverify([In()] [MarshalAs(UnmanagedType.LPStr)] string file);

        // extern void cl_cvdfree(struct cl_cvd *cvd);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_cvdfree")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_cvdfree")]
#endif
        private static extern void cl_cvdfree(ref cl_cvd cvd);

        // db dir stat functions
        // extern int cl_statinidir(const char *dirname, struct cl_stat *dbstat);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_statinidir")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_statinidir")]
#endif
        private static extern int cl_statinidir([In()] [MarshalAs(UnmanagedType.LPStr)] string dirname, ref cl_stat dbstat);

        // extern int cl_statchkdir(const struct cl_stat *dbstat);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_statchkdir")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_statchkdir")]
#endif
        private static extern int cl_statchkdir(ref cl_stat dbstat);

        // extern int cl_statfree(struct cl_stat *dbstat);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_statfree")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_statfree")]
#endif
        private static extern int cl_statfree(ref cl_stat dbstat);

        // enable debug messages
        // extern void cl_debug(void);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_debug")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_debug")]
#endif
        private static extern void cl_debug();

        // software versions
        // extern unsigned int cl_retflevel(void);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_retflevel")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_retflevel")]
#endif
        private static extern uint cl_retflevel();

        // extern const char *cl_retver(void);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_retver")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_retver")]
#endif
        private static extern IntPtr cl_retver();

        // others
        // extern void cl_settempdir(const char *dir, short leavetemps);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_settempdir")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_settempdir")]
#endif
        private static extern void cl_settempdir([In()] [MarshalAs(UnmanagedType.LPStr)] string dir, short leavetemps);

        // extern char *cli_gettempdir(void);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cli_gettempdir")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cli_gettempdir")]
#endif
        private static extern IntPtr cli_gettempdir();

        // extern int cli_rmdirs(const char *dirname);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cli_rmdirs")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cli_rmdirs")]
#endif
        private static extern int cli_rmdirs([In()] [MarshalAs(UnmanagedType.LPStr)] string dirname);

        // extern const char *cl_strerror(int clerror);
#if DEBUG
        [DllImport("libclamavd.dll", EntryPoint = "cl_strerror")]
#else
        [DllImport("libclamav.dll", EntryPoint = "cl_strerror")]
#endif
        private static extern IntPtr cl_strerror(int clerror);
    }
}