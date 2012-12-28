using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Graffiti.Core
{
	public class LiHyperLink : HyperLink
	{
		private bool _IsSelected;

		private string _name = "__NOT_SET_BY_DEFAULT__";
		public string LiCSS { get; set; }

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		protected override void OnPreRender(EventArgs e)
		{
			_IsSelected = IsNameMatched() || (VirtualPathUtility.ToAbsolute(NavigateUrl).ToLower() ==
			                                  Context.Request.Url.AbsolutePath.ToLower()
			                                         .Replace(Util.DEFAULT_PAGE_LOWERED, string.Empty));


			base.OnPreRender(e);
		}

		private bool IsNameMatched()
		{
			return Context.Items["li_Selected_Name"] as string == Name;
		}


		public static void SetNameToCompare(HttpContext context, string name)
		{
			context.Items["li_Selected_Name"] = name;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write("<li");

			if (_IsSelected)
				writer.Write(" class = \"selected\"");
			writer.Write(">");
			base.Render(writer);
			writer.Write("</li>");
		}
	}
}