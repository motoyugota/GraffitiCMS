using System;
using System.Web.UI;

namespace Graffiti.Core
{
    public class GraffitiPage : Page
    {
        protected virtual void Authenticate()
        {
        }

        protected void LicenseRequires(LicenseType type)
        {
            if (!GraffitiLicense.IsLicenseValid(type))
            {
                Response.Clear();
                Response.Write("<h1>Invalid License Requirements for this feature</h1>");
                Response.End();
            }
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

            if (!RolePermissionManager.CanViewControlPanel(GraffitiUsers.Current) && !GraffitiUsers.IsAdmin(GraffitiUsers.Current))
                Response.Redirect("~/");
        }

    }

    public class ManagerControlPanelPage : ControlPanelPage
    {
        protected override void Authenticate()
        {
            if (GraffitiUsers.Current == null)
                Response.Redirect("~/login/");

            if (!RolePermissionManager.CanViewControlPanel(GraffitiUsers.Current) && !GraffitiUsers.IsAdmin(GraffitiUsers.Current))
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
