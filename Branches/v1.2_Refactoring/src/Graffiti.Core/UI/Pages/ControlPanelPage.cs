using System;
using System.Linq;
using System.Web.UI;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
    public class GraffitiPage : Page
    {
        protected IRolePermissionService _rolePermissionService = ServiceLocator.Get<IRolePermissionService>();
        protected IPostService _postService = ServiceLocator.Get<IPostService>();
        protected ICommentService _commentService = ServiceLocator.Get<ICommentService>();
        protected ICategoryService _categoryService = ServiceLocator.Get<ICategoryService>();
        protected IVersionStoreService _versionStoreService = ServiceLocator.Get<IVersionStoreService>();

        protected virtual void Authenticate()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            Authenticate();
            base.OnInit(e);
        }
    }

    public class ControlPanelPage : GraffitiPage
    {
 
        protected override void Authenticate()
        {
            if (GraffitiUsers.Current == null)
                Response.Redirect("~/login/");

            if (!_rolePermissionService.CanViewControlPanel(GraffitiUsers.Current) && !GraffitiUsers.IsAdmin(GraffitiUsers.Current))
                Response.Redirect("~/");
        }

    }

    public class ManagerControlPanelPage : ControlPanelPage
    {
        protected override void Authenticate()
        {
            if (GraffitiUsers.Current == null)
                Response.Redirect("~/login/");

            if (!_rolePermissionService.CanViewControlPanel(GraffitiUsers.Current) && !GraffitiUsers.IsAdmin(GraffitiUsers.Current))
                Response.Redirect("~/");   
        }
    }

    public class AdminControlPanelPage : ControlPanelPage
    {
        protected override void Authenticate()
        {
            if (GraffitiUsers.Current == null)
                Response.Redirect("~/login/");

            if (!GraffitiUsers.IsAdmin(GraffitiUsers.Current))
                Response.Redirect("~/");
        }
    }
}
