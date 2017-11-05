using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoqEFCoreExtension
{
    /// <summary>
    /// 自定义实现EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class UnitTestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public UnitTestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public UnitTestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new UnitTestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new UnitTestAsyncQueryProvider<T>(this); }
        }
    }
}
