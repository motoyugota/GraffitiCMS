using System;
using DataBuddy;
using Graffiti.Core;
using Telligent.Glow;

public partial class graffiti_admin_posts_HomePostSortOrder : AdminControlPanelPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Util.CanWriteRedirect(Context);

			LiHyperLink.SetNameToCompare(Context, "settings");
		}


		if (!IsPostBack)
		{
			//EnableHomeSort.Checked = SiteSettings.Get().UseCustomHomeList;

			Posts.Items.Clear();

			Query query = Post.CreateQuery();
			query.AndWhere(Post.Columns.IsHome, true);
			query.OrderByAsc(Post.Columns.HomeSortOrder);

			string itemFormat = "<div style=\"border: solid 1px #999; padding: 4px;\"><strong>{0}</strong></div>";
			foreach (Post p in PostCollection.FetchByQuery(query))
			{
				Posts.Items.Add(new OrderedListItem(string.Format(itemFormat, p.Title), p.Title, p.Id.ToString()));
			}
		}
	}
}