<%@ Page Language="C#" MasterPageFile="~/graffiti-admin/common/AdminMasterPage.master" Title="Welcome to Graffiti" Inherits="Graffiti.Core.ControlPanelPage" %>
<%@ Register TagPrefix="reports" TagName="postview" Src="~/graffiti-admin/reporting/dashboardpostview.ascx" %>
<%@ Register TagPrefix="reports" TagName="popularview" Src="~/graffiti-admin/reporting/dashboardpopularview.ascx" %>
<%@ Import Namespace="System.Collections.Generic"%>
<%@ Import Namespace="System.Linq"%>
<%@ Import Namespace="Graffiti.Core.Services"%>
<%@ Import namespace="RssToolkit.Rss"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderRegion" Runat="Server">
<script type="text/javascript" src="reporting/swfobject.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainRegion" Runat="Server">

<script language="javascript" type="text/javascript">
function decrementComments()
{
    var counter = document.getElementById('<%= CommentCount.ClientID %>');
    counter.value = parseInt(counter.value) - 1;
           
    cpending = document.getElementById('<%= lblTotalCommentsPending.ClientID %>');
     
    if (counter.value == 1)
        cpending.innerHTML = " is <span class=\"largeNumberText\">1</span> comment ";
    else
        cpending.innerHTML = " are <span class=\"largeNumberText\">" + counter.value + "</span> comments ";
}

function deleteComment(id)
{
    Comments.deleteCommentWithStatus('<%= new Urls().AdminAjax %>', id, 'comment'+id);
}

function approveComment(id)
{
    Comments.approve('<%= new Urls().AdminAjax %>', id, 'comment' + id);
}
</script>

<h1>Your Dashboard</h1>
<div style="margin-top: 15px;">
    <div style="float: left; width: 45%;">
        <h3>Welcome back, <%= GraffitiUsers.Current.ProperName %>!</h3>
        
        <p style="margin-bottom: 20px; margin-top: 0px;">You've published <asp:Label ID="lblTotalPublished" runat="server" /> and your content has <asp:Label ID="lblTotalComments" runat="server" />.</p>
        
        <div id="WaitingApproval" style="margin-bottom: 20px;" runat="server" visible="false">
            <p style="margin-bottom: 5px;">There <asp:Label ID="lblTotalPending" runat="server" /> waiting for approval.</p>
            
            <Z:Repeater ShowHeaderFooterOnNone="false" ID="PendingPostList" runat="Server">
                <HeaderTemplate>
                    <table>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="padding-left: 15px;">
                            <li><a href="<%# ResolveUrl("~/graffiti-admin/posts/write/?id=" + Eval("Id")) %>"><%# Eval("Title") %></a>, published by <%# Eval("User.ProperName") %></li>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </Z:Repeater>
        </div>
        
        <p style="margin-bottom: 5px;">There <asp:Label ID="lblTotalCommentsPending" runat="server" /> waiting for approval.</p>
        
        <Z:Repeater ShowHeaderFooterOnNone="false" ID="PendingCommentList" runat="Server">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id="comment<%# Eval("ID") %>">
                    <td style="width: 255px; padding-left: 15px;">
                        <li><a href="<%# ResolveUrl("~/graffiti-admin/comments/?id=" + Eval("Id")) %>"><%# Eval("Post.Name") %></a>, posted by <%# Eval("Name") %></li>
                    </td>
                    <td>
                        <a href="javascript:void(0);" onclick="approveComment('<%# Eval("Id") %>'); return false;">Approve</a>
                        <asp:Label ID="pipe2" runat="server" Text=" | " />
                        <a href="javascript:void(0);" onclick="deleteComment('<%# Eval("Id") %>'); return false;">Delete</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </Z:Repeater>
        
        <reports:postview id="reportsbypost" runat="server" />
        
        <div style="clear: both;"></div>
    </div>
    <div style="float: right; width: 45%;">
        <h3>Recent Graffiti News</h3>
        <Z:Repeater runat="Server" ID = "RecentNews" ShowHeaderFooterOnNone="false">
            <HeaderTemplate><ul style="margin-left:0;padding-left:0;"></HeaderTemplate>
            <FooterTemplate></ul></FooterTemplate>
            <ItemTemplate>
                <li style="list-style-type:none;margin-bottom:3px;"><a target="_blank" href="<%# Eval("Link") %>"><%# Eval("Title") %></a><br /><%# Util.RemoveHtml(Eval("Description") as string, 200)%>...</li>
            </ItemTemplate>
            <NoneTemplate>Sorry, no news was found.</NoneTemplate>
        </Z:Repeater>

        <reports:popularview id="reportbypopularity" runat="server" />
        
        <div style="clear: both;"></div>
    </div>
    <div style="clear: both;"></div>
