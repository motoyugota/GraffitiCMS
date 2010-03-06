using System;
using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class LogService : ILogService
    {
        private ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public IList<Log> FetchByType(string type, int pageIndex, int pageSize, out int totalCount)
        {
            return _logRepository.FetchByType(type, pageIndex, pageSize, out totalCount).ToList();
        }

        public void RemoveLogsOlderThan(int hours)
        {
            _logRepository.RemoveLogsOlderThan(hours);
        }

        public void DetailedSave(int type, string title, string messageFormat, params object[] details) 
        {
            try {
                QuickSave(type, title, string.Format(messageFormat, details));
            } catch //need to make sure we throw no errors here
            {
            }
        }

        public void QuickSave(int type, string title, string message) 
        {
            Log l = new Log();
            l.Type = type;
            l.Title = title;
            l.Message = message;
            l = _logRepository.SaveLog(l, "", DateTime.Now.AddHours(SiteSettings.Get().TimeZoneOffSet));
        }

    }
}
