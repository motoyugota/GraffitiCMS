using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DataBuddy;
using System.Data;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataPost : DataBuddyBase
    {
		private static readonly Table _Table = null;

		static DataPost ()
		{
			_Table = new Table("graffiti_Posts", "Post");
			_Table.IsReadOnly = false;
			_Table.PrimaryKey = "Id";
			_Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
			_Table.Columns.Add(new Column("Title", DbType.String, typeof(System.String), "Title", false, false));
			_Table.Columns.Add(new Column("PostBody", DbType.String, typeof(System.String), "PostBody", false, false));
			_Table.Columns.Add(new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false));
			_Table.Columns.Add(new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false));
			_Table.Columns.Add(new Column("Status", DbType.Int32, typeof(System.Int32), "Status", false, false));
			_Table.Columns.Add(new Column("Content_Type", DbType.String, typeof(System.String), "ContentType", false, false));
			_Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
			_Table.Columns.Add(new Column("Comment_Count", DbType.Int32, typeof(System.Int32), "CommentCount", false, false));
			_Table.Columns.Add(new Column("Tag_List", DbType.String, typeof(System.String), "TagList", true, false));
			_Table.Columns.Add(new Column("CategoryId", DbType.Int32, typeof(System.Int32), "CategoryId", false, false));
			_Table.Columns.Add(new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false));
			_Table.Columns.Add(new Column("ModifiedBy", DbType.String, typeof(System.String), "ModifiedBy", true, false));
			_Table.Columns.Add(new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", true, false));
			_Table.Columns.Add(new Column("ExtendedBody", DbType.String, typeof(System.String), "ExtendedBody", false, false));
			_Table.Columns.Add(new Column("IsDeleted", DbType.Boolean, typeof(System.Boolean), "IsDeleted", false, false));
			_Table.Columns.Add(new Column("Published", DbType.DateTime, typeof(System.DateTime), "Published", false, false));
			_Table.Columns.Add(new Column("Pending_Comment_Count", DbType.Int32, typeof(System.Int32), "PendingCommentCount", false, false));
			_Table.Columns.Add(new Column("Views", DbType.Int32, typeof(System.Int32), "Views", false, false));
			_Table.Columns.Add(new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false));
			_Table.Columns.Add(new Column("EnableComments", DbType.Boolean, typeof(System.Boolean), "EnableComments", false, false));
			_Table.Columns.Add(new Column("PropertyKeys", DbType.String, typeof(System.String), "PropertyKeys", true, false));
			_Table.Columns.Add(new Column("PropertyValues", DbType.String, typeof(System.String), "PropertyValues", true, false));
			_Table.Columns.Add(new Column("UserName", DbType.String, typeof(System.String), "UserName", false, false));
			_Table.Columns.Add(new Column("Notes", DbType.String, typeof(System.String), "Notes", true, false));
			_Table.Columns.Add(new Column("ImageUrl", DbType.String, typeof(System.String), "ImageUrl", true, false));
			_Table.Columns.Add(new Column("MetaDescription", DbType.String, typeof(System.String), "MetaDescription", true, false));
			_Table.Columns.Add(new Column("MetaKeywords", DbType.String, typeof(System.String), "MetaKeywords", true, false));
			_Table.Columns.Add(new Column("IsPublished", DbType.Boolean, typeof(System.Boolean), "IsPublished", false, false));
			_Table.Columns.Add(new Column("SortOrder", DbType.Int32, typeof(System.Int32), "SortOrder", false, false));
			_Table.Columns.Add(new Column("ParentId", DbType.Int32, typeof(System.Int32), "ParentId", false, false));
			_Table.Columns.Add(new Column("IsHome", DbType.Boolean, typeof(System.Boolean), "IsHome", false, false));
			_Table.Columns.Add(new Column("HomeSortOrder", DbType.Int32, typeof(System.Int32), "HomeSortOrder", false, false));
		}

		/// <summary>
		/// Fetches an instance of Post based on a single column value. If more than one record is found, only the first will be used.
		/// </summary>
		public static DataPost FetchByColumn(Column column, object value)
		{
			Query q = new Query(_Table);
			q.AndWhere(column, value);
			return FetchByQuery(q);
		}

		public static DataPost FetchByQuery(Query q)
		{
			DataPost item = new DataPost ();
			using(IDataReader reader = q.ExecuteReader())
			{
				if(reader.Read())
					item.LoadAndCloseReader(reader);
			}

			return item;
		}

		/// <summary>
		/// Creates an instance of Query for the type Post
		/// </summary>
		public static Query CreateQuery()
		{
			return new Query(_Table);
		}

		public DataPost (){}
		/// <summary>
		/// Loads an instance of Post for the supplied primary key value
		/// </summary>
		public DataPost (object keyValue)
		{
			Query q = new Query(_Table);
			q.AndWhere(Columns.Id, keyValue);
			using(IDataReader reader = q.ExecuteReader())
			{
				if(reader.Read())
					LoadAndCloseReader(reader);
			}
		}

		/// <summary>
		/// Hydrates an instance of Post. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
		/// </summary>
		public void Load(IDataReader reader)
		{
			Load(reader, false);
		}

		/// <summary>
		/// Hydrates an instance of Post. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
		/// </summary>
		public void LoadAndCloseReader(IDataReader reader)
		{
			Load(reader, true);
		}

		private void Load(IDataReader reader, bool close)
		{
			Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
			Title = DataService.GetValue<System.String>(Columns.Title, reader);
			PostBody = DataService.GetValue<System.String>(Columns.PostBody, reader);
			CreatedOn = DataService.GetValue<System.DateTime>(Columns.CreatedOn, reader);
			ModifiedOn = DataService.GetValue<System.DateTime>(Columns.ModifiedOn, reader);
			Status = DataService.GetValue<System.Int32>(Columns.Status, reader);
			ContentType = DataService.GetValue<System.String>(Columns.ContentType, reader);
			Name = DataService.GetValue<System.String>(Columns.Name, reader);
			CommentCount = DataService.GetValue<System.Int32>(Columns.CommentCount, reader);
			TagList = DataService.GetValue<System.String>(Columns.TagList, reader);
			CategoryId = DataService.GetValue<System.Int32>(Columns.CategoryId, reader);
			Version = DataService.GetValue<System.Int32>(Columns.Version, reader);
			ModifiedBy = DataService.GetValue<System.String>(Columns.ModifiedBy, reader);
			CreatedBy = DataService.GetValue<System.String>(Columns.CreatedBy, reader);
			ExtendedBody = DataService.GetValue<System.String>(Columns.ExtendedBody, reader);
			IsDeleted = DataService.GetValue<System.Boolean>(Columns.IsDeleted, reader);
			Published = DataService.GetValue<System.DateTime>(Columns.Published, reader);
			PendingCommentCount = DataService.GetValue<System.Int32>(Columns.PendingCommentCount, reader);
			Views = DataService.GetValue<System.Int32>(Columns.Views, reader);
			UniqueId = DataService.GetValue<System.Guid>(Columns.UniqueId, reader);
			EnableComments = DataService.GetValue<System.Boolean>(Columns.EnableComments, reader);
			PropertyKeys = DataService.GetValue<System.String>(Columns.PropertyKeys, reader);
			PropertyValues = DataService.GetValue<System.String>(Columns.PropertyValues, reader);
			UserName = DataService.GetValue<System.String>(Columns.UserName, reader);
			Notes = DataService.GetValue<System.String>(Columns.Notes, reader);
			ImageUrl = DataService.GetValue<System.String>(Columns.ImageUrl, reader);
			MetaDescription = DataService.GetValue<System.String>(Columns.MetaDescription, reader);
			MetaKeywords = DataService.GetValue<System.String>(Columns.MetaKeywords, reader);
			IsPublished = DataService.GetValue<System.Boolean>(Columns.IsPublished, reader);
			SortOrder = DataService.GetValue<System.Int32>(Columns.SortOrder, reader);
			ParentId = DataService.GetValue<System.Int32>(Columns.ParentId, reader);
			IsHome = DataService.GetValue<System.Boolean>(Columns.IsHome, reader);
			HomeSortOrder = DataService.GetValue<System.Int32>(Columns.HomeSortOrder, reader);
			Loaded();
			ResetStatus();

			if(close)
				reader.Close();
		}

		#region public System.Int32 Id

		private System.Int32 _Id;

		public System.Int32 Id
		{
			get{return _Id;}
			set{MarkDirty();_Id = value;}
		}

		#endregion

		#region public System.String Title

		private System.String _Title;

		public System.String Title
		{
			get{return _Title;}
			set{MarkDirty();_Title = value;}
		}

		#endregion

		#region public System.String PostBody

		private System.String _PostBody;

		public System.String PostBody
		{
			get{return _PostBody;}
			set{MarkDirty();_PostBody = value;}
		}

		#endregion

		#region public System.DateTime CreatedOn

		private System.DateTime _CreatedOn;

		public System.DateTime CreatedOn
		{
			get{return _CreatedOn;}
			set{MarkDirty();_CreatedOn = value;}
		}

		#endregion

		#region public System.DateTime ModifiedOn

		private System.DateTime _ModifiedOn;

		public System.DateTime ModifiedOn
		{
			get{return _ModifiedOn;}
			set{MarkDirty();_ModifiedOn = value;}
		}

		#endregion

		#region public System.Int32 Status

		private System.Int32 _Status;

		public System.Int32 Status
		{
			get{return _Status;}
			set{MarkDirty();_Status = value;}
		}

		#endregion

		#region public System.String ContentType

		private System.String _ContentType;

		public System.String ContentType
		{
			get{return _ContentType;}
			set{MarkDirty();_ContentType = value;}
		}

		#endregion

		#region public System.String Name

		private System.String _Name;

		public System.String Name
		{
			get{return _Name;}
			set{MarkDirty();_Name = value;}
		}

		#endregion

		#region public System.Int32 CommentCount

		private System.Int32 _CommentCount;

		public System.Int32 CommentCount
		{
			get{return _CommentCount;}
			set{MarkDirty();_CommentCount = value;}
		}

		#endregion

		#region public System.String TagList

		private System.String _TagList;

		public System.String TagList
		{
			get{return _TagList;}
			set{MarkDirty();_TagList = value;}
		}

		#endregion

		#region public System.Int32 CategoryId

		private System.Int32 _CategoryId;

		public System.Int32 CategoryId
		{
			get{return _CategoryId;}
			set{MarkDirty();_CategoryId = value;}
		}

		#endregion

		#region public System.Int32 Version

		private System.Int32 _Version;

		public System.Int32 Version
		{
			get{return _Version;}
			set{MarkDirty();_Version = value;}
		}

		#endregion

		#region public System.String ModifiedBy

		private System.String _ModifiedBy;

		public System.String ModifiedBy
		{
			get{return _ModifiedBy;}
			set{MarkDirty();_ModifiedBy = value;}
		}

		#endregion

		#region public System.String CreatedBy

		private System.String _CreatedBy;

		public System.String CreatedBy
		{
			get{return _CreatedBy;}
			set{MarkDirty();_CreatedBy = value;}
		}

		#endregion

		#region public System.String ExtendedBody

		private System.String _ExtendedBody;

		public System.String ExtendedBody
		{
			get{return _ExtendedBody;}
			set{MarkDirty();_ExtendedBody = value;}
		}

		#endregion

		#region public System.Boolean IsDeleted

		private System.Boolean _IsDeleted;

		public System.Boolean IsDeleted
		{
			get{return _IsDeleted;}
			set{MarkDirty();_IsDeleted = value;}
		}

		#endregion

		#region public System.DateTime Published

		private System.DateTime _Published;

		public System.DateTime Published
		{
			get{return _Published;}
			set{MarkDirty();_Published = value;}
		}

		#endregion

		#region public System.Int32 PendingCommentCount

		private System.Int32 _PendingCommentCount;

		public System.Int32 PendingCommentCount
		{
			get{return _PendingCommentCount;}
			set{MarkDirty();_PendingCommentCount = value;}
		}

		#endregion

		#region public System.Int32 Views

		private System.Int32 _Views;

		public System.Int32 Views
		{
			get{return _Views;}
			set{MarkDirty();_Views = value;}
		}

		#endregion

		#region public System.Guid UniqueId

		private System.Guid _UniqueId;

		public System.Guid UniqueId
		{
			get{return _UniqueId;}
			set{MarkDirty();_UniqueId = value;}
		}

		#endregion

		#region public System.Boolean EnableComments

		private System.Boolean _EnableComments;

		public System.Boolean EnableComments
		{
			get{return _EnableComments;}
			set{MarkDirty();_EnableComments = value;}
		}

		#endregion

		#region public System.String PropertyKeys

		private System.String _PropertyKeys;

		public System.String PropertyKeys
		{
			get{return _PropertyKeys;}
			set{MarkDirty();_PropertyKeys = value;}
		}

		#endregion

		#region public System.String PropertyValues

		private System.String _PropertyValues;

		public System.String PropertyValues
		{
			get{return _PropertyValues;}
			set{MarkDirty();_PropertyValues = value;}
		}

		#endregion

		#region public System.String UserName

		private System.String _UserName;

		public System.String UserName
		{
			get{return _UserName;}
			set{MarkDirty();_UserName = value;}
		}

		#endregion

		#region public System.String Notes

		private System.String _Notes;

		public System.String Notes
		{
			get{return _Notes;}
			set{MarkDirty();_Notes = value;}
		}

		#endregion

		#region public System.String ImageUrl

		private System.String _ImageUrl;

		public System.String ImageUrl
		{
			get{return _ImageUrl;}
			set{MarkDirty();_ImageUrl = value;}
		}

		#endregion

		#region public System.String MetaDescription

		private System.String _MetaDescription;

		public System.String MetaDescription
		{
			get{return _MetaDescription;}
			set{MarkDirty();_MetaDescription = value;}
		}

		#endregion

		#region public System.String MetaKeywords

		private System.String _MetaKeywords;

		public System.String MetaKeywords
		{
			get{return _MetaKeywords;}
			set{MarkDirty();_MetaKeywords = value;}
		}

		#endregion

		#region public System.Boolean IsPublished

		private System.Boolean _IsPublished;

		public System.Boolean IsPublished
		{
			get{return _IsPublished;}
			set{MarkDirty();_IsPublished = value;}
		}

		#endregion

		#region public System.Int32 SortOrder

		private System.Int32 _SortOrder;

		public System.Int32 SortOrder
		{
			get{return _SortOrder;}
			set{MarkDirty();_SortOrder = value;}
		}

		#endregion

		#region public System.Int32 ParentId

		private System.Int32 _ParentId;

		public System.Int32 ParentId
		{
			get{return _ParentId;}
			set{MarkDirty();_ParentId = value;}
		}

		#endregion

		#region public System.Boolean IsHome

		private System.Boolean _IsHome;

		public System.Boolean IsHome
		{
			get{return _IsHome;}
			set{MarkDirty();_IsHome = value;}
		}

		#endregion

		#region public System.Int32 HomeSortOrder

		private System.Int32 _HomeSortOrder;

		public System.Int32 HomeSortOrder
		{
			get{return _HomeSortOrder;}
			set{MarkDirty();_HomeSortOrder = value;}
		}

		#endregion


        /// <summary>
        /// Returns absolute URL to the post.
        /// </summary>
        public string Url {
            get {
                return VirtualPathUtility.ToAbsolute(VirtualUrl);
            }
        }

        /// <summary>
        /// Returns the virtual URL to the post.
        /// </summary>
        public string VirtualUrl {
            get {
                if (DataCategoryController.UnCategorizedId != CategoryId)
                    return "~/" + new DataCategoryController().GetCachedCategory(CategoryId, false).LinkName + "/" + Name + "/";
                else
                    return "~/" + Name + "/";
            }
        }        


        /// <summary>
		/// The table object which represents Post
		/// </summary>
		public static Table Table { get{return _Table;}}

		/// <summary>
		/// The columns which represent Post
		/// </summary>
		public static class Columns
		{
			public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
			public static readonly Column Title = new Column("Title", DbType.String, typeof(System.String), "Title", false, false);
			public static readonly Column PostBody = new Column("PostBody", DbType.String, typeof(System.String), "PostBody", false, false);
			public static readonly Column CreatedOn = new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false);
			public static readonly Column ModifiedOn = new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false);
			public static readonly Column Status = new Column("Status", DbType.Int32, typeof(System.Int32), "Status", false, false);
			public static readonly Column ContentType = new Column("Content_Type", DbType.String, typeof(System.String), "ContentType", false, false);
			public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
			public static readonly Column CommentCount = new Column("Comment_Count", DbType.Int32, typeof(System.Int32), "CommentCount", false, false);
			public static readonly Column TagList = new Column("Tag_List", DbType.String, typeof(System.String), "TagList", true, false);
			public static readonly Column CategoryId = new Column("CategoryId", DbType.Int32, typeof(System.Int32), "CategoryId", false, false);
			public static readonly Column Version = new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false);
			public static readonly Column ModifiedBy = new Column("ModifiedBy", DbType.String, typeof(System.String), "ModifiedBy", true, false);
			public static readonly Column CreatedBy = new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", true, false);
			public static readonly Column ExtendedBody = new Column("ExtendedBody", DbType.String, typeof(System.String), "ExtendedBody", false, false);
			public static readonly Column IsDeleted = new Column("IsDeleted", DbType.Boolean, typeof(System.Boolean), "IsDeleted", false, false);
			public static readonly Column Published = new Column("Published", DbType.DateTime, typeof(System.DateTime), "Published", false, false);
			public static readonly Column PendingCommentCount = new Column("Pending_Comment_Count", DbType.Int32, typeof(System.Int32), "PendingCommentCount", false, false);
			public static readonly Column Views = new Column("Views", DbType.Int32, typeof(System.Int32), "Views", false, false);
			public static readonly Column UniqueId = new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false);
			public static readonly Column EnableComments = new Column("EnableComments", DbType.Boolean, typeof(System.Boolean), "EnableComments", false, false);
			public static readonly Column PropertyKeys = new Column("PropertyKeys", DbType.String, typeof(System.String), "PropertyKeys", true, false);
			public static readonly Column PropertyValues = new Column("PropertyValues", DbType.String, typeof(System.String), "PropertyValues", true, false);
			public static readonly Column UserName = new Column("UserName", DbType.String, typeof(System.String), "UserName", false, false);
			public static readonly Column Notes = new Column("Notes", DbType.String, typeof(System.String), "Notes", true, false);
			public static readonly Column ImageUrl = new Column("ImageUrl", DbType.String, typeof(System.String), "ImageUrl", true, false);
			public static readonly Column MetaDescription = new Column("MetaDescription", DbType.String, typeof(System.String), "MetaDescription", true, false);
			public static readonly Column MetaKeywords = new Column("MetaKeywords", DbType.String, typeof(System.String), "MetaKeywords", true, false);
			public static readonly Column IsPublished = new Column("IsPublished", DbType.Boolean, typeof(System.Boolean), "IsPublished", false, false);
			public static readonly Column SortOrder = new Column("SortOrder", DbType.Int32, typeof(System.Int32), "SortOrder", false, false);
			public static readonly Column ParentId = new Column("ParentId", DbType.Int32, typeof(System.Int32), "ParentId", false, false);
			public static readonly Column IsHome = new Column("IsHome", DbType.Boolean, typeof(System.Boolean), "IsHome", false, false);
			public static readonly Column HomeSortOrder = new Column("HomeSortOrder", DbType.Int32, typeof(System.Int32), "HomeSortOrder", false, false);
		}

		public static int Delete(Column column, object value)
		{
			DataPost objectToDelete = FetchByColumn(column, value);
			if(!objectToDelete.IsNew)
			{
				objectToDelete.BeforeRemove(false);
				int i = DataService.Delete(Table,column,value);
				objectToDelete.AfterRemove(false);
				return i;
			}

			return 0;
		}

		public static int Delete(object value)
		{
			return Delete(Columns.Id,value);
		}

		public static int Destroy(Column column, object value)
		{
			DataPost objectToDelete = FetchByColumn(column, value);
			if(!objectToDelete.IsNew)
			{
				objectToDelete.BeforeRemove(true);
				int i = DataService.Destroy(Table,column,value);
				objectToDelete.AfterRemove(true);
				return i;
			}

			return 0;
		}


		public static int Destroy(object value)
		{
			return Destroy(Columns.Id, value);
		}
		protected override void SetPrimaryKey(int pkID)
		{
			Id = pkID;
		}

		protected override Table GetTable()
		{
			return Table;
		}

		public static Parameter FindParameter(List<Parameter> parameters, string name)
		{
			if( parameters == null )
				throw new ArgumentNullException("parameters");
			if( string.IsNullOrEmpty( name ) )
				throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

			return parameters.Find( delegate(Parameter p) { return ( p.Name == name ); } );
		}

		public static Parameter FindParameter(string name)
		{
			if( string.IsNullOrEmpty( name ) )
				throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

			return GenerateParameters().Find( delegate(Parameter p) { return ( p.Name == name ); } );
		}

		#region public static List<Parameter> GenerateParameters()

		public static List<Parameter> GenerateParameters()
		{
			List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

			parameters.Add( new Parameter( "Id", null, DbType.Int32 ) );

			Parameter pTitle = new Parameter( "Title", null, DbType.String );
			pTitle.Length = 255;
			parameters.Add( pTitle );

			Parameter pPostBody = new Parameter( "PostBody", null, DbType.String );
			pPostBody.Length = 2147483647;
			parameters.Add( pPostBody );

			parameters.Add( new Parameter( "CreatedOn", null, DbType.DateTime ) );

			parameters.Add( new Parameter( "ModifiedOn", null, DbType.DateTime ) );

			parameters.Add( new Parameter( "Status", null, DbType.Int32 ) );

			Parameter pContent_Type = new Parameter( "Content_Type", null, DbType.String );
			pContent_Type.Length = 50;
			parameters.Add( pContent_Type );

			Parameter pName = new Parameter( "Name", null, DbType.String );
			pName.Length = 255;
			parameters.Add( pName );

			parameters.Add( new Parameter( "Comment_Count", null, DbType.Int32 ) );

			Parameter pTag_List = new Parameter( "Tag_List", null, DbType.String );
			pTag_List.Length = 512;
			parameters.Add( pTag_List );

			parameters.Add( new Parameter( "CategoryId", null, DbType.Int32 ) );

			parameters.Add( new Parameter( "Version", null, DbType.Int32 ) );

			Parameter pModifiedBy = new Parameter( "ModifiedBy", null, DbType.String );
			pModifiedBy.Length = 128;
			parameters.Add( pModifiedBy );

			Parameter pCreatedBy = new Parameter( "CreatedBy", null, DbType.String );
			pCreatedBy.Length = 128;
			parameters.Add( pCreatedBy );

			Parameter pExtendedBody = new Parameter( "ExtendedBody", null, DbType.String );
			pExtendedBody.Length = 2147483647;
			parameters.Add( pExtendedBody );

			parameters.Add( new Parameter( "IsDeleted", null, DbType.Boolean ) );

			parameters.Add( new Parameter( "Published", null, DbType.DateTime ) );

			parameters.Add( new Parameter( "Pending_Comment_Count", null, DbType.Int32 ) );

			parameters.Add( new Parameter( "Views", null, DbType.Int32 ) );

			parameters.Add( new Parameter( "UniqueId", null, DbType.Guid ) );

			parameters.Add( new Parameter( "EnableComments", null, DbType.Boolean ) );

			Parameter pPropertyKeys = new Parameter( "PropertyKeys", null, DbType.String );
			pPropertyKeys.Length = 2147483647;
			parameters.Add( pPropertyKeys );

			Parameter pPropertyValues = new Parameter( "PropertyValues", null, DbType.String );
			pPropertyValues.Length = 2147483647;
			parameters.Add( pPropertyValues );

			Parameter pUserName = new Parameter( "UserName", null, DbType.String );
			pUserName.Length = 128;
			parameters.Add( pUserName );

			Parameter pNotes = new Parameter( "Notes", null, DbType.String );
			pNotes.Length = 2000;
			parameters.Add( pNotes );

			Parameter pImageUrl = new Parameter( "ImageUrl", null, DbType.String );
			pImageUrl.Length = 255;
			parameters.Add( pImageUrl );

			Parameter pMetaDescription = new Parameter( "MetaDescription", null, DbType.String );
			pMetaDescription.Length = 255;
			parameters.Add( pMetaDescription );

			Parameter pMetaKeywords = new Parameter( "MetaKeywords", null, DbType.String );
			pMetaKeywords.Length = 255;
			parameters.Add( pMetaKeywords );

			parameters.Add( new Parameter( "IsPublished", null, DbType.Boolean ) );

			parameters.Add( new Parameter( "SortOrder", null, DbType.Int32 ) );

			parameters.Add( new Parameter( "ParentId", null, DbType.Int32 ) );

			parameters.Add( new Parameter( "IsHome", null, DbType.Boolean ) );

			parameters.Add( new Parameter( "HomeSortOrder", null, DbType.Int32 ) );

			return parameters;
		}

		#endregion

		protected override List<Parameter> GetParameters()
		{
			List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

			if(!IsNew)
			{
				parameters.Add( new Parameter("Id", Id, DbType.Int32) );
			}

			Parameter pTitle = new Parameter("Title", Title, DbType.String);
			pTitle.Length = 255;
			parameters.Add( pTitle );

			Parameter pPostBody = new Parameter("PostBody", PostBody, DbType.String);
			pPostBody.Length = 2147483647;
			parameters.Add( pPostBody );

			parameters.Add( new Parameter("CreatedOn", CreatedOn, DbType.DateTime) );

			parameters.Add( new Parameter("ModifiedOn", ModifiedOn, DbType.DateTime) );

			parameters.Add( new Parameter("Status", Status, DbType.Int32) );

			Parameter pContent_Type = new Parameter("Content_Type", ContentType, DbType.String);
			pContent_Type.Length = 50;
			parameters.Add( pContent_Type );

			Parameter pName = new Parameter("Name", Name, DbType.String);
			pName.Length = 255;
			parameters.Add( pName );

			parameters.Add( new Parameter("Comment_Count", CommentCount, DbType.Int32) );

			Parameter pTag_List = new Parameter("Tag_List", TagList, DbType.String);
			pTag_List.Length = 512;
			parameters.Add( pTag_List );

			parameters.Add( new Parameter("CategoryId", CategoryId, DbType.Int32) );

			parameters.Add( new Parameter("Version", Version, DbType.Int32) );

			Parameter pModifiedBy = new Parameter("ModifiedBy", ModifiedBy, DbType.String);
			pModifiedBy.Length = 128;
			parameters.Add( pModifiedBy );

			Parameter pCreatedBy = new Parameter("CreatedBy", CreatedBy, DbType.String);
			pCreatedBy.Length = 128;
			parameters.Add( pCreatedBy );

			Parameter pExtendedBody = new Parameter("ExtendedBody", ExtendedBody, DbType.String);
			pExtendedBody.Length = 2147483647;
			parameters.Add( pExtendedBody );

			parameters.Add( new Parameter("IsDeleted", IsDeleted, DbType.Boolean) );

			parameters.Add( new Parameter("Published", Published, DbType.DateTime) );

			parameters.Add( new Parameter("Pending_Comment_Count", PendingCommentCount, DbType.Int32) );

			parameters.Add( new Parameter("Views", Views, DbType.Int32) );

			parameters.Add( new Parameter("UniqueId", UniqueId, DbType.Guid) );

			parameters.Add( new Parameter("EnableComments", EnableComments, DbType.Boolean) );

			Parameter pPropertyKeys = new Parameter("PropertyKeys", PropertyKeys, DbType.String);
			pPropertyKeys.Length = 2147483647;
			parameters.Add( pPropertyKeys );

			Parameter pPropertyValues = new Parameter("PropertyValues", PropertyValues, DbType.String);
			pPropertyValues.Length = 2147483647;
			parameters.Add( pPropertyValues );

			Parameter pUserName = new Parameter("UserName", UserName, DbType.String);
			pUserName.Length = 128;
			parameters.Add( pUserName );

			Parameter pNotes = new Parameter("Notes", Notes, DbType.String);
			pNotes.Length = 2000;
			parameters.Add( pNotes );

			Parameter pImageUrl = new Parameter("ImageUrl", ImageUrl, DbType.String);
			pImageUrl.Length = 255;
			parameters.Add( pImageUrl );

			Parameter pMetaDescription = new Parameter("MetaDescription", MetaDescription, DbType.String);
			pMetaDescription.Length = 255;
			parameters.Add( pMetaDescription );

			Parameter pMetaKeywords = new Parameter("MetaKeywords", MetaKeywords, DbType.String);
			pMetaKeywords.Length = 255;
			parameters.Add( pMetaKeywords );

			parameters.Add( new Parameter("IsPublished", IsPublished, DbType.Boolean) );

			parameters.Add( new Parameter("SortOrder", SortOrder, DbType.Int32) );

			parameters.Add( new Parameter("ParentId", ParentId, DbType.Int32) );

			parameters.Add( new Parameter("IsHome", IsHome, DbType.Boolean) );

			parameters.Add( new Parameter("HomeSortOrder", HomeSortOrder, DbType.Int32) );

			return parameters;
		}

        #region Custom Field Support

        //container for custom field values
        private NameValueCollection _nvc = new NameValueCollection();
        private bool _isCustomReady = false;

        /// <summary>
        /// Returns a custom fiels keys and values
        /// </summary>
        /// <returns></returns>
        public NameValueCollection CustomFields() {
            if (!_isCustomReady)
                DeserializeCustomFields();
            return _nvc;
        }

        /// <summary>
        /// Called Pre-Update to set the PropertyKeys and PropertyValues properties
        /// </summary>
        internal void SerializeCustomFields() {
            StringBuilder sbKey = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();

            int index = 0;
            foreach (string key in _nvc.AllKeys) {
                if (key.IndexOf(':') != -1)
                    throw new ArgumentException("Extened Fields Key can not contain the character \":\"");

                string v = _nvc[key];
                if (!string.IsNullOrEmpty(v)) {
                    sbKey.AppendFormat("{0}:S:{1}:{2}:", key, index, v.Length);
                    sbValue.Append(v);
                    index += v.Length;
                }
            }


            PropertyKeys = sbKey.ToString();
            PropertyValues = sbValue.ToString();
        }

        /// <summary>
        /// Called during Loaded() method to copy values from PropertyKeys and PropertyValues 
        /// to the CustomFields NameValueCollection
        /// </summary>
        internal void DeserializeCustomFields() {
            _nvc.Clear();

            if (PropertyKeys != null && PropertyValues != null) {
                char[] splitter = new char[1] { ':' };
                string[] keyNames = PropertyKeys.Split(splitter);

                for (int i = 0; i < (keyNames.Length / 4); i++) {
                    int start = int.Parse(keyNames[(i * 4) + 2], CultureInfo.InvariantCulture);
                    int len = int.Parse(keyNames[(i * 4) + 3], CultureInfo.InvariantCulture);
                    string key = keyNames[i * 4];

                    if (((keyNames[(i * 4) + 1] == "S") && (start >= 0)) && (len > 0) && (PropertyValues.Length >= (start + len))) {
                        _nvc[key] = PropertyValues.Substring(start, len);
                    }
                }
            }

            _isCustomReady = true;
        }

        /// <summary>
        /// Provides access to the custom fields collection
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get { return _nvc[key]; }
            set { _nvc[key] = value; }
        }

        /// <summary>
        /// Returns the custom field value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <example>
        /// #foreach($post in $posts)
        /// [TAB]The value of my custom field is $post.Custom("MyCustomFieldName")
        /// #end
        /// </example>
        public string Custom(string key) {
            return this[key];
        }

        #endregion

        #region DataBuddy, Validation, and Pages

        /// <summary>
        /// Flag used to make sure posts returned via search results are not committed to the database
        /// </summary>
        public bool IsCreatedBySearch = false;

        /// <summary>
        /// The current category of the post. 
        /// </summary>
        public DataCategory Category {
            get { return categories.GetCachedCategory(CategoryId, false); }
        }

        protected override void Loaded()
        {
            base.Loaded();
            DeserializeCustomFields();
        }

        protected override void BeforeValidate()
        {
            base.BeforeValidate();

            SerializeCustomFields();

            //Make sure no updates are made to a post created via search. We do not store the full
            //post content in the search index, so it is not safe to save.
            if (IsCreatedBySearch)
                throw new Exception("Cannot save a post from the search index");

            ////We always need a post name
            Name = DataUtil.CleanForUrl(!string.IsNullOrEmpty(Name) ? Name : Title);

            if (IsNew) {
                UniqueId = Guid.NewGuid();
                if (UserName == null)
                    UserName = CreatedBy;
            }

            if (string.IsNullOrEmpty(UserName))
                throw new Exception("Cannot save a post withou a username");

            ////Check for reserved words
            if (!DataUtil.IsValidFileName(Name))
            {
                throw new Exception("Sorry, you cannot use the reserved word *" + Name + "* for a file name.");
            }

            if (CategoryId == categories.GetUnCategorizedCategory().Id)
            {
                //Check to make sure there is no collision between the post name and a category
                //since uncategrozied posts are accessible via .com/post-name

                Regex regex = new Regex("^(" + Name + ")$", RegexOptions.IgnoreCase);
                foreach (DataCategory category in categories.GetCachedCategories())
                {
                    if (regex.IsMatch(category.LinkName))
                    {
                        throw new Exception("Uncategorized posts cannot use the same name as a category. *" + Name +
                                            "* already exists as a category");
                    }
                }
            }

            if(!string.IsNullOrEmpty(TagList))
            {
                List<string> the_Tags = DataUtil.ConvertStringToList(TagList);
                if(the_Tags.Count > 0)
                {
                    for(int i = 0;i<the_Tags.Count; i++)
                    {
                        the_Tags[i] = DataUtil.CleanForUrl(the_Tags[i]);
                    }

                    TagList = string.Join(",", the_Tags.ToArray());
                }
            }
            
        }

        protected override void AfterCommit()
        {
            base.AfterCommit();

            //Update the number of posts per category
            DataCategoryController.UpdatePostCounts();
            //PostController.UpdateVersionCount(Id);

            //Save tags per post
            DataTag.Destroy(DataTag.Columns.PostId, Id);
            if (TagList != null) {
                foreach (string t in DataUtil.ConvertStringToList(TagList)) {
                    DataTag tag = new DataTag();
                    tag.Name = t.Trim();
                    tag.PostId = Id;
                    tag.Save();
                }
            }

            WritePages();

            ZCache.RemoveByPattern("Posts-");
            ZCache.RemoveCache("Post-" + Id);
        }

        /// <summary>
        /// Writes out the empty pages for the post. Also handles the redirect pages
        /// </summary>
        public void WritePages() 
        {
            PageTemplateToolboxContext templateContext = new PageTemplateToolboxContext();
            templateContext.Put("PostId", Id);
            templateContext.Put("CategoryId", CategoryId);
            templateContext.Put("PostName", Name);
            templateContext.Put("Name", Name);
            templateContext.Put("CategoryName", Category.LinkName);
            templateContext.Put("MetaDescription",
                                !string.IsNullOrEmpty(MetaDescription)
                                    ? MetaDescription
                                    : HttpUtility.HtmlEncode(Util.RemoveHtml(PostBody, 255) ?? string.Empty));

            templateContext.Put("MetaKeywords",
                                !string.IsNullOrEmpty(MetaKeywords)
                                    ? MetaKeywords
                                    : TagList);


            string pageName = null;

            if (DataCategoryController.UnCategorizedId != CategoryId)
                pageName = categories.GetCachedCategory(CategoryId, false).LinkName + "/";

            pageName = "~/" + pageName + Name + "/" + Util.DEFAULT_PAGE;

            PageWriter.Write("post.view", pageName, templateContext);

        }

        #endregion

        static readonly DataCategoryController categories = new DataCategoryController();

        /// <summary>
        /// User of the current post. This will cause an exception if the UserName value is null
        /// </summary>
        public IGraffitiUser User {
            get {
                return GraffitiUsers.GetUser(UserName);
            }
        }

