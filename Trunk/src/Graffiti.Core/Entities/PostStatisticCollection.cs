using System;
using System.Collections.Generic;

namespace Graffiti.Core 
{
    [Serializable]
    public class PostStatisticCollection : List<PostStatistic> 
    {
        public PostStatisticCollection() : base(new List<PostStatistic>()) { }

        public PostStatisticCollection(IEnumerable<PostStatistic> postStatistics)
        {
            this.AddRange(postStatistics);
        }
    }
}
