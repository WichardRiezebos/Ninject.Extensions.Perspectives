using System;
using System.Net;
using System.Net.Sockets;

namespace Ninject.Extensions.Perspectives.Perspectives
{
    public interface ISocket : IDisposable
    {
        ProtocolType ProtocolType { get; }

        Socket Accept();

        void Connect(EndPoint remoteEP);
    }
}