//        #region Post Helpers

        public static void DestroyDeletedPosts() 
        {
            DateTime dt = DateTime.Now.AddDays(-1 * 30); //SiteSettings.DestroyDeletedPostsOlderThanDays);
            Query q = CreateQuery();
            q.AndWhere(Columns.IsDeleted, true);
            q.AndWhere(Columns.ModifiedOn, dt, Comparison.LessOrEquals);

            DataPostCollection pc = DataPostCollection.FetchByQuery(q);
            if (pc.Count > 0) {
                DataLog.Info("Deleting Posts", "Deleting {0} post(s) since they were deleted before {1}", pc.Count, dt);

                foreach (DataPost p in pc) {
                    try {
                        DestroyDeletedPost(p.Id);
                        DataLog.Info("Post Deleted", "The post \"{0}\" ({3}) and related content was deleted. It had been marked to be deleted on {1} by {2}", p.Title, p.ModifiedOn, p.ModifiedBy, p.Id);
                    } catch (Exception ex) {
                        DataLog.Error("Post Delete", "The post \"{0}\" was not successfully deleted. Reason: {1}", p.Title, ex.Message);
                    }
                }
            }
        }

        public static void DestroyDeletedPost(int postid) 
        {
            DataPost p = new DataPost(postid);
            DataPostStatistic.Destroy(DataPostStatistic.Columns.PostId, postid);
            DataTag.Destroy(DataTag.Columns.PostId, postid);
            DataComment.Destroy(DataComment.Columns.PostId, postid);
            DataService.ExecuteNonQuery(new QueryCommand("delete from graffiti_VersionStore where Type = 'post/xml' and ItemId = " + postid));

            DataPost.Destroy(postid);
        }

        public static DataPost GetPost(object id) 
        {
            return new DataPost(id);
        }

        public static int GetPostIdByName(string name) 
        {
            object id = ZCache.Get<object>("PostIdByName-" + name);
            if (id == null) {
                string postName;
                string categoryName = null;

                if (name.Contains("/")) {
                    string[] parts = name.Split('/');

                    for (int i = 0; i < parts.Length; i++)
                        parts[i] = DataUtil.CleanForUrl(parts[i]);

                    switch (parts.Length) {
                        case 2:
                            categoryName = parts[0];
                            postName = parts[1];
                            break;
                        case 3:
                            categoryName = parts[0] + "/" + parts[1];
                            postName = parts[2];
                            break;
                        default:
                            return -1;
                    }
                } else
                    postName = DataUtil.CleanForUrl(name);

                int categoryId = -1;
                if (categoryName != null) {
                    DataCategoryCollection the_categories = new DataCategoryController().GetCachedCategories();

                    foreach (DataCategory category in the_categories)
                        if (category.LinkName == categoryName)
                            categoryId = category.Id;

                    if (categoryId == -1)
                        return -1;
                }

                List<Parameter> parameters = DataPost.GenerateParameters();

                /* this is supposed to be TOP 1, but the ExecuteScalar will pull only the first one */
                QueryCommand cmd = new QueryCommand("Select Id FROM graffiti_Posts Where Name = " + DataService.Provider.SqlVariable("Name") + " and IsDeleted = 0");
                cmd.Parameters.Add(DataPost.FindParameter(parameters, "Name")).Value = postName;

                if (categoryId > -1) {
                    cmd.Sql += " and CategoryId = " + DataService.Provider.SqlVariable("CategoryId");
                    cmd.Parameters.Add(DataPost.FindParameter(parameters, "CategoryId")).Value = categoryId;
                }

                cmd.Sql += " order by CategoryId asc";

                object postobj = DataService.ExecuteScalar(cmd);
                if (postobj != null) {
                    id = postobj;
                    ZCache.InsertCache("PostIdByName-" + name, (int)id, 60);
                } else
                    id = -1;
            }
            return (int)id;
        }

        public static List<Core.PostCount> GetPostCounts(Core.IGraffitiUser user, int catID, string username, Delegate callback) {
            List<Core.PostCount> postCounts = new List<Core.PostCount>();
            List<Core.PostCount> final = new List<Core.PostCount>();

            List<Parameter> parameters = DataPost.GenerateParameters();
            QueryCommand cmd = new QueryCommand("Select Status, CategoryId, " + DataService.Provider.SqlCountFunction("Id") + " as StatusCount FROM graffiti_Posts Where IsDeleted = 0");

            if (catID > 0) {
                cmd.Sql += " and CategoryId = " + DataService.Provider.SqlVariable("CategoryId");
                cmd.Parameters.Add(DataPost.FindParameter(parameters, "CategoryId")).Value = catID;
            }

            if (!String.IsNullOrEmpty(username)) {
                cmd.Sql += " and CreatedBy = " + DataService.Provider.SqlVariable("CreatedBy");
                cmd.Parameters.Add(DataPost.FindParameter(parameters, "CreatedBy")).Value = username;
            }

            cmd.Sql += " group by Status, CategoryId";

            using (IDataReader reader = DataService.ExecuteReader(cmd)) {
                while (reader.Read()) {
                    Core.PostCount postCount = new Core.PostCount();
                    postCount.PostStatus = (Core.PostStatus)Int32.Parse(reader["Status"].ToString());
                    postCount.Count = Int32.Parse(reader["StatusCount"].ToString());
                    postCount.CategoryId = Int32.Parse(reader["CategoryId"].ToString());

                    postCounts.Add(postCount);
                }

                reader.Close();
            }

            List<Core.PostCount> filteredPermissions = new List<Core.PostCount>();
            filteredPermissions.AddRange(postCounts);

            foreach (Core.PostCount ac in postCounts) {
                if (!((Core.Permission)callback.DynamicInvoke(ac.CategoryId, user)).Read)
                    filteredPermissions.Remove(ac);
            }

            foreach (Core.PostCount ac in filteredPermissions) {
                Core.PostCount existing = final.Find(
                                                delegate(Core.PostCount postcount) {
                                                    return postcount.PostStatus == ac.PostStatus;
                                                });

                if (existing == null) {
                    final.Add(ac);
                } else {
                    existing.Count += ac.Count;
                }
            }

            return final;
        }

        public static List<Core.CategoryCount> GetCategoryCountForStatus(Core.IGraffitiUser user, Core.PostStatus status, string authorID, Delegate callback) 
        {
            List<Core.CategoryCount> catCounts = new List<Core.CategoryCount>();
            List<Core.CategoryCount> final = new List<Core.CategoryCount>();

            DataProvider dp = DataService.Provider;
            QueryCommand cmd = new QueryCommand(String.Empty);

            if (String.IsNullOrEmpty(authorID)) {
                cmd.Sql = @"select c.Id, " + dp.SqlCountFunction("c.Name") + @" as IdCount, p.CategoryId from graffiti_Posts AS p
                inner join graffiti_Categories AS c on p.CategoryId = c.Id
                where p.Status = " + dp.SqlVariable("Status") + @" and p.IsDeleted = 0
                group by c.Id, p.CategoryId";
            } else {
                cmd.Sql = @"select c.Id, " + dp.SqlCountFunction("c.Name") + @" as IdCount, p.CategoryId from ((graffiti_Posts AS p
                inner join graffiti_Categories AS c on p.CategoryId = c.Id)
                inner join graffiti_Users AS u on p.CreatedBy = u.Name)
                where p.Status = " + dp.SqlVariable("Status") + @" and p.IsDeleted = 0 and u.Id = " + dp.SqlVariable("AuthorId") +
                @" group by c.Id, p.CategoryId";
            }

            cmd.Parameters.Add(DataPost.FindParameter("Status")).Value = (int)status;

            if (!String.IsNullOrEmpty(authorID)) {
                cmd.Parameters.Add("AuthorId", Convert.ToInt32(authorID), DataUser.FindParameter("Id").DbType);
            }

            using (IDataReader reader = DataService.ExecuteReader(cmd)) {
                while (reader.Read()) {
                    Core.CategoryCount catCount = new Core.CategoryCount();
                    catCount.ID = Int32.Parse(reader["Id"].ToString());
                    catCount.Count = Int32.Parse(reader["IdCount"].ToString());
                    catCount.CategoryId = Int32.Parse(reader["CategoryId"].ToString());

                    catCounts.Add(catCount);
                }

                reader.Close();
            }

            // populate the category name
            DataCategoryCollection cats = new DataCategoryController().GetAllCachedCategories();

            List<Core.CategoryCount> tempParentList = new List<Core.CategoryCount>();

            foreach (Core.CategoryCount cc in catCounts) {
                DataCategory temp = cats.Find(
                                 delegate(DataCategory c) {
                                     return c.Id == cc.ID;
                                 });

                if (temp != null) {
                    cc.Name = temp.Name;
                    cc.ParentId = temp.ParentId;
                }

                if (cc.Count > 0 && cc.ParentId >= 1) {
                    // if it's not already in the list, add it
                    Core.CategoryCount parent = catCounts.Find(
                                                delegate(Core.CategoryCount cac) {
                                                    return cac.ID == cc.ParentId;
                                                });

                    if (parent == null) {
                        parent = tempParentList.Find(
                                                    delegate(Core.CategoryCount cac) {
                                                        return cac.ID == cc.ParentId;
                                                    });

                        if (parent == null) {
                            DataCategory tempParent = cats.Find(
                                                    delegate(DataCategory cttemp) {
                                                        return cttemp.Id == cc.ParentId;
                                                    });

                            parent = new Core.CategoryCount();
                            parent.ID = tempParent.Id;
                            parent.ParentId = tempParent.ParentId;
                            parent.Name = tempParent.Name;
                            parent.Count = 0;

                            tempParentList.Add(parent);
                        }
                    }
                }
            }

            catCounts.AddRange(tempParentList);

            List<Core.CategoryCount> filteredPermissions = new List<Core.CategoryCount>();
            filteredPermissions.AddRange(catCounts);

            foreach (Core.CategoryCount ac in catCounts) {
                if (!((Core.Permission)callback.DynamicInvoke(ac.CategoryId, user)).Read)
                    filteredPermissions.Remove(ac);
            }

            foreach (Core.CategoryCount ac in filteredPermissions) {
                Core.CategoryCount existing = final.Find(
                                                delegate(Core.CategoryCount catcount) {
                                                    return catcount.ID == ac.ID;
                                                });

                if (existing == null) {
                    final.Add(ac);
                } else {
                    existing.Count += ac.Count;
                }
            }

            return final;
        }

        public static List<Core.AuthorCount> GetAuthorCountForStatus(Core.IGraffitiUser user, Core.PostStatus status, string categoryID, Delegate callback) 
        {
            List<Core.AuthorCount> autCounts = new List<Core.AuthorCount>();
            List<Core.AuthorCount> final = new List<Core.AuthorCount>();

            QueryCommand cmd = new QueryCommand(
                    @"select u.Id, " + DataService.Provider.SqlCountFunction("u.Id") + @" as IdCount, u.ProperName, p.CategoryId from graffiti_Posts AS p
                    inner join graffiti_Users as u on p.CreatedBy = u.Name
                    where p.Status = " + DataService.Provider.SqlVariable("Status") + @" and p.IsDeleted = 0");

            if (!String.IsNullOrEmpty(categoryID)) {
                cmd.Sql += " and p.CategoryId = " + DataService.Provider.SqlVariable("CategoryId");
            }

            cmd.Sql += " group by u.Id, u.ProperName, p.CategoryId";

            List<Parameter> parameters = DataPost.GenerateParameters();
            cmd.Parameters.Add(DataPost.FindParameter(parameters, "Status")).Value = (int)status;

            if (!String.IsNullOrEmpty(categoryID)) {
                cmd.Parameters.Add(DataPost.FindParameter(parameters, "CategoryId")).Value = Convert.ToInt32(categoryID);
            }

            using (IDataReader reader = DataService.ExecuteReader(cmd)) {
                while (reader.Read()) {
                    Core.AuthorCount autCount = new Core.AuthorCount();
                    autCount.ID = Int32.Parse(reader["Id"].ToString());
                    autCount.Count = Int32.Parse(reader["IdCount"].ToString());
                    autCount.Name = reader["ProperName"].ToString();
                    autCount.CategoryId = Int32.Parse(reader["CategoryId"].ToString());

                    autCounts.Add(autCount);
                }

                List<Core.AuthorCount> filteredPermissions = new List<Core.AuthorCount>();
                filteredPermissions.AddRange(autCounts);

                foreach (Core.AuthorCount ac in autCounts) {
                    if (!((Core.Permission)callback.DynamicInvoke(ac.CategoryId, user)).Read)
                        filteredPermissions.Remove(ac);
                }

                foreach (Core.AuthorCount ac in filteredPermissions) {
                    Core.AuthorCount existing = final.Find(
                                                    delegate(Core.AuthorCount authcount) {
                                                        return authcount.Name == ac.Name;
                                                    });

                    if (existing == null) {
                        final.Add(ac);
                    } else {
                        existing.Count += ac.Count;
                    }
                }

                reader.Close();
            }

            return final;
        }

//        public static Post FromXML(string xml)
//        {
//            Post the_Post = ObjectManager.ConvertToObject<Post>(xml);
//            the_Post.Loaded();
//            the_Post.ResetStatus();

//            return the_Post;
//        }

//        public string ToXML()
//        {
//            SerializeCustomFields();
//            return ObjectManager.ConvertToString(this);
//        }

        public static void UpdatePostStatus(int id, int status) 
        {
            //UpdateVersionCount(id);

            QueryCommand command = new QueryCommand("Update graffiti_Posts Set Status = " + DataService.Provider.SqlVariable("Status") + " Where Id = " + DataService.Provider.SqlVariable("Id"));
            List<Parameter> parameters = DataPost.GenerateParameters();
            command.Parameters.Add(DataPost.FindParameter(parameters, "Status")).Value = (int)status;
            command.Parameters.Add(DataPost.FindParameter(parameters, "Id")).Value = id;

            DataService.ExecuteNonQuery(command);

            ZCache.RemoveByPattern("Posts-");
            ZCache.RemoveCache("Post-" + id);
        }

        /// <summary>
        /// Returns a collection of posts for a given tag. This is a special call
        /// since tagName is not available on the post query
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static DataPostCollection FetchPostsByTag(string tagName) 
        {
            QueryCommand command = new QueryCommand("SELECT p.* FROM graffiti_Posts AS p INNER JOIN graffiti_Tags AS t ON p.Id = t.PostId WHERE p.IsPublished <> 0 and p.IsDeleted = 0 and p.Published <= " + DataService.Provider.SqlVariable("Published") + " and t.Name = " + DataService.Provider.SqlVariable("Name") + " ORDER BY p.Published DESC");
            command.Parameters.Add(DataPost.FindParameter("Published")).Value = SiteSettings.CurrentUserTime;
            command.Parameters.Add(DataTag.FindParameter("Name")).Value = tagName;
            DataPostCollection pc = new DataPostCollection();
            pc.LoadAndCloseReader(DataService.ExecuteReader(command));
            return pc;
        }

        public static DataPostCollection FetchPostsByTagAndCategory(string tagName, int categoryId)
        {
            QueryCommand command = new QueryCommand("SELECT p.* FROM graffiti_Posts AS p INNER JOIN graffiti_Tags AS t ON p.Id = t.PostId WHERE p.CategoryId = " + categoryId.ToString() + " and p.IsPublished <> 0 and p.IsDeleted = 0 and p.Published <= " + DataService.Provider.SqlVariable("Published") + " and t.Name = " + DataService.Provider.SqlVariable("Name") + " ORDER BY p.Published DESC");
            command.Parameters.Add(DataPost.FindParameter("Published")).Value = SiteSettings.CurrentUserTime;
            command.Parameters.Add(DataTag.FindParameter("Name")).Value = tagName;
            DataPostCollection pc = new DataPostCollection();
            pc.LoadAndCloseReader(DataService.ExecuteReader(command));
            return pc;
        }

        public static void UpdateViewCount(int postid)
        {
            QueryCommand command = new QueryCommand("UPDATE graffiti_Posts Set Views = Views + 1 WHERE Id = " + DataService.Provider.SqlVariable("Id"));
            command.Parameters.Add(DataPost.FindParameter("Id")).Value = postid;
            DataService.ExecuteNonQuery(command);

            DataPostStatistic ps = new DataPostStatistic();
            ps.PostId = postid;
            ps.DateViewed = DateTime.Now;

            ps.Save();
        }

        public static void UpdateCommentCount(int postid) {
            QueryCommand command = null;
            DataProvider dp = DataService.Provider;

            if (DataUtil.IsAccess) {
                Query q1 = DataComment.CreateQuery();
                q1.AndWhere(DataComment.Columns.PostId, postid);
                q1.AndWhere(DataComment.Columns.IsPublished, true);
                q1.AndWhere(DataComment.Columns.IsDeleted, false);

                int Comment_Count = q1.GetRecordCount();

                Query q2 = DataComment.CreateQuery();
                q2.AndWhere(DataComment.Columns.PostId, postid);
                q2.AndWhere(DataComment.Columns.IsPublished, false);
                q2.AndWhere(DataComment.Columns.IsDeleted, false);

                int Pending_Comment_Count = q2.GetRecordCount();

                command = new QueryCommand("UPDATE graffiti_Posts Set Comment_Count = "
                    + dp.SqlVariable("Comment_Count")
                    + ", Pending_Comment_Count = " + dp.SqlVariable("Pending_Comment_Count")
                    + " WHERE Id = " + dp.SqlVariable("Id"));
                List<Parameter> parameters = DataPost.GenerateParameters();
                command.Parameters.Add(DataPost.FindParameter(parameters, "Comment_Count")).Value = Comment_Count;
                command.Parameters.Add(DataPost.FindParameter(parameters, "Pending_Comment_Count")).Value = Pending_Comment_Count;
                command.Parameters.Add(DataPost.FindParameter(parameters, "Id")).Value = postid;
            } else {
                string sql =
                    @"Update graffiti_Posts
                    Set
	                    Comment_Count = (Select " + dp.SqlCountFunction() + @" FROM graffiti_Comments AS c where c.PostId = " + dp.SqlVariable("Id") + @" and c.IsPublished = 1 and c.IsDeleted = 0),
	                    Pending_Comment_Count = (Select " + dp.SqlCountFunction() + @" FROM graffiti_Comments AS c where c.PostId = " + dp.SqlVariable("Id") + @" and c.IsPublished = 0 and c.IsDeleted = 0)
                   Where Id = " + dp.SqlVariable("Id");

                command = new QueryCommand(sql);
                command.Parameters.Add(DataPost.FindParameter("Id")).Value = postid;
            }

            DataService.ExecuteNonQuery(command);
        }

//        #endregion
    }    
}