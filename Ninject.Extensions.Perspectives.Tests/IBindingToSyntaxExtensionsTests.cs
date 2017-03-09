using Ninject.Extensions.Perspectives.Cases;
using Ninject.Extensions.Perspectives.Perspectives;
using Ninject.Parameters;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Sockets;

namespace Ninject.Extensions.Perspectives
{
    [TestFixture]
    public class IBindingToSyntaxExtensionsTests
    {
        [Test]
        public void ToPerspective_WhenStaticClass_DoesWhatExpected()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IPath>().ToPerspective(typeof(Path));

            var persp = kernel.Get<IPath>();

            var result = persp.GetRandomFileName();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ToPerspective_WhenInstance_DoesResolve()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ISocket>().ToPerspective(typeof(Socket));

            var persp = kernel.Get<ISocket>(
                new ConstructorArgument("socketType", SocketType.Stream),
                new ConstructorArgument("protocolType", ProtocolType.Tcp));

            using (persp)
            {
                Assert.That(persp, Is.Not.Null);
                Assert.That(persp.ProtocolType, Is.EqualTo(ProtocolType.Tcp));
            }
        }

        [Test]
        public void ToPerspective_WhenInstanceGeneric_DoesResolve()
        {
            var kernel = new StandardKernel();
            kernel.Bind(typeof(IStore<>)).ToPerspective(typeof(Store<>));

            var persp = kernel.Get<IStore<int>>();

            Assert.That(persp, Is.Not.Null);
            Assert.That(persp.Get(), Is.EqualTo(0));
        }

        [Test]
        public void ToPerspective_WhenTransient_CanBeRequested()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IPath>().ToPerspective(typeof(Path));

            var persp = kernel.Get<Repo>();

            Assert.That(persp.DoesRandomWork(), Is.EqualTo(true));
        }
    }
}
