using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Graffiti.Core.Marketplace
{
	public class ItemInfo
	{
		private string _creatorId = string.Empty;
		private DateTime _dateAdded = DateTime.MinValue;
		private string _description = string.Empty;
		private string _downloadUrl = string.Empty;
		private string _fileName = string.Empty;
		private string _iconUrl = string.Empty;
		private string _name = string.Empty;
		private string _screenshotUrl = string.Empty;
		private List<string> _tags = new List<string>();
		private string _version = string.Empty;

		public ItemInfo(CatalogInfo catalog, XElement node)
		{
			Catalog = catalog;

			string value;

			if (node.TryGetAttributeValue("id", out value))
				Id = int.Parse(value);
			if (node.TryGetAttributeValue("categoryId", out value))
				CategoryId = int.Parse(value);
			if (node.TryGetAttributeValue("creatorId", out value))
				_creatorId = value;

			XElement n = node.Element("name");
			if (n != null && n.TryGetValue(out value))
				_name = value;

			n = node.Element("description");
			if (n != null && n.TryGetValue(out value))
				_description = value;

			n = node.Element("version");
			if (n != null && n.TryGetValue(out value))
				_version = value;

			n = node.Element("downloadUrl");
			if (n != null && n.TryGetValue(out value))
				_downloadUrl = value;

			n = node.Element("fileName");
			if (n != null && n.TryGetValue(out value))
				_fileName = value;

			n = node.Element("screenshotUrl");
			if (n != null && n.TryGetValue(out value))
				_screenshotUrl = value;

			n = node.Element("iconUrl");
			if (n != null && n.TryGetValue(out value))
				_iconUrl = value;

			n = node.Element("worksWithMajorVersion");
			if (n != null && n.TryGetValue(out value))
				WorksWithMajorVersion = int.Parse(value);

			n = node.Element("worksWithMinorVersion");
			if (n != null && n.TryGetValue(out value))
				WorksWithMinorVersion = int.Parse(value);

			n = node.Element("requiresManualIntervention");
			if (n != null && n.TryGetValue(out value))
				RequiresManualIntervention = bool.Parse(value);

			n = node.Element("dateAdded");
			if (n != null && n.TryGetValue(out value))
				_dateAdded = DateTime.Parse(value);

			n = node.Element("statisticsInfo");
			if (n != null)
				Statistics = new StatisticsInfo(n);

			n = node.Element("purchaseInfo");
			if (n != null)
				Purchase = new PurchaseInfo(n);

			n = node.Element("tags");
			if (n != null)
			{
				foreach (XElement e in n.Elements("tag"))
				{
					string tag;
					if (e.TryGetValue(out tag))
						_tags.Add(tag);
				}
			}
		}

		public CatalogInfo Catalog { get; set; }

		public int Id { get; set; }

		public int CategoryId { get; set; }

		public CategoryInfo Category
		{
			get
			{
				if (Catalog.Categories.ContainsKey(CategoryId))
					return Catalog.Categories[CategoryId];
				return null;
			}
		}

		public string CreatorId
		{
			get { return _creatorId; }
			set { _creatorId = value; }
		}

		public CreatorInfo Creator
		{
			get
			{
				if (Marketplace.Creators.ContainsKey(CreatorId))
					return Marketplace.Creators[CreatorId];
				return null;
			}
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public string Version
		{
			get { return _version; }
			set { _version = value; }
		}

		public string DownloadUrl
		{
			get { return _downloadUrl; }
			set { _downloadUrl = value; }
		}

		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public string ScreenshotUrl
		{
			get { return _screenshotUrl; }
			set { _screenshotUrl = value; }
		}

		public string IconUrl
		{
			get { return _iconUrl; }
			set { _iconUrl = value; }
		}

		public int WorksWithMajorVersion { get; set; }

		public int WorksWithMinorVersion { get; set; }

		public bool RequiresManualIntervention { get; set; }

		public string Tags
		{
			get { return string.Join(", ", _tags.ToArray()); }
		}

		public DateTime DateAdded
		{
			get { return _dateAdded; }
			set { _dateAdded = value; }
		}

		public StatisticsInfo Statistics { get; set; }

		public PurchaseInfo Purchase { get; set; }
	}
}