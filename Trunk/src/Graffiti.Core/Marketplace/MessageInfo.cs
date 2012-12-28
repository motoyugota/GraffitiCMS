using System.Xml.Linq;

namespace Graffiti.Core.Marketplace
{
	public class MessageInfo
	{
		private string _text = string.Empty;
		private string _title = string.Empty;

		public MessageInfo(XElement node)
		{
			string value;

			if (node.TryGetAttributeValue("id", out value))
				Id = int.Parse(value);

			XElement n = node.Element("title");
			if (n != null && n.TryGetValue(out value))
				_title = value;

			n = node.Element("text");
			if (n != null && n.TryGetValue(out value))
				_text = value;
		}

		public int Id { get; set; }

		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
	}
}