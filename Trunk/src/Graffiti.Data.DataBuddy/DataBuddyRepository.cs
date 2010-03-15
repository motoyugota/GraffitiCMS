using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy 
{
    public class DataBuddyRepository : 
           IPostRepository, 
            ICategoryRepository, 
            ICommentRepository,
            ITagRepository, 
            IObjectStoreRepository, 
            IRoleRepository, 
            IPermissionRepository, 
            IUserRepository, 
            ILogRepository,
            IVersionStoreRepository, 
            IReportRepository
    {
        #region Post

        public Post FetchPost(int id) 
        {
            return ConvertDataPostToPost(DataPost.GetPost(id));
        }

        public Post FetchPost(string id)
        {
            return ConvertDataPostToPost(DataPost.GetPost(id));
        }

        public Post FetchPostByUniqueId(Guid uniqueId)
        {
            return ConvertDataPostToPost(DataPost.FetchByColumn(DataPost.Columns.UniqueId, uniqueId));
        }

        public IQueryable<Post> FetchPostsByTag(string tag)
        {
            return DataPost.FetchPostsByTag(tag).Select(x => ConvertDataPostToPost(x)).AsQueryable();
        }

        public IQueryable<Post> FetchPostsByTagAndCategory(string tagName, int categoryId)
        {
            return DataPost.FetchPostsByTagAndCategory(tagName, categoryId).Select(x => ConvertDataPostToPost(x)).AsQueryable();
        }

        public IQueryable<Post> FetchPostsByCategory(int categoryId) 
        {
            DataPostCollection dataPosts = new DataPostCollection();
            Query q = DataPost.CreateQuery();
            q.PageIndex = 0;
            q.AndWhere(DataPost.Columns.CategoryId, categoryId);

            dataPosts.LoadAndCloseReader(q.ExecuteReader());

            return dataPosts.Select(x => ConvertDataPostToPost(x)).AsQueryable();
        }

        public IQueryable<Post> FetchPostsByUsername(string username)
        {
            DataPostCollection pc = new DataPostCollection();
            Query q = DataPost.CreateQuery();
            q.AndWhere(DataPost.Columns.UserName, username);
            pc.LoadAndCloseReader(q.ExecuteReader());

            return pc.Select(x => ConvertDataPostToPost(x)).AsQueryable();
        }

        public IQueryable<Post> FetchRecentPosts(int numberOfPosts)
        {
            DataPostCollection dataPosts = new DataPostCollection();
            Query q = DataPostCollection.DefaultQuery(1, numberOfPosts, SortOrderType.Descending);
            dataPosts.LoadAndCloseReader(q.ExecuteReader());

            return dataPosts.Select(x => ConvertDataPostToPost(x)).AsQueryable();
        }

        public IQueryable<Post> FetchPosts(int numberOfPosts, Core.SortOrderType sortOrder)
        {
            Query q = DataPostCollection.DefaultQuery(1, numberOfPosts, (SortOrderType)Enum.Parse(typeof(SortOrderType), sortOrder.ToString()));
            DataPostCollection dataPosts = new DataPostCollection();
            dataPosts.LoadAndCloseReader(q.ExecuteReader());

            return dataPosts.Select(x => ConvertDataPostToPost(x)).AsQueryable();           
        }

        public IQueryable<Post> FetchPosts() 
        {
            return DataPostCollection.FetchAll().Select(x => ConvertDataPostToPost(x)).AsQueryable();
        }

        public int GetPostIdByName(string name)
        {
            return DataPost.GetPostIdByName(name);
        }

        public void UpdateViewCount(int postId)
        {
            DataPost.UpdateViewCount(postId);
        }

        public void UpdatePostStatus(int postId, PostStatus postStatus)
        {
            DataPost.UpdatePostStatus(postId, (int) postStatus);
        }

        public Post SavePost(Post post) 
        {
            DataPost dataPost = new DataPost(post.Id);
            dataPost = UpdateDataPostWithPost(dataPost, post);
            dataPost.Save();
            return ConvertDataPostToPost(dataPost);
        }

        public Post SavePost(Post post, string username)
        {
            DataPost dataPost = new DataPost(post.Id);
            dataPost = UpdateDataPostWithPost(dataPost, post);
            dataPost.Save(username);
            return ConvertDataPostToPost(dataPost);
        }

        public Post SavePost(Post post, string username, DateTime updateDate)
        {
            DataPost dataPost = new DataPost(post.Id);
            dataPost = UpdateDataPostWithPost(dataPost, post);
            dataPost.Save(username, updateDate);
            return ConvertDataPostToPost(dataPost);            
        }

        public void DeletePost(object postId)
        {
            DataPost.Delete(postId);
        }

        public void DestroyDeletedPost(int postId)
        {
            DataPost.DestroyDeletedPost(postId);
        }

        public void DestroyDeletedPosts()
        {
            DataPost.DestroyDeletedPosts();
        }

        public List<PostCount> GetPostCounts(IGraffitiUser user, int categoryId, string username, Delegate callback)
        {
            return DataPost.GetPostCounts(user, categoryId, username, callback);
        }

        public List<CategoryCount> GetCategoryCountForStatus(IGraffitiUser user, PostStatus postStatus, string author, Delegate categoryPermissionCheckcallback)
        {
            return DataPost.GetCategoryCountForStatus(user, postStatus, author, categoryPermissionCheckcallback);
        }

        public List<AuthorCount> GetAuthorCountForStatus(IGraffitiUser user, PostStatus status, string categoryId, Delegate categoryPermissionCheckCallback)
        {
            return DataPost.GetAuthorCountForStatus(user, status, categoryId, categoryPermissionCheckCallback);            
        }

        #endregion

        #region Comment
        
        public IQueryable<Comment> FetchComments()
        {
            DataCommentCollection dataComments = DataCommentCollection.FetchAll();

            return dataComments.Select(x => ConvertDataCommentToComment(x)).AsQueryable();
        }

        public Comment FetchComment(object commentId)
        {
            return ConvertDataCommentToComment(DataComment.FetchByColumn(DataComment.Columns.Id, commentId));
        }

        public Comment SaveComment(Comment comment) 
        {
            DataComment dataComment = new DataComment(comment.Id);
            dataComment = UpdateDataCommentWithComment(dataComment, comment);
            dataComment.Save();
            return ConvertDataCommentToComment(dataComment);
        }

        public Comment SaveComment(Comment comment, string username) {
            DataComment dataComment = new DataComment(comment.Id);
            dataComment = UpdateDataCommentWithComment(dataComment, comment);
            dataComment.Save(username);
            return ConvertDataCommentToComment(dataComment);
        }

        public void DeleteComment(int commentId)
        {
            DataComment.Delete(commentId);
        }

        public void DeleteDeletedComments()
        {
            DataComment.DeleteDeletedComments();
        }

        public void DeleteUnpublishedComments()
        {
            DataComment.DeleteUnpublishedComments();            
        }

        public int GetPublishedCommentCount(string username)
        {
            return DataComment.GetPublishedCommentCount(username);
        }

        #endregion

        #region Tag

        public IQueryable<Tag> FetchAllTags()
        {
            return DataTagCollection.FetchAll().Select(x => ConvertDataTagToTag(x)).AsQueryable();
        }

        public IQueryable<TagWeight> FetchAllTagWeights()
        {
            return DataTagWeightCollection.FetchAll().Select(x => ConvertDataTagWeightToTagWeight(x)).AsQueryable();
        }

        #endregion

        #region Categories

        public IQueryable<Category> FetchAllCategories()
        {
            return DataCategoryCollection.FetchAll().Select(x => ConvertDataCategoryToCategory(x)).AsQueryable();
        }

        public Category FetchCategory(object id)
        {
            return ConvertDataCategoryToCategory(new DataCategory(id));
        }

        public Category FetchCategoryByUniqueId(Guid uniqueId)
        {
            return ConvertDataCategoryToCategory(DataCategory.FetchByColumn(DataCategory.Columns.UniqueId, uniqueId));
        }

        public Category SaveCategory(Category category) {
            DataCategory dataCategory = new DataCategory(category.Id);
            dataCategory = UpdateDataCategoryWithCategory(dataCategory, category);
            dataCategory.Save();
            return ConvertDataCategoryToCategory(dataCategory);
        }

        public Category SaveCategory(Category category, string username) {
            DataCategory dataCategory = new DataCategory(category.Id);
            dataCategory = UpdateDataCategoryWithCategory(dataCategory, category);
            dataCategory.Save(username);
            return ConvertDataCategoryToCategory(dataCategory);
        }

        public void DestroyCategory(int categoryId)
        {
            DataCategory.Destroy(categoryId);
        }

        #endregion

        #region User

        public User FetchUserByUsername(string username)
        {
            return ConvertDataUserToUser(DataUser.FetchByColumn(DataUser.Columns.Name, username));
        }

        public User FetchUserById(int id)
        {
            return ConvertDataUserToUser(DataUser.FetchByColumn(DataUser.Columns.Id, id));
        }

        public User FetchUserByUniqueId(Guid uniqueId)
        {
            return ConvertDataUserToUser(DataUser.FetchByColumn(DataUser.Columns.UniqueId, uniqueId));            
        }

        public User SaveUser(User user)
        {
            DataUser dataUser = new DataUser(user.Id);
            dataUser = UpdateDataUserWithUser(dataUser, user);
            dataUser.Save();
            return ConvertDataUserToUser(dataUser);            
        }

        public User SaveUser(User user, string modifiedBy) 
        {
            DataUser dataUser = new DataUser(user.Id);
            dataUser = UpdateDataUserWithUser(dataUser, user);
            dataUser.Save(modifiedBy);
            return ConvertDataUserToUser(dataUser);
        }

        public void DestroyUser(Guid uniqueId)
        {
            DataUser.Destroy(DataUser.Columns.UniqueId, uniqueId);
        }

        #endregion

        #region UserRole

        public IQueryable<UserRole> FetchUserRolesForUserByRoleName(int userId, string roleName)
        {
            Query q = DataUserRole.CreateQuery();
            q.AndWhere(DataUserRole.Columns.UserId, userId);
            q.AndWhere(DataUserRole.Columns.RoleName, roleName);
            DataUserRoleCollection urCol = new DataUserRoleCollection();
            urCol.LoadAndCloseReader(q.ExecuteReader());

            return urCol.Select(x => ConvertDataUserRoleToUserRole(x)).AsQueryable();
        }

        public UserRole SaveUserRole(UserRole userRole)
        {
            DataUserRole dataUserRole = new DataUserRole(userRole.Id);
            dataUserRole = UpdateDataUserRoleWithUserRole(dataUserRole, userRole);
            dataUserRole.Save();
            return ConvertDataUserRoleToUserRole(dataUserRole);               
        }

        public void DestroyUserRole(int userRoleId)
        {
            DataUserRole.Destroy(userRoleId); 
        }

        public List<string> FetchUsernamesInRole(string roleName)
        {
            QueryCommand command = new QueryCommand("SELECT u.Name FROM graffiti_Users AS u INNER JOIN graffiti_UserRoles AS ur on u.Id = ur.UserId WHERE ur.RoleName = " + DataService.Provider.SqlVariable("RoleName"));
            command.Parameters.Add(DataUserRole.FindParameter("RoleName")).Value = roleName;
            List<string> userNames = new List<string>();
            using (IDataReader reader = DataService.ExecuteReader(command)) {
                while (reader.Read()) {
                    userNames.Add(reader["Name"] as string);
                }

                reader.Close();
            }
            return userNames;
        }

        #endregion

        #region ObjectStore

        public ObjectStore FetchObjectStoreByName(string name)
        {
            return ConvertDataObjectStoreToObjectStore(DataObjectStore.FetchByColumn(DataObjectStore.Columns.Name, name));
        }

        public ObjectStore FetchObjectStoreByUniqueId(Guid uniqueId)
        {
            return
                ConvertDataObjectStoreToObjectStore(DataObjectStore.FetchByColumn(DataObjectStore.Columns.UniqueId,
                                                                                  uniqueId));
        }

        public IQueryable<ObjectStore> FetchObjectStoresByContentType(string contentType)
        {
            DataObjectStoreCollection osc = new DataObjectStoreCollection();
            Query q = DataObjectStore.CreateQuery();
            q.AndWhere(DataObjectStore.Columns.ContentType, contentType);
            osc.LoadAndCloseReader(q.ExecuteReader());

            return osc.Select(x => ConvertDataObjectStoreToObjectStore(x)).AsQueryable();
        }

        public ObjectStore SaveObjectStore(ObjectStore store)
        {
            DataObjectStore dataStore = new DataObjectStore(store.Id);
            dataStore = UpdateDataObjectStoreWithObjectStore(dataStore, store);
            dataStore.Save();
            return ConvertDataObjectStoreToObjectStore(dataStore);
        }

        public ObjectStore SaveObjectStore(ObjectStore store, string username) 
        {
            DataObjectStore dataStore = new DataObjectStore(store.Id);
            dataStore = UpdateDataObjectStoreWithObjectStore(dataStore, store);
            dataStore.Save(username);
            return ConvertDataObjectStoreToObjectStore(dataStore);
        }

        public void DestroyObjectStore(int id)
        {
            DataObjectStore.Destroy(id);
        }

        #endregion

        #region RolePermissions

        public IQueryable<RolePermissions> FetchAllRolePermissions()
        {
            return DataRolePermissionsCollection.FetchAll().Select(x => ConvertDataRolePermissionsToRolePermissions(x)).AsQueryable();
        }

        public IQueryable<RoleCategoryPermissions> FetchAllRoleCategoryPermissions()
        {
            return
                DataRoleCategoryPermissionsCollection.FetchAll().Select(
                    x => ConvertDataRoleCategoryPermissionsToRoleCategoryPermissions(x)).AsQueryable();
        }

        public void DestroyRolePermission(int id)
        {
            DataRolePermissions.Destroy(id);
        }
        
        public void DestroyRolePermission(string roleName)
        {
            DataRolePermissions.Destroy(DataRolePermissions.Columns.RoleName, roleName);
        }

        public void DestroyRoleCategoryPermission(int id)
        {
            DataRoleCategoryPermissions.Destroy(id);
        }

        public void DestroyRoleCategoryPermission(string roleName)
        {
            DataRoleCategoryPermissions.Destroy(DataRoleCategoryPermissions.Columns.RoleName, roleName);
        }

        public RolePermissions SaveRolePermission(RolePermissions rolePermissions)
        {
            DataRolePermissions dataRolePermissions = new DataRolePermissions(rolePermissions.Id);
            dataRolePermissions = UpdateDataRolePermissionsWithRolePermissions(dataRolePermissions, rolePermissions);
            dataRolePermissions.Save();
            return ConvertDataRolePermissionsToRolePermissions(dataRolePermissions);                
        }

        public RoleCategoryPermissions SaveRoleCategoryPermission(RoleCategoryPermissions roleCategoryPermissions)
        {
            DataRoleCategoryPermissions dataRoleCategoryPermissions = new DataRoleCategoryPermissions(roleCategoryPermissions.Id);
            dataRoleCategoryPermissions = UpdateDataRoleCategoryPermissionsWithRoleCategoryPermissions(dataRoleCategoryPermissions, roleCategoryPermissions);
            dataRoleCategoryPermissions.Save();
            return ConvertDataRoleCategoryPermissionsToRoleCategoryPermissions(dataRoleCategoryPermissions);             
        }

        #endregion

        #region Log

        public IQueryable<Log> FetchByType(string type, int pageIndex, int pageSize, out int totalCount)
        {
            Query q = DataLog.CreateQuery();
            q.AndWhere(DataLog.Columns.Type, type);
            q.PageSize = pageSize;
            q.PageIndex = pageIndex;
            q.OrderByDesc(DataLog.Columns.CreatedOn);

            totalCount = q.GetRecordCount();
            return DataLogCollection.FetchByQuery(q).Select(x => ConvertDataLogToLog(x)).AsQueryable();
        }

        public void RemoveLogsOlderThan(int hours)
        {
            DataLog.RemoveLogsOlderThan(hours);
        }

        public Log SaveLog(Log log, string username, DateTime modifiedTime)
        {
            DataLog dataLog = new DataLog(log.Id);
            dataLog = UpdateDataLogWithLog(dataLog, log);
            dataLog.Save(username, modifiedTime);
            return ConvertDataLogToLog(dataLog);
        }

        #endregion

        #region VersionStore

        public IQueryable<VersionStore> FetchVersionHistory(string filename, bool checkLicensed)
        {
            return DataVersionStore.GetVersionHistory(filename, checkLicensed).Select(x => ConvertDataVersionStoreToVersionStore(x)).AsQueryable();
        }

        public IQueryable<VersionStore> FetchVersionHistory(int postId)
        {
            return DataVersionStore.GetVersionHistory(postId).Select(x => ConvertDataVersionStoreToVersionStore(x)).AsQueryable();
        }

        public IQueryable<VersionStore> FetchVersionHistoryByPostId(int postId, int version) 
        {
            Query q = DataVersionStore.CreateQuery();
            q.AndWhere(DataVersionStore.Columns.ItemId, postId);
            if (version > 0)
                q.AndWhere(DataVersionStore.Columns.Version, version);

            return
                DataVersionStoreCollection.FetchByQuery(q).Select(x => ConvertDataVersionStoreToVersionStore(x)).
                    AsQueryable();
        }

        public void DestroyVersionStore(Guid uniqueId)
        {
            DataVersionStore.Destroy(DataVersionStore.Columns.UniqueId, uniqueId);
        }

        public int VersionFile(FileInfo fileInfo, string username, DateTime saved)
        {
            return DataVersionStore.VersionFile(fileInfo, username, saved);
        }

        public VersionStore SaveVersionStore(VersionStore versionStore, string username)
        {
            DataVersionStore dataVersionStore = new DataVersionStore(versionStore.Id);
            dataVersionStore = UpdateDataVersionStoreWithVersionStore(dataVersionStore, versionStore);
            dataVersionStore.Save(username);
            return ConvertDataVersionStoreToVersionStore(dataVersionStore);
        }

        public int GetNextVersionId(int postId, int currentVersionId)
        {
            QueryCommand command = new QueryCommand("Select Max(v.Version) FROM graffiti_VersionStore as v where v.Name = " + DataService.Provider.SqlVariable("Name"));
            command.Parameters.Add(DataVersionStore.FindParameter("Name")).Value = "Post:" + postId.ToString();
            object obj = DataService.ExecuteScalar(command);
            if (obj == null || obj is System.DBNull)
                return 2;
            else
                return Math.Max(((int)obj), currentVersionId) + 1;    
        }

        public int CurrentVersion(FileInfo fileInfo)
        {
            return DataVersionStore.CurrentVersion(fileInfo);
        }

        #endregion

        #region Reports

        public IDictionary<DateTime, int> ViewsByDate(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.ViewsByDate(min, max, getInclauseForReadPermissionsCallback);
        }

        public ReportData ViewsByDateSingle(DateTime date, Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.ViewsByDateSingle(date, getInclauseForReadPermissionsCallback);
        }

        public ReportData ViewsByPost(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.ViewsByPost(min, max, getInclauseForReadPermissionsCallback);
        }

        public IDictionary<DateTime, int> ViewsByPostSingle(int postId, DateTime min, DateTime max) 
        {
            return DataReports.ViewsByPostSingle(postId, min, max);
        }

        public int ViewsByPostSingleCount(int postId, DateTime min, DateTime max) 
        {
            return DataReports.ViewsByPostSingleCount(postId, min, max);
        }

        public ReportData CommentsByDateSingle(DateTime date, Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.CommentsByDateSingle(date, getInclauseForReadPermissionsCallback);
        }

        public ReportData CommentsByPost(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.CommentsByPost(min, max, getInclauseForReadPermissionsCallback);
        }

        public IDictionary<DateTime, int> CommentsByPostSingle(int postId, DateTime min, DateTime max) 
        {
            return DataReports.CommentsByPostSingle(postId, min, max);
        }

        public int CommentsByPostSingleCount(int postId, DateTime min, DateTime max) 
        {
            return DataReports.CommentsByPostSingleCount(postId, min, max);
        }

        public IDictionary<DateTime, int> CommentsByDate(DateTime min, DateTime max, Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.CommentsByDate(min, max, getInclauseForReadPermissionsCallback);
        }

        public ReportData MostPopularPosts(Delegate getInclauseForReadPermissionsCallback) 
        {
            return DataReports.MostPopularPosts(getInclauseForReadPermissionsCallback);
        }

        #endregion

        #region Mappers

        private VersionStore ConvertDataVersionStoreToVersionStore(DataVersionStore data)
        {
            VersionStore versionStore = new VersionStore();

            versionStore.CreatedBy = data.CreatedBy;
            versionStore.CreatedOn = data.CreatedOn;
            versionStore.Data = data.Data;
            versionStore.Id = data.Id;
            versionStore.ItemId = data.ItemId;
            versionStore.Name = data.Name;
            versionStore.Notes = data.Notes;
            versionStore.Type = data.Type;
            versionStore.UniqueId = data.UniqueId;
            versionStore.Version = data.Version;

            return versionStore;
        }

        private DataVersionStore UpdateDataVersionStoreWithVersionStore(DataVersionStore data, VersionStore versionStore)
        {
            data.CreatedBy = versionStore.CreatedBy;
            data.CreatedOn = versionStore.CreatedOn;
            data.Data = versionStore.Data;
            data.Id = versionStore.Id;
            data.ItemId = versionStore.ItemId;
            data.Name = versionStore.Name;
            data.Notes = versionStore.Notes;
            data.Type = versionStore.Type;
            data.UniqueId = versionStore.UniqueId;
            data.Version = versionStore.Version;

            return data;
        }

        private DataLog UpdateDataLogWithLog(DataLog data, Log log)
        {
            data.CreatedBy = log.CreatedBy;
            data.CreatedOn = log.CreatedOn;
            //data.Id = log.Id;
            data.Message = log.Message;
            data.Title = log.Title;
            data.Type = log.Type;

            return data;
        }

        private Log ConvertDataLogToLog(DataLog data)
        {
            Log log = new Log();

            log.CreatedBy = data.CreatedBy;
            log.CreatedOn = data.CreatedOn;
            log.Id = data.Id;
            log.Message = data.Message;
            log.Title = data.Title;
            log.Type = data.Type;

            return log;
        }

        private DataRolePermissions UpdateDataRolePermissionsWithRolePermissions(DataRolePermissions data, RolePermissions rolePermissions)
        {
            data.CreatedBy = rolePermissions.CreatedBy;
            data.HasEdit = rolePermissions.HasEdit;
            data.HasPublish = rolePermissions.HasPublish;
            data.HasRead = rolePermissions.HasRead;
            data.Id = rolePermissions.Id;
            data.ModifiedBy = rolePermissions.ModifiedBy;
            data.ModifiedOn = rolePermissions.ModifiedOn;
            data.RoleName = rolePermissions.RoleName;

            return data;
        }

        private DataRoleCategoryPermissions UpdateDataRoleCategoryPermissionsWithRoleCategoryPermissions(DataRoleCategoryPermissions data, RoleCategoryPermissions roleCategoryPermissions) 
        {
            data.CategoryId = roleCategoryPermissions.CategoryId;
            data.CreatedBy = roleCategoryPermissions.CreatedBy;
            data.HasEdit = roleCategoryPermissions.HasEdit;
            data.HasPublish = roleCategoryPermissions.HasPublish;
            data.HasRead = roleCategoryPermissions.HasRead;
            data.Id = roleCategoryPermissions.Id;
            data.ModifiedBy = roleCategoryPermissions.ModifiedBy;
            data.ModifiedOn = roleCategoryPermissions.ModifiedOn;
            data.RoleName = roleCategoryPermissions.RoleName;

            return data;
        }

        private RoleCategoryPermissions ConvertDataRoleCategoryPermissionsToRoleCategoryPermissions(DataRoleCategoryPermissions data) 
        {
            RoleCategoryPermissions roleCategoryPermissions = new RoleCategoryPermissions();

            roleCategoryPermissions.CategoryId = data.CategoryId;
            roleCategoryPermissions.CreatedBy = data.CreatedBy;
            roleCategoryPermissions.HasEdit = data.HasEdit;
            roleCategoryPermissions.HasPublish = data.HasPublish;
            roleCategoryPermissions.HasRead = data.HasRead;
            roleCategoryPermissions.Id = data.Id;
            roleCategoryPermissions.ModifiedBy = data.ModifiedBy;
            roleCategoryPermissions.ModifiedOn = data.ModifiedOn;
            roleCategoryPermissions.RoleName = data.RoleName;

            return roleCategoryPermissions;
        }

        private RolePermissions ConvertDataRolePermissionsToRolePermissions(DataRolePermissions data)
        {
            RolePermissions rolePermissions = new RolePermissions();

            rolePermissions.CreatedBy = data.CreatedBy;
            rolePermissions.HasEdit = data.HasEdit;
            rolePermissions.HasPublish = data.HasPublish;
            rolePermissions.HasRead = data.HasRead;
            rolePermissions.Id = data.Id;
            rolePermissions.ModifiedBy = data.ModifiedBy;
            rolePermissions.ModifiedOn = data.ModifiedOn;
            rolePermissions.RoleName = data.RoleName;

            return rolePermissions;
        }

        private UserRole ConvertDataUserRoleToUserRole(DataUserRole data)
        {
            UserRole userRole = new UserRole();

            userRole.Id = data.Id;
            userRole.RoleName = data.RoleName;
            userRole.UserId = data.UserId;

            return userRole;
        }

        private DataUserRole UpdateDataUserRoleWithUserRole(DataUserRole data, UserRole userRole)
        {
            data.Id = userRole.Id;
            data.RoleName = userRole.RoleName;
            data.UserId = userRole.UserId;

            return data;
        }

        private DataUser UpdateDataUserWithUser(DataUser data, User user)
        {
            data.Avatar = user.Avatar;
            data.Bio = user.Bio;
            data.Email = user.Email;
            data.Id = user.Id;
            data.Name = user.Name;
            data.Password = user.Password;
            data.PasswordFormat = user.PasswordFormat;
            data.PasswordSalt = user.PasswordSalt;
            data.ProperName = user.ProperName;
            data.PublicEmail = user.PublicEmail;
            data.TimeZoneOffSet = user.TimeZoneOffSet;
            data.UniqueId = user.UniqueId;
            data.WebSite = user.WebSite;

            return data;
        }

        private User ConvertDataUserToUser(DataUser data)
        {
            User user = new User();

            user.Avatar = data.Avatar;
            user.Bio = data.Bio;
            user.Email = data.Email;
            user.Id = data.Id;
            user.IsLoaded = data.IsLoaded;
            user.IsNew = data.IsNew;
            user.Name = data.Name;
            user.Password = data.Password;
            user.PasswordFormat = data.PasswordFormat;
            user.PasswordSalt = data.PasswordSalt;
            user.ProperName = data.ProperName;
            user.PublicEmail = data.PublicEmail;
            user.Roles = data.Roles;
            user.TimeZoneOffSet = data.TimeZoneOffSet;
            user.UniqueId = data.UniqueId;
            user.WebSite = data.WebSite;

            return user;
        }

        private Comment ConvertDataCommentToComment(DataComment data) 
        {
            Comment comment = new Comment();

            comment.Body = data.Body;
            comment.CreatedBy = data.CreatedBy;
            comment.DontChangeUser = data.DontChangeUser;
            comment.Email = data.Email;
            comment.IPAddress = data.IPAddress;
            comment.Id = data.Id;
            comment.IsDeleted = data.IsDeleted;
            comment.IsLoaded = data.IsLoaded;
            comment.IsNew = data.IsNew;
            comment.IsPublished = data.IsPublished;
            comment.IsTrackback = data.IsTrackback;
            comment.ModifiedBy = data.ModifiedBy;
            comment.ModifiedOn = data.ModifiedOn;
            comment.Name = data.Name;
            comment.Post = ConvertDataPostToPost(data.Post);
            comment.PostId = data.PostId;
            comment.SpamScore = data.SpamScore;
            comment.Published = data.Published;
            comment.UniqueId = data.UniqueId;
            comment.UserName = data.UserName;
            comment.User = data.User;
            comment.Version = data.Version;
            comment.WebSite = data.WebSite;

            return comment;
        }

        private DataComment UpdateDataCommentWithComment(DataComment data, Comment comment) 
        {
            data.Body = comment.Body;
            data.CreatedBy = comment.CreatedBy;
            data.DontChangeUser = comment.DontChangeUser;
            data.Email = comment.Email;
            data.IPAddress = comment.IPAddress;
            data.Id = comment.Id;
            data.IsDeleted = comment.IsDeleted;
            data.IsPublished = comment.IsPublished;
            data.IsTrackback = comment.IsTrackback;
            data.ModifiedBy = comment.ModifiedBy;
            data.ModifiedOn = comment.ModifiedOn;
            data.Name = comment.Name;
            data.PostId = comment.PostId;
            data.SpamScore = comment.SpamScore;
            data.Published = comment.Published;
            data.UniqueId = comment.UniqueId;
            data.UserName = comment.UserName;
            data.Version = comment.Version;
            data.WebSite = comment.WebSite;

            return data;
        }

        private Post ConvertDataPostToPost(DataPost data)
        {
            Post post = new Post();

            post.CategoryId = data.CategoryId;
            post.CommentCount = data.CommentCount;
            post.ContentType = data.ContentType;
            post.CreatedBy = data.CreatedBy;
            post.CreatedOn = data.CreatedOn;
            post.EnableComments = data.EnableComments;
            post.ExtendedBody = data.ExtendedBody;
            post.HomeSortOrder = data.HomeSortOrder;
            post.Id = data.Id;
            post.ImageUrl = data.ImageUrl;
            post.IsDeleted = data.IsDeleted;
            post.IsHome = data.IsHome;
            post.IsLoaded = data.IsLoaded;
            post.IsNew = data.IsNew;
            post.IsPublished = data.IsPublished;
            post.MetaDescription = data.MetaDescription;
            post.MetaKeywords = data.MetaKeywords;
            post.ModifiedBy = data.ModifiedBy;
            post.ModifiedOn = data.ModifiedOn;
            post.Name = data.Name;
            post.Notes = data.Notes;
            post.ParentId = data.ParentId;
            post.PendingCommentCount = data.PendingCommentCount;
            post.PostBody = data.PostBody;
            post.PropertyKeys = data.PropertyKeys;
            post.PropertyValues = data.PropertyValues;
            post.Published = data.Published;
            post.SortOrder = data.SortOrder;
            post.Status = data.Status;
            post.TagList = data.TagList;
            post.Title = data.Title;
            post.UniqueId = data.UniqueId;
            post.UserName = data.UserName;
            post.Version = data.Version;
            post.Views = data.Views;

            return post;
        }

        private DataPost UpdateDataPostWithPost(DataPost data, Post post)
        {
            data.CategoryId = post.CategoryId;
            data.CommentCount = post.CommentCount;
            data.ContentType = post.ContentType;
            data.CreatedBy = post.CreatedBy;
            data.CreatedOn = post.CreatedOn;
            data.EnableComments = post.EnableComments;
            data.ExtendedBody = post.ExtendedBody;
            data.HomeSortOrder = post.HomeSortOrder;
            //data.Id = post.Id;
            data.ImageUrl = post.ImageUrl;
            data.IsDeleted = post.IsDeleted;
            data.IsHome = post.IsHome;
            data.IsPublished = post.IsPublished;
            data.MetaDescription = post.MetaDescription;
            data.MetaKeywords = post.MetaKeywords;
            data.ModifiedBy = post.ModifiedBy;
            data.ModifiedOn = post.ModifiedOn;
            data.Name = post.Name;
            data.Notes = post.Notes;
            data.ParentId = post.ParentId;
            data.PendingCommentCount = post.PendingCommentCount;
            data.PostBody = post.PostBody;
            data.PropertyKeys = post.PropertyKeys;
            data.PropertyValues = post.PropertyValues;
            data.Published = post.Published;
            data.SortOrder = post.SortOrder;
            data.Status = post.Status;
            data.TagList = post.TagList;
            data.Title = post.Title;
            data.UniqueId = post.UniqueId;
            data.UserName = post.UserName;
            data.Version = post.Version;
            data.Views = post.Views;

            return data;
        }

        private Tag ConvertDataTagToTag(DataTag data)
        {
            Tag tag = new Tag();

            tag.Id = data.Id;
            tag.Name = data.Name;
            tag.PostId = data.PostId;

            return tag;
        }

        private TagWeight ConvertDataTagWeightToTagWeight(DataTagWeight data)
        {
            TagWeight tagWeight = new TagWeight();

            tagWeight.FontFactor = data.FontFactor;
            tagWeight.FontSize = data.FontSize;
            tagWeight.Name = data.Name;
            tagWeight.Weight = data.Weight;

            return tagWeight;
        }

		  private Category ConvertDataCategoryToCategory(DataCategory data)
		  {
			  return ConvertDataCategoryToCategory(data, null);
		  }

        private Category ConvertDataCategoryToCategory(DataCategory data, Category parent)
        {
            Category category = new Category();

            category.Body = data.Body;
            category.ExcludeSubCategoryPosts = data.ExcludeSubCategoryPosts;
            category.FeaturedId = data.FeaturedId;
            category.FeedUrlOverride = data.FeedUrlOverride;
            category.FormattedName = data.FormattedName;
            category.Id = data.Id;
            category.ImageUrl = data.ImageUrl;
            category.InitialCategoryName = data.LinkName;
            category.IsDeleted = data.IsDeleted;
            category.IsLoaded = data.IsLoaded;
            category.IsNew = data.IsNew;
            category.IsUncategorized = data.IsUncategorized;
            category.LinkName = data.LinkName;
            category.MetaDescription = data.MetaDescription;
            category.MetaKeywords = data.MetaKeywords;
            category.Name = data.Name;
				if (parent != null)
					category.Parent = parent;
			   else if (data.Parent != null)
					category.Parent = ConvertDataCategoryToCategory(data.Parent);
            category.ParentId = data.ParentId;
            category.PostCount = data.PostCount;
            category.PostView = data.PostView;
            category.SortOrder = (Core.SortOrderType)(Enum.Parse(typeof(Core.SortOrderType), data.SortOrder.ToString()));
            category.SortOrderTypeId = data.SortOrderTypeId;
            category.Type = data.Type;
            category.UniqueId = data.UniqueId;
            category.View = data.View;

            foreach (DataCategory dataCategory in data.Children)
            {
                category.Children.Add(ConvertDataCategoryToCategory(dataCategory, category));
            }

            return category;
        }

        private DataCategory UpdateDataCategoryWithCategory(DataCategory data, Category category) 
        {
            data.Body = category.Body;
            data.ExcludeSubCategoryPosts = category.ExcludeSubCategoryPosts;
            data.FeaturedId = category.FeaturedId;
            data.FeedUrlOverride = category.FeedUrlOverride;
            data.FormattedName = category.FormattedName;
            //data.Id = category.Id;
            data.ImageUrl = category.ImageUrl;
            data.IsDeleted = category.IsDeleted;
            data.LinkName = category.LinkName;
            data.MetaDescription = category.MetaDescription;
            data.MetaKeywords = category.MetaKeywords;
            data.Name = category.Name;
            data.ParentId = category.ParentId;
            data.PostCount = category.PostCount;
            data.PostView = category.PostView;
            data.SortOrder = (SortOrderType)(Enum.Parse(typeof(SortOrderType), category.SortOrder.ToString()));
            data.SortOrderTypeId = category.SortOrderTypeId;
            data.Type = category.Type;
            data.UniqueId = category.UniqueId;
            data.View = category.View;

            if (category.Children != null)
            {
                data.Children.Clear();
                foreach (Category cat in category.Children)
                {
                    data.Children.Add(new DataCategory(cat.Id));
                }
            }

            return data;
        }

        private DataObjectStore UpdateDataObjectStoreWithObjectStore(DataObjectStore data, ObjectStore store) 
        {
            data.ContentType = store.ContentType;
            data.CreatedOn = store.CreatedOn;
            data.Data = store.Data;
            data.Id = store.Id;
            data.ModifiedOn = store.ModifiedOn;
            data.Name = store.Name;
            data.Type = store.Type;
            data.UniqueId = store.UniqueId;
            data.Version = store.Version;

            return data;
        }

        private ObjectStore ConvertDataObjectStoreToObjectStore(DataObjectStore data) 
        {
            ObjectStore store = new ObjectStore();

            store.ContentType = data.ContentType;
            store.CreatedOn = data.CreatedOn;
            store.Data = data.Data;
            store.Id = data.Id;
            store.IsLoaded = data.IsLoaded;
            store.IsNew = data.IsNew;
            store.ModifiedOn = data.ModifiedOn;
            store.Name = data.Name;
            store.Type = data.Type;
            store.UniqueId = data.UniqueId;
            store.Version = data.Version;

            return store;
        }

        #endregion
    }
}
