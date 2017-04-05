using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public class NotNullConstraintViolationException : SQLiteException
	{
		public IEnumerable<TableMapping.Column> Columns
		{
			get;
			protected set;
		}

		protected NotNullConstraintViolationException(SQLite3.Result r, string message) : this(r, message, null, null)
		{
		}

		protected NotNullConstraintViolationException(SQLite3.Result r, string message, TableMapping mapping, object obj) : base(r, message)
		{
			if (mapping != null && obj != null)
			{
				this.Columns = mapping.Columns.Where<TableMapping.Column>((TableMapping.Column c) => {
					if (c.IsNullable)
					{
						return false;
					}
					return c.GetValue(obj) == null;
				});
			}
		}

		public static new NotNullConstraintViolationException New(SQLite3.Result r, string message)
		{
			return new NotNullConstraintViolationException(r, message);
		}

		public static NotNullConstraintViolationException New(SQLite3.Result r, string message, TableMapping mapping, object obj)
		{
			return new NotNullConstraintViolationException(r, message, mapping, obj);
		}

		public static NotNullConstraintViolationException New(SQLiteException exception, TableMapping mapping, object obj)
		{
			return new NotNullConstraintViolationException(exception.Result, exception.Message, mapping, obj);
		}
	}
}