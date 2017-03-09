using System;
using System.Collections.Generic;

namespace Ninject.Extensions.Perspectives.Perspectives
{
    public interface IStoreItems<T> : IEnumerable<T>
    {
        void Add(T item);
    }
}
