using Ninject.Activation;
using System;

namespace Ninject.Extensions.Perspectives
{
    internal interface IPerspectivesActivator
    {
        TInterface Activate<TInterface>(IContext context, Type perspectiveType);
    }
}
