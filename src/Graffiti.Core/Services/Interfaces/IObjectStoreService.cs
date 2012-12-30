using System;
using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface IObjectStoreService 
    {
        ObjectStore SaveObjectStore(ObjectStore store);
        ObjectStore SaveObjectStore(ObjectStore store, string username);
        void DestroyObjectStore(int id);
        ObjectStore FetchByName(string name);
        ObjectStore FetchByUniqueId(Guid uniqueId);
        IList<ObjectStore> FetchByNameAndContentType(string name, string contentType);
        IList<ObjectStore> FetchByContentType(string contentType);
    }
}
