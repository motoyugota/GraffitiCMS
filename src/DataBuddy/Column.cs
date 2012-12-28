using System;
using System.Data;

namespace DataBuddy
{
	public class Column
	{
		private int _Length = -1;
		private string _alias;

		public Column()
		{
		}

		public Column(string name, DbType dtType, Type type, string alias, bool allowNull, bool isPK)
		{
			Name = name;
			DbType = dtType;
			Type = type;
			_alias = alias;
			IsPrimaryKey = isPK;
			AllowNull = allowNull;
		}

		public string Name { get; set; }

		public DbType DbType { get; set; }

		public Type Type { get; set; }

		public string Alias
		{
			get
			{
				if (string.IsNullOrEmpty(_alias))
					return Name;

				return _alias;
			}
			set { _alias = value; }
		}


		public bool IsPrimaryKey { get; set; }

		public bool AllowNull { get; set; }

		public int Length
		{
			get { return _Length; }
			set { _Length = value; }
		}
	}
}