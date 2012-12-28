using System.Collections.Generic;

namespace DataBuddy
{
	public class Schema : List<Table>
	{
		public string NameSpace { get; set; }
	}
}