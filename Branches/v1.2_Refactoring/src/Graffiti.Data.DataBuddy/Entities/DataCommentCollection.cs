using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataCommentCollection : List<DataComment> 
    {
        /// <summary>
        /// Hydrates a collection of Comment. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of Comment. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataComment obj = new DataComment();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of Comment
        /// </summary>
        public static DataCommentCollection FetchAll() {
            Query q = DataComment.CreateQuery();
            DataCommentCollection itemCollection = new DataCommentCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Comment based on the supplied query
        /// </summary>
        public static DataCommentCollection FetchByQuery(Query q) {
            DataCommentCollection itemCollection = new DataCommentCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Comment based on the supplied query
        /// </summary>
        public static DataCommentCollection FetchByColumn(Column column, object value) {
            DataCommentCollection itemCollection = new DataCommentCollection();
            Query q = DataComment.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
