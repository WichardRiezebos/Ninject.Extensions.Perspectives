using Ninject.Syntax;
using System;

namespace Ninject.Extensions.Perspectives
{
    public static class IBindingToSyntaxExtensions
    {
        public static void ToPerspective<T>(this IBindingToSyntax<T> source, Type perspectiveType)
            => source.ToMethod(ctx => Perspective.CreateNew<T>(ctx, perspectiveType));
    }
}
