<%@ Page Language="C#" AutoEventWireup="true"  Inherits="Graffiti.Core.TemplatedThemePage" ClassName="GraffitiHomePage" %>
<%@ Import Namespace="System.Linq"%>
<%@ Import namespace="DataBuddy"%>
<script runat="Server">

    protected override string ViewLookUp(string baseName, string defaultViewName)
    {
        if (ViewExists("home" + baseName))
            return "home" + baseName;
        else
            return base.ViewLookUp(baseName, defaultViewName);

    }

    public override bool IsIndexable
    {
        get
        {
            return PageIndex <= 1;
        }
        set
        {
            base.IsIndexable = value;
        }
    }

    protected override void LoadContent(GraffitiContext graffitiContext)
    {

        graffitiContext["where"] = "home";
        graffitiContext["title"] = SiteSettings.Get().Title;

        if (SiteSettings.Get().UseCustomHomeList)
        {
            graffitiContext.RegisterOnRequestDelegate("posts", GetCustomHomeSortPosts);
            
            // GetCustomHomeSortPosts needs to be called so the pager works
            GetCustomHomeSortPosts("posts", graffitiContext);
        }
        else
        {
            graffitiContext.RegisterOnRequestDelegate("posts", GetDefaultPosts);
            
            // GetDefaultPosts needs to be called so the pager works
            GetDefaultPosts("posts", graffitiContext);
        }
    }
    
    protected object GetCustomHomeSortPosts(string key, GraffitiContext graffitiContext)
    {
        PostCollection pc = ZCache.Get<PostCollection>(CacheKey + ":customhome:" + PageIndex);
        if (pc == null)
        {
            pc = new PostCollection(_postService.HomeQueryOverride(PageIndex-1, SiteSettings.Get().PageSize));
            ZCache.InsertCache(CacheKey + ":customhome:" + PageIndex, pc, 30);
        }

        object postCount = ZCache.Get<object>(CacheKey + ":customhome:" +"Count");
        if (postCount == null)
        {
            postCount = new PostCollection(_postService.HomeQueryOverride(-1, -1)).Count;
            ZCache.InsertCache(CacheKey + ":customhome:" + "Count", postCount, 60);
        }

        graffitiContext.TotalRecords = (int)postCount;
        graffitiContext.PageIndex = PageIndex;
        graffitiContext.PageSize = SiteSettings.Get().PageSize;

        PostCollection permissionsFiltered = new PostCollection();
        permissionsFiltered.AddRange(pc);

        foreach (Post p in pc)
        {
            if (!_rolePermissionService.GetPermissions(p.CategoryId, GraffitiUsers.Current).Read)
                permissionsFiltered.Remove(p);
        }
        
        return permissionsFiltered;
    }

    protected object GetDefaultPosts(string key, GraffitiContext graffitiContext)
    {
        PostCollection pc = ZCache.Get<PostCollection>(CacheKey + PageIndex);
        if (pc == null)
        {
            pc = new PostCollection(_postService.DefaultQuery(PageIndex, SiteSettings.Get().PageSize, SortOrderType.Descending));
            ZCache.InsertCache(CacheKey + PageIndex, pc, 30);
        }

        object postCount = ZCache.Get<object>(CacheKey + "Count");
        if (postCount == null)
        {
            postCount = new PostCollection(_postService.DefaultQuery()).Count;
            ZCache.InsertCache(CacheKey + "Count", postCount, 60);
        }

        graffitiContext.TotalRecords = (int)postCount;
        graffitiContext.PageIndex = PageIndex;
        graffitiContext.PageSize = SiteSettings.Get().PageSize;


        PostCollection permissionsFiltered = new PostCollection();
        permissionsFiltered.AddRange(pc);

        foreach (Post p in pc)
        {
            if (!_rolePermissionService.GetPermissions(p.CategoryId, GraffitiUsers.Current).Read)
                permissionsFiltered.Remove(p);
        }

        return permissionsFiltered;
    }

    private const string CacheKey = "Posts-Index-";
    
</script>
