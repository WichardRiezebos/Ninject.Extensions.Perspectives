using Castle.DynamicProxy;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ninject.Extensions.Perspectives
{
    internal class PerspectiveInterceptor : IInterceptor, IDisposable
    {
        private readonly IContext context;
        private readonly Type serviceType;

        private Dictionary<string, MethodInfo> methods;

        private object instance;

        public PerspectiveInterceptor(IContext context, Type targetType)
        {
            this.context = context;

            serviceType = FlattenType(targetType);

            methods = GetMethodsResursive(serviceType)
                .GroupBy(m => m.ToString(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => g.First()
                );

            instance = CreateInstance();
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.DeclaringType == typeof(IDisposable))
            {
                Dispose();
            }
            else
            {
                var fingerprint = invocation.Method.ToString();

                var invocator = default(MethodInfo);

                if (methods.TryGetValue(fingerprint, out invocator))
                {
                    invocation.ReturnValue = invocator?
                        .Invoke(instance, invocation.Arguments);
                }
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

        private IReadOnlyCollection<MethodInfo> GetMethodsResursive(Type type)
        {
            return type.GetMethods()
                .Concat(type.GetInterfaces()
                    .SelectMany(GetMethodsResursive)
                )
                .ToList();
        }

        public void Dispose()
        {
            (instance as IDisposable)?.Dispose();
            instance = null;

            methods?.Clear();
            methods = null;
        }
    }
}
