using System.Xml.Linq;

namespace Graffiti.Core.Marketplace
{
	public class StatisticsInfo
	{
		public StatisticsInfo(XElement node)
		{
			string value;

			XElement n = node.Element("downloadCount");
			if (n != null && n.TryGetValue(out value))
				DownloadCount = int.Parse(value);

			n = node.Element("viewCount");
			if (n != null && n.TryGetValue(out value))
				ViewCount = int.Parse(value);
		}

		public int DownloadCount { get; set; }

		public int ViewCount { get; set; }
	}
}