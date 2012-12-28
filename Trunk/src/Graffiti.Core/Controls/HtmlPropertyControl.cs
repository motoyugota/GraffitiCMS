using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telligent.DynamicConfiguration.Components;

namespace Graffiti.Core
{
	public class HtmlPropertyControl : GraffitiEditor, IPropertyControl
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			ToolbarSet = "Simple";
			Width = new Unit(600);

			if (!string.IsNullOrEmpty(ConfigurationProperty.Attributes["width"]))
				Width = Unit.Parse(ConfigurationProperty.Attributes["width"]);

			if (!string.IsNullOrEmpty(ConfigurationProperty.Attributes["height"]))
				Height = Unit.Parse(ConfigurationProperty.Attributes["height"]);
		}

		#region IPropertyControl Members

		public Property ConfigurationProperty { get; set; }

		public ConfigurationDataBase ConfigurationData { get; set; }

		public void SetConfigurationPropertyValue(object value)
		{
			Text = value == null ? string.Empty : value.ToString();
		}

		public object GetConfigurationPropertyValue()
		{
			if (string.IsNullOrEmpty(Text))
				return string.Empty;
			else
				return Text;
		}

		public event ConfigurationPropertyChanged ConfigurationValueChanged
		{
			add { throw new InvalidOperationException("The HtmlPropertyControl does not support the ConfigurationValueChanged event"); }
			remove { throw new InvalidOperationException("The HtmlPropertyControl does not support the ConfigurationValueChanged event"); }
		}

		public Control Control
		{
			get { return this; }
		}

		#endregion
	}
}