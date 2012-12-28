using System.Xml.Linq;

namespace Graffiti.Core.Marketplace
{
	public class CategoryInfo
	{
		private string _description = string.Empty;
		private ItemInfoCollection _items;
		private string _name = string.Empty;

		public CategoryInfo(CatalogInfo catalog, XElement node)
		{
			Catalog = catalog;

			string value;

			if (node.TryGetAttributeValue("id", out value))
				Id = int.Parse(value);

			XElement n = node.Element("name");
			if (n != null && n.TryGetValue(out value))
				_name = value;

			n = node.Element("description");
			if (n != null && n.TryGetValue(out value))
				_description = value;
		}

		public CatalogInfo Catalog { get; set; }

		public int Id { get; set; }

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

		public ItemInfoCollection Items
		{
			get
			{
				if (_items == null)
				{
					_items = new ItemInfoCollection();
					foreach (ItemInfo item in Catalog.Items.Values)
					{
						if (item.CategoryId == Id)
							_items.Add(item.Id, item);
					}
				}

				return _items;
			}
		}
	}
}