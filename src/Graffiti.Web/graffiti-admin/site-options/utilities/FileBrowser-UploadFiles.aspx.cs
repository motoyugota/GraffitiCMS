using System;
using System.IO;
using System.Web.UI;
using Graffiti.Core;
using Telligent.Glow;

namespace Graffiti.Web
{
	public partial class FileBrowser_UploadFiles : Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.Request.Browser.IsBrowser("IE") || Page.Request.Browser.IsBrowser("Safari") ||
			    Page.Request.Browser.IsBrowser("Opera") || Page.Request.Browser.IsBrowser("Gecko"))
			{
				IGraffitiUser user = GraffitiUsers.Current;
				if (user.UniqueId == Guid.Empty)
				{
					GraffitiUsers.Save(user, user.Name);
					user = GraffitiUsers.GetUser(user.Name, true);
				}

				Files.UploadUrlQueryString = "Username=" + Server.UrlEncode(user.Name) + "&Ticket=" +
				                             Server.UrlEncode(user.UniqueId.ToString());
			}
		}

		protected void Save_Click(object sender, EventArgs e)
		{
			if (Files.UploadedFiles != null)
			{
				string path = Path.Combine(
					Server.MapPath("~/")
					, Util.NormalizePath(Request.QueryString["path"])
					);

				try
				{
					foreach (UploadedFile file in Files.UploadedFiles)
					{
						var content = new byte[file.InputStream.Length];
						file.InputStream.Read(content, 0, content.Length);
						File.WriteAllBytes(Path.Combine(path, file.FileName), content);
						file.InputStream.Close();
					}

					Files.ClearUploadedFiles();
				}
				catch (Exception ex)
				{
					lblError.Text = ex.Message;

					try
					{
						Files.ClearUploadedFiles();
					}
					catch
					{
					}

					return;
				}
			}

			FormWrapper.Visible = false;
			ClientScript.RegisterStartupScript(GetType(), "close",
			                                   "window.parent.location.href = window.parent.location;", true);
		}
	}
}