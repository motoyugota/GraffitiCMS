using System;
using System.Web;
using System.Web.UI.WebControls;
using DataBuddy;
using Graffiti.Core;
using Telligent.Glow;

public partial class graffiti_admin_presentation_navigation_Default : AdminControlPanelPage
{
	protected static string TrashImage
	{
		get { return VirtualPathUtility.ToAbsolute("~/graffiti-admin/common/img/trash.gif"); }
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		LiHyperLink.SetNameToCompare(Context, "presentation");

		NavigationSettings settings = NavigationSettings.Get();
		CategoryCollection cc = new CategoryController().GetTopLevelCachedCategories();

		foreach (Category c in cc)
		{
			bool found = false;
			foreach (DynamicNavigationItem di in settings.SafeItems())
			{
				if (di.NavigationType == DynamicNavigationType.Category && di.CategoryId == c.Id)
				{
					found = true;
					break;
				}
			}

			if (!found)
				the_Categories.Items.Add(new ListItem(c.Name, "Category-" + c.UniqueId));
		}


		Query q = Post.CreateQuery();
		q.AndWhere(Post.Columns.CategoryId, CategoryController.UnCategorizedId);
		q.AndWhere(Post.Columns.IsDeleted, false);
		q.AndWhere(Post.Columns.Status, 1);

		PostCollection pc = new PostCollection();
		pc.LoadAndCloseReader(q.ExecuteReader());

		foreach (Post p in pc)
		{
			bool found = false;
			foreach (DynamicNavigationItem di in settings.SafeItems())
			{
				if (di.NavigationType == DynamicNavigationType.Post && di.PostId == p.Id)
				{
					found = true;
					break;
				}
			}

			if (!found)
				the_Posts.Items.Add(new ListItem(p.Title, "Post-" + p.UniqueId));
		}

		// 0 - Title, 1 - Type, 2 - LID
		string itemFormat =
			"<div style=\"border: solid 1px #999; padding: 4px;\"><strong>{0} ({1})</strong><div style=\"text-align:right;\"><a title=\"Delete Link\" href=\"javascript:void();\" onclick=\"remove_Link( &#39;{1}&#39;,&#39;{0}&#39;, &#39;{2}&#39;); return false;\">Delete</a></div></div>";
		foreach (DynamicNavigationItem dni in settings.SafeItems())
		{
			lbar.Items.Add(
				new OrderedListItem(string.Format(itemFormat, dni.Name, dni.NavigationType.ToString(), dni.Id),
				                    dni.Name, dni.NavigationType.ToString() + "-" + dni.Id.ToString()));
		}
	}
}