using Ninject.Activation;
using System;

namespace Ninject.Extensions.Perspectives
{
    internal interface IPerspectivesInterceptorFactory
    {
        IPerspectivesInterceptor CreateNew(IContext context, Type perspectiveType);
    }
}
