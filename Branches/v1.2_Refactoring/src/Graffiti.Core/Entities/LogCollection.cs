using System;
using System.Collections.Generic;

namespace Graffiti.Core
{
    [Serializable]
    public class LogCollection : List<Log> 
    {
        public LogCollection() : base(new List<Log>()) { }

        public LogCollection(IEnumerable<Log> logs)
        {
            this.AddRange(logs);
        }
    }
}
