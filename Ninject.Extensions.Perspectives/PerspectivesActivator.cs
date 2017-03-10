using Castle.DynamicProxy;
using Ninject.Activation;
using System;

namespace Ninject.Extensions.Perspectives
{
    internal class PerspectivesActivator : IPerspectivesActivator
    {
        private readonly IPerspectivesInterceptorFactory interceptorFactory;

        private ProxyGenerator generator;

        public PerspectivesActivator(
            IPerspectivesInterceptorFactory interceptorFactory)
        {
            this.interceptorFactory = interceptorFactory;

            generator = new ProxyGenerator();
        }

        public TInterface Activate<TInterface>(IContext context, Type perspectiveType)
        {
            var interceptor = interceptorFactory.CreateNew(context, perspectiveType);

            var requestedInterface = context.Request.Service;

            return (TInterface)generator.CreateInterfaceProxyWithoutTarget(
                requestedInterface, new[] { typeof(IDisposable) }, interceptor);
        }
    }
}
