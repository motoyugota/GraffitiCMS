using System;
using System.Collections.Specialized;
using System.IO;
using System.Web.Caching;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Telligent.DynamicConfiguration.Components;
using Telligent.DynamicConfiguration.Controls;

namespace Graffiti.Core
{
	[Serializable]
	public class ThemeConfigurationData : ConfigurationDataBase, IXmlConfigurationParserHelper, IXmlSerializable
	{
		private NameValueCollection _values;

		#region Constructors

		public ThemeConfigurationData()
		{
			_theme = "default";
			_values = new NameValueCollection();
		}

		#endregion

		#region Public Properties

		private string _theme;

		public string Theme
		{
			get { return _theme; }
			set { _theme = value; }
		}

		public string Key
		{
			get { return GetKey(Theme); }
		}

		#endregion

		#region ConfigurationDataBase Implementation

		public override PropertyGroup[] PropertyGroups
		{
			get
			{
				string key = Key + "-Groups";
				var groups = ZCache.Get<PropertyGroup[]>(key);

				if (groups == null)
				{
					groups = ParsePropertyGroups();
					ZCache.MaxCache(key, groups, new CacheDependency(ViewManager.GetThemePath(Theme)));
				}

				return groups;
			}
		}

		public override ConfigurationDataBase Clone()
		{
			ThemeConfigurationData data2 = new ThemeConfigurationData();

			data2.Theme = Theme;
			foreach (string key in _values.Keys)
			{
				data2.SetValueInternal(key, _values[key]);
			}

			return data2;
		}

		public override string GetResourceOrText(string resourceName, string resourceFile, string text)
		{
			// Graffiti doesn't currently support language resources
			return text;
		}

		protected override string GetValueInternal(string propertyName, string defaultValue)
		{
			return _values[propertyName] ?? defaultValue;
		}

		protected override void InternalCommit()
		{
			ObjectManager.Save(this, Key);
		}

		protected override void SetValueInternal(string propertyName, string value)
		{
			_values[propertyName] = value;
		}

		#endregion

		#region MetaData Loading Methods

		private PropertyGroup[] ParsePropertyGroups()
		{
			string path = Path.Combine(ViewManager.GetThemePath(Theme), "theme.config");
			if (File.Exists(path))
			{
				try
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(path);

					XmlNode node = doc.SelectSingleNode("Theme/DynamicConfiguration");
					if (node != null)
						return PropertyGroup.ParseAll(node.ChildNodes, this);
				}
				catch
				{
					// ignore
				}
			}

			return new PropertyGroup[0];
		}

		#endregion

		public static string GetKey(string theme)
		{
			return "ThemeConfigurationData-" + theme;
		}

		#region IXmlSerializable Members

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			Theme = reader.GetAttribute("Theme");

			_values = new NameValueCollection();
			while (reader.MoveToNextAttribute())
			{
				_values[reader.Name] = reader.Value;
			}

			reader.Read();
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Theme", Theme);
			foreach (PropertyGroup group in PropertyGroups)
			{
				foreach (Property property in group.GetAllProperties())
				{
					if (_values[property.ID] != null)
						writer.WriteAttributeString(property.ID, _values[property.ID]);
				}
			}
		}

		#endregion

		#region IXmlConfigurationParserHelper Methods

		public void PropertyGroupParsed(XmlNode node, PropertyGroup group)
		{
		}

		public void PropertySubGroupParsed(XmlNode node, PropertySubGroup subGroup)
		{
		}

		public void PropertyParsed(XmlNode node, Property property)
		{
			if (node.Attributes["controlType"] == null)
			{
				if (node.Attributes["dataType"] != null)
				{
					switch (node.Attributes["dataType"].Value.ToLower())
					{
						case "html":
							property.DataType = PropertyType.String;
							property.ControlType = typeof (HtmlPropertyControl);
							break;

						case "file":
						case "url":
							property.DataType = PropertyType.Url;
							property.ControlType = typeof (FilePropertyControl);
							break;

						case "multilinestring":
							property.DataType = PropertyType.String;
							property.ControlType = typeof (MultilineStringControl);
							break;
					}
				}
			}
		}

		public void PropertyValueParsed(XmlNode node, PropertyValue value)
		{
		}

		public void PropertyRuleParsed(XmlNode node, PropertyRule rule)
		{
		}

		#endregion
	}
}