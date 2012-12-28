using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Graffiti.Core;
using Graffiti.Core.API;
using Telligent.DynamicConfiguration.Components;
using Telligent.DynamicConfiguration.Controls;

namespace Graffiti.Web.graffiti_admin.presentation.themes
{
	public partial class ConfigureTheme : AdminControlPanelPage
	{
		protected PlaceHolder ConfigWrapper;
		protected ConfigurationForm ConfigurationForm;
		protected PlaceHolder NoConfigWrapper;
		protected LinkButton RestoreDefaults;
		protected Button Save;

		protected void Page_Load(object sender, EventArgs e)
		{
			LiHyperLink.SetNameToCompare(Context, "presentation");
			DataBind();
		}

		public override void DataBind()
		{
			ThemeConfigurationData themeConfig =
				ObjectManager.Get<ThemeConfigurationData>(
					ThemeConfigurationData.GetKey(Request.QueryString[QueryStringKey.Theme]));
			themeConfig.Theme = Request.QueryString[QueryStringKey.Theme];

			ConfigurationForm.ConfigurationData = themeConfig;

			bool hasGroups = false;
			if (themeConfig.PropertyGroups != null)
			{
				foreach (PropertyGroup group in themeConfig.PropertyGroups)
				{
					if (group.Visible)
					{
						hasGroups = true;
						break;
					}
				}
			}

			Save.Visible = hasGroups;
			ConfigWrapper.Visible = hasGroups;
			NoConfigWrapper.Visible = !hasGroups;

			if (!hasGroups)
				SetMessage(StatusType.Warning, "The selected theme does not require any additional configuration.");

			base.DataBind();
		}

		protected void Save_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				ConfigurationForm.Commit();

				ConfigurationForm.ReloadControlValuesFromConfigurationData();

				SetMessage(StatusType.Success, "Your changes have been successfully saved.");
			}
			else
				SetMessage(StatusType.Error, "Please check your configuration.  There was an issue while saving.");
		}

		protected void RestoreDefaults_Click(object sender, EventArgs e)
		{
			ThemeConfigurationData data = ConfigurationForm.ConfigurationData as ThemeConfigurationData;
			if (data != null)
			{
				foreach (PropertyGroup group in data.PropertyGroups)
				{
					foreach (Property property in group.GetAllProperties())
					{
						data.ClearValue(property);
					}
				}

				ConfigurationForm.ReloadControlValuesFromConfigurationData();

				ConfigurationForm.Commit();

				SetMessage(StatusType.Success, "The default values have been restored and saved.");
			}
		}

		protected void SetMessage(StatusType type, string message)
		{
			SetMessageRecursive(type, message, this);
		}

		protected void SetMessageRecursive(StatusType type, string message, Control parent)
		{
			foreach (Control c in parent.Controls)
			{
				if (c is StatusMessage)
				{
					StatusMessage sm = (StatusMessage) c;
					sm.Text = message;
					sm.Type = type;
				}
				else if (c.Controls.Count > 0)
					SetMessageRecursive(type, message, c);
			}
		}
	}
}