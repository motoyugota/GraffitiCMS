using System.Linq;
using Graffiti.Core;

namespace Graffiti.Data 
{
    public interface ITagRepository 
    {
        IQueryable<Tag> FetchAllTags();
        IQueryable<TagWeight> FetchAllTagWeights();
    }
}
