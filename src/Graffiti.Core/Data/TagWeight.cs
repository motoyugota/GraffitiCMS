using System.Web;

namespace Graffiti.Core
{
	public partial class TagWeight
	{
		private int _fontFactor;
		private string _fontSize;

		public string VirtualUrl
		{
			get { return "~/tags/" + Name.ToLower() + "/"; }
		}

		public string Url
		{
			get { return VirtualPathUtility.ToAbsolute(VirtualUrl); }
		}


		public string FontSize
		{
			get { return _fontSize; }
			set { _fontSize = value; }
		}

		public int FontFactor
		{
			get { return _fontFactor; }
			set { _fontFactor = value; }
		}

		public int Count
		{
			get { return Weight; }
		}
	}
}