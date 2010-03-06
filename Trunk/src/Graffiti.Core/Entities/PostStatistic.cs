using System;

namespace Graffiti.Core
{
    [Serializable]
    public class PostStatistic 
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public DateTime DateViewed { get; set; }
    }
}
