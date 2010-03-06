using System;
using System.IO;
using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data
{
    public interface IVersionStoreRepository 
    {
        IQueryable<VersionStore> FetchVersionHistory(string filename, bool checkLicensed);
        IQueryable<VersionStore> FetchVersionHistory(int postId);
        IQueryable<VersionStore> FetchVersionHistoryByPostId(int postId, int version);
        int VersionFile(FileInfo fileInfo, string username, DateTime saved);
        int GetNextVersionId(int postId, int currentVersionId);
        VersionStore SaveVersionStore(VersionStore versionStore, string username);
        int CurrentVersion(FileInfo fileInfo);
        void DestroyVersionStore(Guid uniqueId);
    }
}
