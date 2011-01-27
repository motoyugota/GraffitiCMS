using System;
using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface ILogRepository 
    {
        IQueryable<Log> FetchByType(string type, int pageIndex, int pageSize, out int totalCount);
        void RemoveLogsOlderThan(int hours);
        Log SaveLog(Log log, string username, DateTime modifiedTime);
    }
}
