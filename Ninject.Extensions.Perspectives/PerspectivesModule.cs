using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Ninject.Extensions.Perspectives
{
    /// <summary>
    /// Defines the bindings for this extension
    /// </summary>
    public class PerspectivesModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IPerspectivesActivator>().To<PerspectivesActivator>().InSingletonScope();

            Bind<IPerspectivesInterceptorFactory>().ToFactory();
            Bind<IPerspectivesInterceptor>().To<PerspectivesInterceptor>().InTransientScope();
        }
    }
}
