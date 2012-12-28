using System.Collections.Generic;

namespace DataBuddy
{
	public class Table
	{
		private List<Column> _columns = new List<Column>();
		private string _name;
		private string _typeName;

		public Table(string name, string typeName)
		{
			_name = name;
			_typeName = typeName;
		}

		public bool IsReadOnly { get; set; }


		public string TableName
		{
			get { return _name; }
		}

		public string TypeName
		{
			get { return _typeName; }
		}

		public List<Column> Columns
		{
			get { return _columns; }
		}

		public string PrimaryKey { get; set; }

		public bool HasPrimaryKey
		{
			get { return !string.IsNullOrEmpty(PrimaryKey); }
		}
	}
}