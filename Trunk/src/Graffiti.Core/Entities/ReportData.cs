using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graffiti.Core 
{
    public class ReportData 
    {
        public IDictionary<int, int> Counts = new Dictionary<int, int>();
        public IDictionary<int, string> Titles = new Dictionary<int, string>();
    }
}
