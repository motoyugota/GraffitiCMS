using System;
using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface IObjectStoreRepository 
    {
        ObjectStore FetchObjectStoreByName(string name);
        ObjectStore FetchObjectStoreByUniqueId(Guid uniqueId);
        IQueryable<ObjectStore> FetchObjectStoresByContentType(string contentType);
        ObjectStore SaveObjectStore(ObjectStore objectStore);
        ObjectStore SaveObjectStore(ObjectStore objectStore, string username);
        void DestroyObjectStore(int id);
    }
}
