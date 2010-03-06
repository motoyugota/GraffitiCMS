using System;
using System.Linq;
using Graffiti.Core;

public partial class graffiti_admin_posts_HomePostSortOrder : AdminControlPanelPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Util.CanWriteRedirect(Context);

                LiHyperLink.SetNameToCompare(Context, "settings");
            }


            if (!IsPostBack)
            {     
                Posts.Items.Clear();

                string itemFormat = "<div style=\"border: solid 1px #999; padding: 4px;\"><strong>{0}</strong></div>";
                foreach (Post p in _postService.FetchPosts().Where(x => x.IsHome).OrderBy(x => x.HomeSortOrder))
                {
                    Posts.Items.Add(new Telligent.Glow.OrderedListItem(string.Format(itemFormat, p.Title), p.Title, p.Id.ToString()));
                }
            }
        }
    }