using Ninject.Activation;
using Ninject.Syntax;
using System;

namespace Ninject.Extensions.Perspectives
{
    /// <summary>
    /// Extension methods for <see cref="IBindingToSyntax{TInterface}"/>
    /// </summary>
    public static class BindToExtensions
    {
        /// <summary>
        /// Defines that the interface shall be bound to an existing class without the interface.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="syntax">The syntax.</param>
        /// <param name="perspectiveType">Type of the perspective.</param>
        public static IBindingWhenInNamedWithOrOnSyntax<TInterface> ToPerspective<TInterface>(
            this IBindingToSyntax<TInterface> syntax, Type perspectiveType)
        {
            return syntax.ToMethod(ctx => Activate<TInterface>(ctx, perspectiveType));
        }

        private static TInterface Activate<TInterface>(IContext context, Type perspectiveType)
        {
            return context.Kernel
                .Get<IPerspectivesActivator>()
                .Activate<TInterface>(context, perspectiveType);
        }
    }
}