</div>
<asp:HiddenField ID="CommentCount" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarRegion" Runat="Server">
    <div id="sidebar"><div class="gutter">
    <div class="box">
	    <h3>About This Page</h3>
	    <p>Throughout the Graffiti control panel you can expect to find help and tips in this area.</p>
    </div>
    <div style="clear: both;"></div>
    </div></div>
</asp:Content>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            GetTotals();

            if (GraffitiUsers.IsAdmin(GraffitiUsers.Current))
                GetWaitingApprovals();

            Feed feed = FeedManager.GetFeed("http://graffiticms.com/blog/feed/", false);

            List<RssItem> items = new List<RssItem>();
            if(feed != null && feed.Document != null && feed.Document.Channel != null && feed.Document.Channel.Items.Count > 0)
            {
                
                for (int i = 0; i < Math.Min(3, feed.Document.Channel.Items.Count); i++ )
                {
                    items.Add(feed.Document.Channel.Items[i]);
                }
                
            }

            
            RecentNews.DataSource = items;
            RecentNews.DataBind();
        }
    }

    private void GetTotals()
    {
        CategoryPermissionCheck callback = (_rolePermissionService.GetPermissions);
        List<PostCount> counts = _postService.GetPostCounts(GraffitiUsers.Current, 0, GraffitiUsers.Current.Name, callback);

        PostCount published = counts.Find(
                                        delegate(PostCount p)
                                        {
                                            return p.PostStatus == PostStatus.Publish;
                                        });

        if (published != null)
        {   
            if (published.Count == 1)
                lblTotalPublished.Text = " <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/posts/") + "\">1</a></span> item ";
            else
                lblTotalPublished.Text = " <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/posts/") + "\">" + published.Count.ToString() + "</a></span> items ";
        }
        else
        {
            lblTotalPublished.Text = " <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/posts/") + "\">0</a></span> items ";
        }

        int c = _commentService.GetPublishedCommentCount(GraffitiUsers.Current.Name);
        
        if (c == 1)
            lblTotalComments.Text = " <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/comments/") + "\">1</a></span> comment";
        else
            lblTotalComments.Text = "<span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/comments/") + "\">" + c.ToString() + "</a></span> comments";


        IList<Comment> cc = _commentService.FetchComments().Where(x => !x.IsPublished && !x.IsDeleted).ToList();

        PendingCommentList.DataSource = cc;
        PendingCommentList.DataBind();

        if (cc != null)
        {
            CommentCount.Value = cc.Count.ToString();
            
            if (cc.Count == 1)
                lblTotalCommentsPending.Text = " is <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/comments/?a=f") + "\">1</a></span> comment ";
            else
                lblTotalCommentsPending.Text = " are <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/comments/?a=f") + "\">" + cc.Count.ToString() + "</a></span> comments ";
        }
        else
        {
            lblTotalCommentsPending.Text = " are <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/comments/?a=f") + "\">0</a></span> comments ";
        }
    }

	private void GetWaitingApprovals()
	{
		WaitingApproval.Visible = true;

        IList<Post> pc = _postService.FetchPosts().Where(x => !x.IsDeleted && x.PostStatus == PostStatus.PendingApproval).ToList();

		PendingPostList.DataSource = pc;
		PendingPostList.DataBind();

        if (pc != null)
        {
            if (pc.Count == 1)
                lblTotalPending.Text = " is <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/posts/?status=3") + "\">1</a></span> item ";
            else
                lblTotalPending.Text = " are <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/posts/?status=3") + "\">" + pc.Count.ToString() + "</a></span> items ";
        }
        else
        {
            lblTotalPending.Text = " are <span class=\"largeNumberText\"><a style=\"text-decoration: none; color: #000;\" href=\"" + ResolveUrl("~/graffiti-admin/posts/?status=3") + "\">0</a></span> items ";
        }
    }

</script>