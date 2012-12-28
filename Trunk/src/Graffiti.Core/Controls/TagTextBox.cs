using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

[assembly: WebResource("Graffiti.Core.Controls.TagTextBox.js", "text/javascript")]

namespace Graffiti.Core
{
	public class TagTextBox : TextBox
	{
		private HtmlInputHidden _tagsList;

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			_tagsList = new HtmlInputHidden();
			_tagsList.Name = "alltags";
			_tagsList.ID = "alltags";

			StringBuilder tagList = new StringBuilder();
			var renderedTags = new Dictionary<string, bool>();
			foreach (Tag tag in TagCollection.FetchAll())
			{
				if (!renderedTags.ContainsKey(tag.Name))
				{
					if (tagList.Length > 0)
						tagList.Append("&");

					tagList.Append(HttpUtility.UrlEncode(HttpUtility.HtmlDecode(tag.Name)));
					tagList.Append("&");
					tagList.Append(HttpUtility.UrlEncode(tag.Name));

					renderedTags[tag.Name] = true;
				}
			}
			_tagsList.Value = tagList.ToString();

			Controls.Add(_tagsList);

			EnsureID();
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			EnsureChildControls();

			base.RenderEndTag(writer);

			_tagsList.RenderControl(writer);
		}

		protected override void OnPreRender(EventArgs e)
		{
			EnsureChildControls();

			base.OnPreRender(e);

			Page.ClientScript.RegisterClientScriptResource(typeof (TagTextBox), "Graffiti.Core.Controls.TagTextBox.js");
			Page.ClientScript.RegisterStartupScript(typeof (TagTextBox), ClientID,
			                                        string.Format(
				                                        "<script type=\"text/javascript\">\n// <![CDATA[\nvar {0} = new TagTextBox('{0}','{1}','{2}');\n// ]]>\n</script>",
				                                        ClientID,
				                                        _tagsList.ClientID,
				                                        ClientID),
			                                        false);
		}
	}
}