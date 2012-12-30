using System.Collections.Generic;

namespace Graffiti.Core.Services 
{
    public interface ITagService 
    {
        IList<Tag> FetchAllTags();
        IList<TagWeight> FetchAllTagWeights();
    }
}
