using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SQLite
{
	public static class Orm
	{
		public const int DefaultMaxStringLength = 140;

		public const string ImplicitPkName = "Id";

		public const string ImplicitIndexSuffix = "Id";

		public static string Collation(MemberInfo p)
		{
			object[] customAttributes = p.GetCustomAttributes(typeof(CollationAttribute), true);
			if (customAttributes.Length == 0)
			{
				return string.Empty;
			}
			return ((CollationAttribute)customAttributes[0]).Value;
		}

		public static IEnumerable<IndexedAttribute> GetIndices(MemberInfo p)
		{
			return p.GetCustomAttributes(typeof(IndexedAttribute), true).Cast<IndexedAttribute>();
		}

		public static bool IsAutoInc(MemberInfo p)
		{
			return p.GetCustomAttributes(typeof(AutoIncrementAttribute), true).Length != 0;
		}

		public static bool IsMarkedNotNull(MemberInfo p)
		{
			return p.GetCustomAttributes(typeof(NotNullAttribute), true).Length != 0;
		}

		public static bool IsPK(MemberInfo p)
		{
			return p.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length != 0;
		}

		public static int? MaxStringLength(PropertyInfo p)
		{
			object[] customAttributes = p.GetCustomAttributes(typeof(MaxLengthAttribute), true);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			return new int?(((MaxLengthAttribute)customAttributes[0]).Value);
		}

		public static string SqlDecl(TableMapping.Column p, bool storeDateTimeAsTicks)
		{
			string str = string.Concat(new string[] { "\"", p.Name, "\" ", Orm.SqlType(p, storeDateTimeAsTicks), " " });
			if (p.IsPK)
			{
				str = string.Concat(str, "primary key ");
			}
			if (p.IsAutoInc)
			{
				str = string.Concat(str, "autoincrement ");
			}
			if (!p.IsNullable)
			{
				str = string.Concat(str, "not null ");
			}
			if (!string.IsNullOrEmpty(p.Collation))
			{
				str = string.Concat(str, "collate ", p.Collation, " ");
			}
			return str;
		}

		public static string SqlType(TableMapping.Column p, bool storeDateTimeAsTicks)
		{
			Type columnType = p.ColumnType;
			if (columnType == typeof(bool) || columnType == typeof(byte) || columnType == typeof(ushort) || columnType == typeof(sbyte) || columnType == typeof(short) || columnType == typeof(int))
			{
				return "integer";
			}
			if (columnType == typeof(uint) || columnType == typeof(long))
			{
				return "bigint";
			}
			if (columnType == typeof(float) || columnType == typeof(double) || columnType == typeof(decimal))
			{
				return "float";
			}
			if (columnType == typeof(string))
			{
				int? maxStringLength = p.MaxStringLength;
				if (!maxStringLength.HasValue)
				{
					return "varchar";
				}
				return string.Concat("varchar(", maxStringLength.Value, ")");
			}
			if (columnType == typeof(TimeSpan))
			{
				return "bigint";
			}
			if (columnType == typeof(DateTime))
			{
				if (!storeDateTimeAsTicks)
				{
					return "datetime";
				}
				return "bigint";
			}
			if (columnType == typeof(DateTimeOffset))
			{
				return "bigint";
			}
			if (columnType.IsEnum)
			{
				return "integer";
			}
			if (columnType == typeof(byte[]))
			{
				return "blob";
			}
			if (columnType != typeof(Guid))
			{
				throw new NotSupportedException(string.Concat("Don't know about ", columnType));
			}
			return "varchar(36)";
		}
	}
}