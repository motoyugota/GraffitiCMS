using System;
using System.Web;

namespace Graffiti.Core
{
    [Serializable]
    public class TagWeight
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public string FontSize { get; set; }
        public int FontFactor { get; set; }
      
        public string VirtualUrl
        {
            get { return "~/tags/" + Name.ToLower() + "/"; }
        }

        public string Url
        {
            get { return VirtualPathUtility.ToAbsolute(VirtualUrl); }
        }
	
        public int Count
        {
            get { return Weight; }
        }
	
    }
}
