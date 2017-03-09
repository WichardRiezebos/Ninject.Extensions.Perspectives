using Castle.DynamicProxy;
using Ninject.Activation;
using System;

namespace Ninject.Extensions.Perspectives
{
    internal static class Perspective
    {
        private readonly static Lazy<ProxyGenerator> generatorLazy =
            new Lazy<ProxyGenerator>(() => new ProxyGenerator());

        public static T CreateNew<T>(IContext context, Type targetType)
        {
            var generator = generatorLazy.Value;
            var interceptor = new PerspectiveInterceptor(context, targetType);

            var requestedType = context.Request.Service;

            object subject = generator
                .CreateInterfaceProxyWithoutTarget(requestedType, new[] { typeof(IDisposable) }, interceptor);

            return (T)subject;
        }
    }
}
