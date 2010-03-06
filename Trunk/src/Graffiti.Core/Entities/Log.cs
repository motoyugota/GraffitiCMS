using System;
using Graffiti.Core.Services;

namespace Graffiti.Core
{
    [Serializable]
    public class Log
    {

        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        #region Events

        //protected override void AfterCommit()
        //{
            
        //}

        //protected override void BeforeInsert()
        //{
            
        //}

        //protected override void BeforeUpdate()
        //{
            
        //}

        //protected override void BeforeValidate()
        //{
            
        //}

        #endregion

        #region Static Log Helpers

        public static void Info(string title, string messsage)
        {
            ILogService _logService = ServiceLocator.Get<ILogService>();
            _logService.QuickSave(1,title,messsage);
        }

        public static void Info(string title, string messsageFormat, params object[] details)
        {
            ILogService _logService = ServiceLocator.Get<ILogService>();
            _logService.DetailedSave(1, title, messsageFormat, details);
        }

        public static void Warn(string title, string messsage)
        {
            ILogService _logService = ServiceLocator.Get<ILogService>();
            _logService.QuickSave(2, title, messsage);
        }

        public static void Warn(string title, string messsageFormat, params object[] details)
        {
            ILogService _logService = ServiceLocator.Get<ILogService>();
            _logService.DetailedSave(2, title, messsageFormat, details);
        }

        public static void Error(string title, string messsage)
        {
            ILogService _logService = ServiceLocator.Get<ILogService>();
            _logService.QuickSave(3, title, messsage);
        }

        public static void Error(string title, string messsageFormat, params object[] details)
        {
            ILogService _logService = ServiceLocator.Get<ILogService>();
            _logService.DetailedSave(3, title, messsageFormat, details);
        }

        #endregion


    }
}
