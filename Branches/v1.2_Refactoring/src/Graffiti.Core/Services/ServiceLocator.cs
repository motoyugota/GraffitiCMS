using System;

namespace Graffiti.Core.Services 
{
    /// <summary>
    /// Provides access to reusable services.
    /// </summary>
    public static class ServiceLocator 
    {
        private static readonly object _lock = new object();

        public static IServiceProvider ServiceProvider { get; private set; }

        public static bool IsInitialized {
            get { return ServiceProvider != null; }
        }

        public static void Initialize(IServiceProvider provider) {
            lock (_lock) {
                if (IsInitialized)
                    throw new InvalidOperationException("The service locator is already initialized");

                ServiceProvider = provider;
            }
        }

        public static void Shutdown() {
            lock (_lock) {
                EnsureInitialized();

                var disposable = ServiceProvider as IDisposable;

                if (disposable != null)
                    disposable.Dispose();

                ServiceProvider = null;
            }
        }

        public static T Get<T>() {
            EnsureInitialized();
            return (T)ServiceProvider.GetService(typeof(T));
        }

        public static object Get(Type type) {
            EnsureInitialized();
            return ServiceProvider.GetService(type);
        }

        private static void EnsureInitialized() {
            if (!IsInitialized)
                throw new InvalidOperationException("The service locator has not been initialized");
        }
    }

}
