namespace DataBuddy
{
	public class QueryCommand
	{
		private readonly ParameterCollection _parameters = new ParameterCollection();

		/// <summary>
		///     If this override is used, you must set the Sql property as well.
		/// </summary>
		public QueryCommand()
		{
		}

		public QueryCommand(string sql)
		{
			Sql = sql;
		}

		public string Sql { get; set; }

		public ParameterCollection Parameters
		{
			get { return _parameters; }
		}
	}
}