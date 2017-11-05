using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoqEFCoreExtension
{
    /// <summary>
    /// 定义关现IAsyncEnumerator<T>类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class UnitTestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public UnitTestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T Current
        {
            get
            {
                return _inner.Current;
            }
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }
    }
}
