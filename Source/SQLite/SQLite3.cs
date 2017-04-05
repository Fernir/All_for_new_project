using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SQLite
{
	public static class SQLite3
	{
		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_bind_blob", ExactSpelling=false)]
		public static extern int BindBlob(IntPtr stmt, int index, byte[] val, int n, IntPtr free);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_bind_double", ExactSpelling=false)]
		public static extern int BindDouble(IntPtr stmt, int index, double val);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_bind_int", ExactSpelling=false)]
		public static extern int BindInt(IntPtr stmt, int index, int val);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_bind_int64", ExactSpelling=false)]
		public static extern int BindInt64(IntPtr stmt, int index, long val);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_bind_null", ExactSpelling=false)]
		public static extern int BindNull(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_bind_parameter_index", ExactSpelling=false)]
		public static extern int BindParameterIndex(IntPtr stmt, string name);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Unicode, EntryPoint="sqlite3_bind_text16", ExactSpelling=false)]
		public static extern int BindText(IntPtr stmt, int index, string val, int n, IntPtr free);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_busy_timeout", ExactSpelling=false)]
		public static extern SQLite3.Result BusyTimeout(IntPtr db, int milliseconds);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_changes", ExactSpelling=false)]
		public static extern int Changes(IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_close", ExactSpelling=false)]
		public static extern SQLite3.Result Close(IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_blob", ExactSpelling=false)]
		public static extern IntPtr ColumnBlob(IntPtr stmt, int index);

		public static byte[] ColumnByteArray(IntPtr stmt, int index)
		{
			int num = SQLite3.ColumnBytes(stmt, index);
			byte[] numArray = new byte[num];
			if (num > 0)
			{
				Marshal.Copy(SQLite3.ColumnBlob(stmt, index), numArray, 0, num);
			}
			return numArray;
		}

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_bytes", ExactSpelling=false)]
		public static extern int ColumnBytes(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_count", ExactSpelling=false)]
		public static extern int ColumnCount(IntPtr stmt);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_double", ExactSpelling=false)]
		public static extern double ColumnDouble(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_int", ExactSpelling=false)]
		public static extern int ColumnInt(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_int64", ExactSpelling=false)]
		public static extern long ColumnInt64(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_name", ExactSpelling=false)]
		public static extern IntPtr ColumnName(IntPtr stmt, int index);

		public static string ColumnName16(IntPtr stmt, int index)
		{
			return Marshal.PtrToStringUni(SQLite3.ColumnName16Internal(stmt, index));
		}

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_name16", ExactSpelling=false)]
		private static extern IntPtr ColumnName16Internal(IntPtr stmt, int index);

		public static string ColumnString(IntPtr stmt, int index)
		{
			return Marshal.PtrToStringUni(SQLite3.ColumnText16(stmt, index));
		}

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_text", ExactSpelling=false)]
		public static extern IntPtr ColumnText(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_text16", ExactSpelling=false)]
		public static extern IntPtr ColumnText16(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_column_type", ExactSpelling=false)]
		public static extern SQLite3.ColType ColumnType(IntPtr stmt, int index);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_config", ExactSpelling=false)]
		public static extern SQLite3.Result Config(SQLite3.ConfigOption option);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_enable_load_extension", ExactSpelling=false)]
		public static extern SQLite3.Result EnableLoadExtension(IntPtr db, int onoff);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_errmsg16", ExactSpelling=false)]
		public static extern IntPtr Errmsg(IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_extended_errcode", ExactSpelling=false)]
		public static extern SQLite3.ExtendedResult ExtendedErrCode(IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_finalize", ExactSpelling=false)]
		public static extern SQLite3.Result Finalize(IntPtr stmt);

		public static string GetErrmsg(IntPtr db)
		{
			return Marshal.PtrToStringUni(SQLite3.Errmsg(db));
		}

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_initialize", ExactSpelling=false)]
		public static extern SQLite3.Result Initialize();

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_last_insert_rowid", ExactSpelling=false)]
		public static extern long LastInsertRowid(IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_libversion_number", ExactSpelling=false)]
		public static extern int LibVersionNumber();

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_open", ExactSpelling=false)]
		public static extern SQLite3.Result Open(string filename, out IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_open_v2", ExactSpelling=false)]
		public static extern SQLite3.Result Open(string filename, out IntPtr db, int flags, IntPtr zvfs);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_open_v2", ExactSpelling=false)]
		public static extern SQLite3.Result Open(byte[] filename, out IntPtr db, int flags, IntPtr zvfs);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_open16", ExactSpelling=false)]
		public static extern SQLite3.Result Open16(string filename, out IntPtr db);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_prepare_v2", ExactSpelling=false)]
		public static extern SQLite3.Result Prepare2(IntPtr db, string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);

		public static IntPtr Prepare2(IntPtr db, string query)
		{
			IntPtr intPtr;
			SQLite3.Result result = SQLite3.Prepare2(db, query, Encoding.UTF8.GetByteCount(query), out intPtr, IntPtr.Zero);
			if (result != SQLite3.Result.OK)
			{
				throw SQLiteException.New(result, SQLite3.GetErrmsg(db));
			}
			return intPtr;
		}

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_reset", ExactSpelling=false)]
		public static extern SQLite3.Result Reset(IntPtr stmt);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Unicode, EntryPoint="sqlite3_win32_set_directory", ExactSpelling=false)]
		public static extern int SetDirectory(uint directoryType, string directoryPath);

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_shutdown", ExactSpelling=false)]
		public static extern SQLite3.Result Shutdown();

		[DllImport("sqlite3", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.None, EntryPoint="sqlite3_step", ExactSpelling=false)]
		public static extern SQLite3.Result Step(IntPtr stmt);

		public enum ColType
		{
			Integer = 1,
			Float = 2,
			Text = 3,
			Blob = 4,
			Null = 5
		}

		public enum ConfigOption
		{
			SingleThread = 1,
			MultiThread = 2,
			Serialized = 3
		}

		public enum ExtendedResult
		{
			BusyRecovery = 261,
			LockedSharedcache = 262,
			ReadonlyRecovery = 264,
			IOErrorRead = 266,
			CorruptVTab = 267,
			CannottOpenNoTempDir = 270,
			ConstraintCheck = 275,
			NoticeRecoverWAL = 283,
			AbortRollback = 516,
			ReadonlyCannotLock = 520,
			IOErrorShortRead = 522,
			CannotOpenIsDir = 526,
			ConstraintCommitHook = 531,
			NoticeRecoverRollback = 539,
			ReadonlyRollback = 776,
			IOErrorWrite = 778,
			CannotOpenFullPath = 782,
			ConstraintForeignKey = 787,
			IOErrorFsync = 1034,
			ConstraintFunction = 1043,
			IOErrorDirFSync = 1290,
			ConstraintNotNull = 1299,
			IOErrorTruncate = 1546,
			ConstraintPrimaryKey = 1555,
			IOErrorFStat = 1802,
			ConstraintTrigger = 1811,
			IOErrorUnlock = 2058,
			ConstraintUnique = 2067,
			IOErrorRdlock = 2314,
			ConstraintVTab = 2323,
			IOErrorDelete = 2570,
			IOErrorBlocked = 2826,
			IOErrorNoMem = 3082,
			IOErrorAccess = 3338,
			IOErrorCheckReservedLock = 3594,
			IOErrorLock = 3850,
			IOErrorClose = 4106,
			IOErrorDirClose = 4362,
			IOErrorSHMOpen = 4618,
			IOErrorSHMSize = 4874,
			IOErrorSHMLock = 5130,
			IOErrorSHMMap = 5386,
			IOErrorSeek = 5642,
			IOErrorDeleteNoEnt = 5898,
			IOErrorMMap = 6154
		}

		public enum Result
		{
			OK = 0,
			Error = 1,
			Internal = 2,
			Perm = 3,
			Abort = 4,
			Busy = 5,
			Locked = 6,
			NoMem = 7,
			ReadOnly = 8,
			Interrupt = 9,
			IOError = 10,
			Corrupt = 11,
			NotFound = 12,
			Full = 13,
			CannotOpen = 14,
			LockErr = 15,
			Empty = 16,
			SchemaChngd = 17,
			TooBig = 18,
			Constraint = 19,
			Mismatch = 20,
			Misuse = 21,
			NotImplementedLFS = 22,
			AccessDenied = 23,
			Format = 24,
			Range = 25,
			NonDBFile = 26,
			Notice = 27,
			Warning = 28,
			Row = 100,
			Done = 101
		}
	}
}