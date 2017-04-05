using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SQLite
{
	public class TableMapping
	{
		private TableMapping.Column _autoPk;

		private TableMapping.Column[] _insertColumns;

		private TableMapping.Column[] _insertOrReplaceColumns;

		private PreparedSqlLiteInsertCommand _insertCommand;

		private string _insertCommandExtra;

		public TableMapping.Column[] Columns
		{
			get;
			private set;
		}

		public string GetByPrimaryKeySql
		{
			get;
			private set;
		}

		public bool HasAutoIncPK
		{
			get;
			private set;
		}

		public TableMapping.Column[] InsertColumns
		{
			get
			{
				if (this._insertColumns == null)
				{
					this._insertColumns = (
						from c in (IEnumerable<TableMapping.Column>)this.Columns
						where !c.IsAutoInc
						select c).ToArray<TableMapping.Column>();
				}
				return this._insertColumns;
			}
		}

		public TableMapping.Column[] InsertOrReplaceColumns
		{
			get
			{
				if (this._insertOrReplaceColumns == null)
				{
					this._insertOrReplaceColumns = this.Columns.ToArray<TableMapping.Column>();
				}
				return this._insertOrReplaceColumns;
			}
		}

		public Type MappedType
		{
			get;
			private set;
		}

		public TableMapping.Column PK
		{
			get;
			private set;
		}

		public string TableName
		{
			get;
			private set;
		}

		public TableMapping(Type type, CreateFlags createFlags = 0)
		{
			int i;
			this.MappedType = type;
			TableAttribute tableAttribute = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault<object>();
			this.TableName = (tableAttribute != null ? tableAttribute.Name : this.MappedType.Name);
			PropertyInfo[] properties = this.MappedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
			List<TableMapping.Column> columns = new List<TableMapping.Column>();
			PropertyInfo[] propertyInfoArray = properties;
			for (i = 0; i < (int)propertyInfoArray.Length; i++)
			{
				PropertyInfo propertyInfo = propertyInfoArray[i];
				bool length = propertyInfo.GetCustomAttributes(typeof(IgnoreAttribute), true).Length != 0;
				if (propertyInfo.CanWrite && !length)
				{
					columns.Add(new TableMapping.Column(propertyInfo, createFlags));
				}
			}
			this.Columns = columns.ToArray();
			TableMapping.Column[] columnArray = this.Columns;
			for (i = 0; i < (int)columnArray.Length; i++)
			{
				TableMapping.Column column = columnArray[i];
				if (column.IsAutoInc && column.IsPK)
				{
					this._autoPk = column;
				}
				if (column.IsPK)
				{
					this.PK = column;
				}
			}
			this.HasAutoIncPK = this._autoPk != null;
			if (this.PK == null)
			{
				this.GetByPrimaryKeySql = string.Format("select * from \"{0}\" limit 1", this.TableName);
				return;
			}
			this.GetByPrimaryKeySql = string.Format("select * from \"{0}\" where \"{1}\" = ?", this.TableName, this.PK.Name);
		}

		private PreparedSqlLiteInsertCommand CreateInsertCommand(SQLiteConnection conn, string extra)
		{
			string str;
			TableMapping.Column[] insertColumns = this.InsertColumns;
			if (insertColumns.Any<TableMapping.Column>() || this.Columns.Count<TableMapping.Column>() != 1 || !this.Columns[0].IsAutoInc)
			{
				if (string.Compare(extra, "OR REPLACE", StringComparison.OrdinalIgnoreCase) == 0)
				{
					insertColumns = this.InsertOrReplaceColumns;
				}
				object[] tableName = new object[] { this.TableName, null, null, null };
				tableName[1] = string.Join(",", (
					from c in (IEnumerable<TableMapping.Column>)insertColumns
					select string.Concat("\"", c.Name, "\"")).ToArray<string>());
				tableName[2] = string.Join(",", (
					from c in (IEnumerable<TableMapping.Column>)insertColumns
					select "?").ToArray<string>());
				tableName[3] = extra;
				str = string.Format("insert {3} into \"{0}\"({1}) values ({2})", tableName);
			}
			else
			{
				str = string.Format("insert {1} into \"{0}\" default values", this.TableName, extra);
			}
			return new PreparedSqlLiteInsertCommand(conn)
			{
				CommandText = str
			};
		}

		protected internal void Dispose()
		{
			if (this._insertCommand != null)
			{
				this._insertCommand.Dispose();
				this._insertCommand = null;
			}
		}

		public TableMapping.Column FindColumn(string columnName)
		{
			return this.Columns.FirstOrDefault<TableMapping.Column>((TableMapping.Column c) => c.Name == columnName);
		}

		public TableMapping.Column FindColumnWithPropertyName(string propertyName)
		{
			return this.Columns.FirstOrDefault<TableMapping.Column>((TableMapping.Column c) => c.PropertyName == propertyName);
		}

		public PreparedSqlLiteInsertCommand GetInsertCommand(SQLiteConnection conn, string extra)
		{
			if (this._insertCommand == null)
			{
				this._insertCommand = this.CreateInsertCommand(conn, extra);
				this._insertCommandExtra = extra;
			}
			else if (this._insertCommandExtra != extra)
			{
				this._insertCommand.Dispose();
				this._insertCommand = this.CreateInsertCommand(conn, extra);
				this._insertCommandExtra = extra;
			}
			return this._insertCommand;
		}

		public void SetAutoIncPK(object obj, long id)
		{
			if (this._autoPk != null)
			{
				this._autoPk.SetValue(obj, Convert.ChangeType(id, this._autoPk.ColumnType, null));
			}
		}

		public class Column
		{
			private PropertyInfo _prop;

			public string Collation
			{
				get;
				private set;
			}

			public Type ColumnType
			{
				get;
				private set;
			}

			public IEnumerable<IndexedAttribute> Indices
			{
				get;
				set;
			}

			public bool IsAutoGuid
			{
				get;
				private set;
			}

			public bool IsAutoInc
			{
				get;
				private set;
			}

			public bool IsNullable
			{
				get;
				private set;
			}

			public bool IsPK
			{
				get;
				private set;
			}

			public int? MaxStringLength
			{
				get;
				private set;
			}

			public string Name
			{
				get;
				private set;
			}

			public string PropertyName
			{
				get
				{
					return this._prop.Name;
				}
			}

			public Column(PropertyInfo prop, CreateFlags createFlags = 0)
			{
				bool flag;
				bool flag1;
				ColumnAttribute columnAttribute = (ColumnAttribute)prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault<object>();
				this._prop = prop;
				this.Name = (columnAttribute == null ? prop.Name : columnAttribute.Name);
				this.ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
				this.Collation = Orm.Collation(prop);
				if (Orm.IsPK(prop))
				{
					flag = true;
				}
				else
				{
					flag = ((createFlags & CreateFlags.ImplicitPK) != CreateFlags.ImplicitPK ? false : string.Compare(prop.Name, "Id", StringComparison.OrdinalIgnoreCase) == 0);
				}
				this.IsPK = flag;
				if (Orm.IsAutoInc(prop))
				{
					flag1 = true;
				}
				else
				{
					flag1 = (!this.IsPK ? false : (createFlags & CreateFlags.AutoIncPK) == CreateFlags.AutoIncPK);
				}
				bool flag2 = flag1;
				this.IsAutoGuid = (!flag2 ? false : this.ColumnType == typeof(Guid));
				this.IsAutoInc = (!flag2 ? false : !this.IsAutoGuid);
				this.Indices = Orm.GetIndices(prop);
				if (!this.Indices.Any<IndexedAttribute>() && !this.IsPK && (createFlags & CreateFlags.ImplicitIndex) == CreateFlags.ImplicitIndex && this.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
				{
					this.Indices = (IEnumerable<IndexedAttribute>)(new IndexedAttribute[] { new IndexedAttribute() });
				}
				this.IsNullable = (this.IsPK ? false : !Orm.IsMarkedNotNull(prop));
				this.MaxStringLength = Orm.MaxStringLength(prop);
			}

			public object GetValue(object obj)
			{
				return this._prop.GetValue(obj, null);
			}

			public void SetValue(object obj, object val)
			{
				this._prop.SetValue(obj, val, null);
			}
		}
	}
}