using Ninject.Extensions.Perspectives.Cases;
using Ninject.Extensions.Perspectives.Perspectives;
using Ninject.Parameters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        [Test]
        public void ToPerspective_WhenInterfaceTree_CanBeInvoked()
        {
            var kernel = new StandardKernel();
            kernel.Bind(typeof(IStoreItems<>)).ToPerspective(typeof(List<>));

            var persp = kernel.Get<IStoreItems<int>>();

            persp.Add(1);
            persp.Add(1);
            persp.Add(1);
            persp.Add(1);

            foreach (int val in persp)
            {
                Assert.That(val, Is.GreaterThan(0));
            }
        }

        [Test]
        public void ToPerspective_WithSingletonScope_DoesWhatExpected()
        {
            var kernel = new StandardKernel();
            kernel.Bind(typeof(IStoreItems<>)).ToPerspective(typeof(List<>)).InSingletonScope();

            var request1 = kernel.Get<IStoreItems<int>>();

            request1.Add(1);

            var request2 = kernel.Get<IStoreItems<int>>();
            var nums = request2.ToList();

            Assert.That(nums, Is.Not.Empty);
        }

        [Test]
        public void ToPerspective_WithTransientScope_DoesWhatExpected()
        {
            var kernel = new StandardKernel();
            kernel.Bind(typeof(IStoreItems<>)).ToPerspective(typeof(List<>)).InTransientScope();

            var request1 = kernel.Get<IStoreItems<int>>();

            request1.Add(1);

            var request2 = kernel.Get<IStoreItems<int>>();
            var nums = request2.ToList();

            Assert.That(nums, Is.Empty);
        }

        [Test]
        public void KernelDispose_WhenCalled_DisposesPerspectives()
        {
            var @case = default(ISocket);

            using (var kernel = new StandardKernel())
            {
                kernel.Bind<ISocket>().ToPerspective(typeof(Socket)).InScope(ctx => new object());

                @case = kernel.Get<ISocket>(
                    new ConstructorArgument("socketType", SocketType.Stream),
                    new ConstructorArgument("protocolType", ProtocolType.Tcp));
            }

            Assert.Throws<ObjectDisposedException>(() => @case.Accept());
        }
    }
}
