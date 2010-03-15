using System;
using System.Collections.Generic;
using System.IO;

namespace Graffiti.Core.Services 
{
    public interface IVersionStoreService 
    {
        IList<VersionStore> FetchVersionHistory(string filename, bool checkLicensed);
        IList<VersionStore> FetchVersionHistory(int postId);
        IList<VersionStore> FetchVersionStoreByPostId(int postId, int version);
        VersionStore SaveVersionStore(VersionStore versionStore, string username);
        int VersionFile(FileInfo fileInfo, string username, DateTime saved);
        int GetNextVersionId(int postId, int currentVersionId);
        int CurrentVersion(FileInfo fileInfo);
		void DestroyVersionStore(Guid uniqueId);
		void DestroyVersionStore(string filePath);
	}
}