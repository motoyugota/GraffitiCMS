using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataLog : DataBuddyBase
    {
        private static readonly Table _Table = null;

        static DataLog() {
            _Table = new Table("graffiti_Logs", "Log");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("Type", DbType.Int32, typeof(System.Int32), "Type", false, false));
            _Table.Columns.Add(new Column("Title", DbType.String, typeof(System.String), "Title", false, false));
            _Table.Columns.Add(new Column("Message", DbType.String, typeof(System.String), "Message", false, false));
            _Table.Columns.Add(new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", false, false));
            _Table.Columns.Add(new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false));
        }

        /// <summary>
        /// Fetches an instance of Log based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataLog FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataLog FetchByQuery(Query q) {
            DataLog item = new DataLog();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type Log
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataLog() { }
        /// <summary>
        /// Loads an instance of Log for the supplied primary key value
        /// </summary>
        public DataLog(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of Log. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of Log. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            Type = DataService.GetValue<System.Int32>(Columns.Type, reader);
            Title = DataService.GetValue<System.String>(Columns.Title, reader);
            Message = DataService.GetValue<System.String>(Columns.Message, reader);
            CreatedBy = DataService.GetValue<System.String>(Columns.CreatedBy, reader);
            CreatedOn = DataService.GetValue<System.DateTime>(Columns.CreatedOn, reader);
            Loaded();
            ResetStatus();

            if (close)
                reader.Close();
        }

        #region public System.Int32 Id

        private System.Int32 _Id;

        public System.Int32 Id {
            get { return _Id; }
            set { MarkDirty(); _Id = value; }
        }

        #endregion

        #region public System.Int32 Type

        private System.Int32 _Type;

        public System.Int32 Type {
            get { return _Type; }
            set { MarkDirty(); _Type = value; }
        }

        #endregion

        #region public System.String Title

        private System.String _Title;

        public System.String Title {
            get { return _Title; }
            set { MarkDirty(); _Title = value; }
        }

        #endregion

        #region public System.String Message

        private System.String _Message;

        public System.String Message {
            get { return _Message; }
            set { MarkDirty(); _Message = value; }
        }

        #endregion

        #region public System.String CreatedBy

        private System.String _CreatedBy;

        public System.String CreatedBy {
            get { return _CreatedBy; }
            set { MarkDirty(); _CreatedBy = value; }
        }

        #endregion

        #region public System.DateTime CreatedOn

        private System.DateTime _CreatedOn;

        public System.DateTime CreatedOn {
            get { return _CreatedOn; }
            set { MarkDirty(); _CreatedOn = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents Log
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent Log
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column Type = new Column("Type", DbType.Int32, typeof(System.Int32), "Type", false, false);
            public static readonly Column Title = new Column("Title", DbType.String, typeof(System.String), "Title", false, false);
            public static readonly Column Message = new Column("Message", DbType.String, typeof(System.String), "Message", false, false);
            public static readonly Column CreatedBy = new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", false, false);
            public static readonly Column CreatedOn = new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false);
        }


        public static int Destroy(Column column, object value) {
            DataLog objectToDelete = FetchByColumn(column, value);
            if (!objectToDelete.IsNew) {
                objectToDelete.BeforeRemove(true);
                int i = DataService.Destroy(Table, column, value);
                objectToDelete.AfterRemove(true);
                return i;
            }

            return 0;
        }


        public static int Destroy(object value) {
            return Destroy(Columns.Id, value);
        }
        protected override void SetPrimaryKey(int pkID) {
            Id = pkID;
        }

        protected override Table GetTable() {
            return Table;
        }

        public static Parameter FindParameter(List<Parameter> parameters, string name) {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

            return parameters.Find(delegate(Parameter p) { return (p.Name == name); });
        }

        public static Parameter FindParameter(string name) {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

            return GenerateParameters().Find(delegate(Parameter p) { return (p.Name == name); });
        }

        #region public static List<Parameter> GenerateParameters()

        public static List<Parameter> GenerateParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            parameters.Add(new Parameter("Id", null, DbType.Int32));

            parameters.Add(new Parameter("Type", null, DbType.Int32));

            Parameter pTitle = new Parameter("Title", null, DbType.String);
            pTitle.Length = 255;
            parameters.Add(pTitle);

            Parameter pMessage = new Parameter("Message", null, DbType.String);
            pMessage.Length = 2000;
            parameters.Add(pMessage);

            Parameter pCreatedBy = new Parameter("CreatedBy", null, DbType.String);
            pCreatedBy.Length = 128;
            parameters.Add(pCreatedBy);

            parameters.Add(new Parameter("CreatedOn", null, DbType.DateTime));

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            if (!IsNew) {
                parameters.Add(new Parameter("Id", Id, DbType.Int32));
            }

            parameters.Add(new Parameter("Type", Type, DbType.Int32));

            Parameter pTitle = new Parameter("Title", Title, DbType.String);
            pTitle.Length = 255;
            parameters.Add(pTitle);

            Parameter pMessage = new Parameter("Message", Message, DbType.String);
            pMessage.Length = 2000;
            parameters.Add(pMessage);

            Parameter pCreatedBy = new Parameter("CreatedBy", CreatedBy, DbType.String);
            pCreatedBy.Length = 128;
            parameters.Add(pCreatedBy);

            parameters.Add(new Parameter("CreatedOn", CreatedOn, DbType.DateTime));

            return parameters;
        }
        
        protected override void AfterCommit()
        {
            
        }

        protected override void BeforeInsert()
        {
            
        }

        protected override void BeforeUpdate()
        {
            
        }

        protected override void BeforeValidate()
        {
            
        }

        public static void Info(string title, string messsage)
        {
            QuickSave(1,title,messsage);
        }

        public static void Info(string title, string messsageFormat, params object[] details)
        {
            DetailedSave(1,title,messsageFormat,details);
        }

        public static void Warn(string title, string messsage)
        {
            QuickSave(2, title, messsage);
        }

        public static void Warn(string title, string messsageFormat, params object[] details)
        {
            DetailedSave(2, title, messsageFormat, details);
        }

        public static void Error(string title, string messsage)
        {
            QuickSave(3, title, messsage);
        }

        public static void Error(string title, string messsageFormat, params object[] details)
        {
            DetailedSave(3, title, messsageFormat, details);
        }

        private static void DetailedSave(int type, string title, string messageFormat, params object[] details)
        {
            try
            {
                QuickSave(type, title, string.Format(messageFormat, details));
            }
            catch //need to make sure we throw no errors here
            {
            }
        }

        private static void QuickSave(int type, string title,string message)
        {
            DataLog l = new DataLog();
            l.Type = type;
            l.Title = title;
            l.Message = message;
            l.Save("", DateTime.Now.AddHours(SiteSettings.Get().TimeZoneOffSet));            
        }

        public static void RemoveLogsOlderThan(int hours)
        {
            DateTime dt = DateTime.Now.AddHours(-1*hours);

			QueryCommand command = new QueryCommand("DELETE FROM graffiti_Logs WHERE CreatedOn <= " + DataService.Provider.SqlVariable("CreatedOn"));
			command.Parameters.Add(DataLog.FindParameter("CreatedOn")).Value = dt;
            int i = DataService.ExecuteNonQuery(command);
            if(i > 0)
                Info("Log", "{0} item(s) were just removed from the logs",i);
        }
    }
}
