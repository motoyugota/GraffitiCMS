using System;
using System.Collections.Generic;
using Graffiti.Core;
using Graffiti.Core.Services;

namespace Graffiti.Web.graffiti_admin.site_options.utilities
{
    public partial class logviewer : Graffiti.Core.AdminControlPanelPage
    {

        private ILogService _logService;

        protected void Page_Load(object sender, EventArgs e)
        {
            LiHyperLink.SetNameToCompare(Context, "settings");

            LogViews.ActiveViewIndex = Int32.Parse(Request.QueryString["type"] ?? "1") - 1;

            int totalCount = 0;
            IList<Log> logsQ = _logService.FetchByType(Request.QueryString["type"] ?? "1",
                                                           Int32.Parse(Request.QueryString["p"] ?? "1"), 15,
                                                           out totalCount);
            LogCollection logs = new LogCollection(logsQ);

            LogList.DataSource = logs;
            LogList.DataBind();

            if(logs.Count > 0)
            {
                pager.Text =
                    Util.Pager(Int32.Parse(Request.QueryString["p"] ?? "1"), 15, totalCount, null,
                               Request.QueryString["type"] == null ? null : "?type=" + Request.QueryString["type"], "&larr; Older Logs", "Newer Logs &rarr;");
                ;
            }
        }
    }
}
