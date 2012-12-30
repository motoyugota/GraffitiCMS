using Ninject;
using Ninject.Modules;

namespace Graffiti.Web 
{
    public static class KernelFactory 
    {
        public static IKernel CreateKernel() {
            var kernel = new StandardKernel();
            kernel.Load("../Modules/*.config");
            return kernel;
        }
    }

    //public class GraffitiModule : NinjectModule {
    //    public override void Load() {
    //        //Bind<IObjectStoreService>().To<ObjectStoreService>().Using<SingletonBehavior>();
    //        //Bind<IObjectStoreRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<IRolePermissionService>().To<RolePermissionService>().Using<SingletonBehavior>();
    //        //Bind<IRoleRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();
    //        //Bind<IPermissionRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<ICategoryService>().To<CategoryService>().Using<SingletonBehavior>();
    //        //Bind<ICategoryRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<IPostService>().To<PostService>().Using<SingletonBehavior>();
    //        //Bind<IPostRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<ICommentService>().To<CommentService>().Using<SingletonBehavior>();
    //        //Bind<ICommentRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<IGraffitiUserService>().To<GraffitiCoreUserService>().Using<SingletonBehavior>();
    //        //Bind<IUserRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<ITagService>().To<TagService>().Using<SingletonBehavior>();
    //        //Bind<ITagRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<IVersionStoreService>().To<VersionStoreService>().Using<SingletonBehavior>();
    //        //Bind<IVersionStoreRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<ILogService>().To<LogService>().Using<SingletonBehavior>();
    //        //Bind<ILogRepository>().To<DataBuddyRepository>().Using<SingletonBehavior>();

    //        //Bind<CategoryAndPostHandler>().To<CategoryAndPostHandler>().Using<SingletonBehavior>();
    //        //Bind<Core.Data>().To<Core.Data>().Using<SingletonBehavior>();
    //    }
    //}



}
