using System;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Web.graffiti_admin.site_options.utilities
{
	public partial class logviewer : AdminControlPanelPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			LiHyperLink.SetNameToCompare(Context, "settings");

			LogViews.ActiveViewIndex = Int32.Parse(Request.QueryString["type"] ?? "1") - 1;

			Query q = Log.CreateQuery();
			q.AndWhere(Log.Columns.Type, Request.QueryString["type"] ?? "1");
			q.PageSize = 15;
			q.PageIndex = Int32.Parse(Request.QueryString["p"] ?? "1");
			q.OrderByDesc(Log.Columns.CreatedOn);

			LogCollection logs = LogCollection.FetchByQuery(q);

			LogList.DataSource = logs;
			LogList.DataBind();

			if (logs.Count > 0)
			{
				pager.Text =
					Util.Pager(q.PageIndex, q.PageSize, q.GetRecordCount(), null,
					           Request.QueryString["type"] == null ? null : "?type=" + Request.QueryString["type"], "&larr; Older Logs",
					           "Newer Logs &rarr;");
				;
			}
		}
	}
}