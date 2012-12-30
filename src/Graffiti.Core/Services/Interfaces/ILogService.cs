using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface ILogService 
    {
        IList<Log> FetchByType(string type, int pageIndex, int pageSize, out int totalCount);
        void RemoveLogsOlderThan(int hours);
        void DetailedSave(int type, string title, string messageFormat, params object[] details);
        void QuickSave(int type, string title, string message);
    }
}
