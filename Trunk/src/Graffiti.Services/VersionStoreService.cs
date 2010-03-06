using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class VersionStoreService : IVersionStoreService
    {
        private IVersionStoreRepository _versionStoreRepository;

        public VersionStoreService(IVersionStoreRepository versionStoreRepository)
        {
            _versionStoreRepository = versionStoreRepository;
        }

        public IList<VersionStore> FetchVersionHistory(string filename, bool checkLicensed)
        {
            return _versionStoreRepository.FetchVersionHistory(filename, checkLicensed).ToList();
        }

        public IList<VersionStore> FetchVersionHistory(int postId)
        {
            return _versionStoreRepository.FetchVersionHistory(postId).ToList();
        }

        public IList<VersionStore> FetchVersionStoreByPostId(int postId, int version)
        {
            return _versionStoreRepository.FetchVersionHistoryByPostId(postId, version).ToList();
        }

        public int VersionFile(FileInfo fileInfo, string username, DateTime saved)
        {
            return _versionStoreRepository.VersionFile(fileInfo, username, saved);
        }

        public int GetNextVersionId(int postId, int currentVersionId)
        {
            return _versionStoreRepository.GetNextVersionId(postId, currentVersionId);
        }

        public VersionStore SaveVersionStore(VersionStore versionStore, string username)
        {
            return _versionStoreRepository.SaveVersionStore(versionStore, username);
        }

        public int CurrentVersion(FileInfo fileInfo)
        {
            return _versionStoreRepository.CurrentVersion(fileInfo);
        }

        public void DestroyVersionStore(Guid uniqueId)
        {
            _versionStoreRepository.DestroyVersionStore(uniqueId);
        }
    }
}
