using Castle.DynamicProxy;
using Ninject.Activation;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Ninject.Extensions.Perspectives
{
    internal class PerspectivesInterceptor : IPerspectivesInterceptor, IDisposable
    {
        private readonly IContext context;
        private readonly Type serviceType;

        private object instance;

        public PerspectivesInterceptor(IContext context, Type perspectiveType)
        {
            this.context = context;

            serviceType = FlattenType(perspectiveType);
            instance = CreateInstance();
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.ReturnValue = serviceType.InvokeMember(
                    invocation.Method.Name,
                    BindingFlags.InvokeMethod,
                    null,
                    instance,
                    invocation.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();

                throw;
            }
        }

        private object CreateInstance()
        {
            return serviceType.GetConstructors().Any()
                ? context.Kernel.Get(serviceType, context.Parameters.ToArray())
                : null;
        }

        private Type FlattenType(Type original)
        {
            return context.HasInferredGenericArguments
                ? original.MakeGenericType(context.GenericArguments)
                : original;
        }

        public void Dispose()
        {
            (instance as IDisposable)?.Dispose();
            instance = null;
        }
    }
}
