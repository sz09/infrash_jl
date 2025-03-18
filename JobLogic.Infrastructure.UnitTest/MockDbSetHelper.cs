
using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.UnitTest
{
    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
    public static class MockDbSetHelper
    {
        public static IDbSet<T> ToMockDbSetObject<T>(this List<T> data) where T : class
        {
            var mock = new Mock<IDbSet<T>>();
            var queryData = data.AsQueryable();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryData.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            // This line is new
            mock.As<IDbAsyncEnumerable<T>>()
                .Setup(x => x.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryData.GetEnumerator()));

            // this line is updated
            mock.As<IQueryable<T>>()
                .Setup(x => x.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryData.Provider));

            mock.As<IQueryable<T>>()
                .Setup(x => x.Expression)
                .Returns(queryData.Expression);

            mock.As<IQueryable<T>>()
                .Setup(x => x.ElementType)
                .Returns(queryData.ElementType);

            mock.As<IQueryable<T>>()
                .Setup(x => x.GetEnumerator())
                .Returns(queryData.GetEnumerator());

            return mock.Object;
        }

        public static IDbSet<T> ToMockDbSetObject<T>(this T data) where T : class
        {
            return ToMockDbSetObject(new List<T> { data });
        }

        public static void SetUpDbSetWithRandom<T, TPropertyItem>(this Mock<T> mock, Expression<Func<T, IDbSet<TPropertyItem>>> expression, int? count = null) 
            where T:class
            where TPropertyItem:class

        {
            if (count == null) count = ValueGenerator.Int(0, 5);
            if(count == 0)
                mock.Setup(expression).Returns((new List<TPropertyItem>().ToMockDbSetObject()));
            else
                mock.Setup(expression).Returns((ValueGenerator.CreateMany<TPropertyItem>(count.Value).ToList().ToMockDbSetObject()));
        }

        public static void SetUpDbSetWithValue<T, TPropertyItem>(this Mock<T> mock, Expression<Func<T, IDbSet<TPropertyItem>>> expression, params TPropertyItem[] items)
            where T : class
            where TPropertyItem : class

        {
                mock.Setup(expression).Returns((items.ToList().ToMockDbSetObject()));
        }

        public static Mock<IDbSet<T>> ToMockDbSet<T>(this List<T> data) where T : class
        {
            var mock = new Mock<IDbSet<T>>();
            var queryData = data.AsQueryable();
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            mock.As<IDbAsyncEnumerable<T>>()
                .Setup(x => x.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryData.GetEnumerator()));

            mock.As<IQueryable<T>>()
                .Setup(x => x.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryData.Provider));

            return mock;
        }
    }
}
