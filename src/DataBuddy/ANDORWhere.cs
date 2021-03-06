namespace DataBuddy
{
	public class ANDORWhere : WHERE
	{
		private Column _column;
		private Comparison _comp = Comparison.Equals;
		private bool _useOr;
		private object _value;

		public ANDORWhere(Column column, object value)
			: this(column, value, false)
		{
		}

		public ANDORWhere(Column column, object value, bool useOr)
			: this(column, value, Comparison.Equals, useOr)
		{
		}

		public ANDORWhere(Column column, object value, Comparison comp, bool useOr)
		{
			_column = column;
			_value = value;
			_comp = comp;
			_useOr = useOr;
		}

		public override string ToSQL(QueryCommand cmd, Table tbl, string letter, bool isFirst)
		{
			string result = isFirst ? "" : (_useOr ? " OR " : " AND ");

			result +=
				DataService.Provider.QuoteName(tbl.TableName)
				+ "." + DataService.Provider.QuoteName(_column.Name)
				+ " "
				+ DataService.GetComparisonOperator(_comp)
				+ " "
				+ DataService.Provider.SqlVariable(_column.Name + "_" + letter);

			cmd.Parameters.Add(_column.Name + "_" + letter, _value, _column.DbType);

			return result;
		}
	}
}